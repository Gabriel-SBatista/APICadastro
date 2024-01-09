using APICadastro.Models;
using APICadastro.Repositories;
using FluentValidation;
using MongoDB.Bson;

namespace APICadastro.Services;

public class InactivationServices
{
    private readonly InactivationRepository _inativacaoRepository;
    private readonly UserServices _userServices;
    private readonly IValidator<Inactivation> _validator;

    public InactivationServices(UserServices userServices, InactivationRepository inativacaoRepository, IValidator<Inactivation> validator)
    {
        _userServices = userServices;
        _inativacaoRepository = inativacaoRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<string>?> InactiveAccount(Inactivation inactivation)
    {
        
        var result = _validator.Validate(inactivation);

        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var user = await _userServices.SearchUserById(ObjectId.Parse(inactivation.UserId));

        if (user is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        var userInactivations = await SearchInactivationByUserId(user.UserId.ToString());

        foreach (var item in userInactivations)
        {
            if (item.EndDate > DateTime.UtcNow || item.EndDate == null)
            {
                List<string> message = new List<string>();
                message.Add("Usuario ja esta inativo...");
                return message;
            }
        }

        await _inativacaoRepository.Insert(inactivation);
        return null;
        
    }

    public async Task<string?> AlterInactivation(string id, DateTime? endDate)
    {
        var user = await _userServices.SearchUserById(ObjectId.Parse(id));

        if (user is null)
        {          
            return("Usuario não encontrado...");
        }

        var userInactivations = await _inativacaoRepository.GetByUserId(id);

        foreach (var item in userInactivations)
        {
            if(item.EndDate > DateTime.UtcNow || item.EndDate == null)
            {
                item.EndDate = endDate;
                await _inativacaoRepository.Update(item.InactivationId, item);
                return null;
            }
        }

        return("Usuario não esta inativado...");
    }

    public async Task<IEnumerable<Inactivation>> SearchInactivationByUserId(string id)
    {
        return await _inativacaoRepository.GetByUserId(id);
    }
}
