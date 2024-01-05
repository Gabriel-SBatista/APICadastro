using APICadastro.Models;
using APICadastro.Repositories;
using FluentValidation;
using MongoDB.Bson;

namespace APICadastro.Services;

public class InativacaoAppServices
{
    private readonly InativacaoRepository _inativacaoRepository;
    private readonly UsuarioAppServices _usuarioServices;
    private readonly IValidator<Inativacao> _validator;

    public InativacaoAppServices(UsuarioAppServices usuarioServices, InativacaoRepository inativacaoRepository, IValidator<Inativacao> validator)
    {
        _usuarioServices = usuarioServices;
        _inativacaoRepository = inativacaoRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<string>?> InativaConta(Inativacao inativacao)
    {
        
        var result = _validator.Validate(inativacao);

        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var usuario = await _usuarioServices.BuscaUsuarioId(inativacao.UsuarioId);

        if (usuario is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        var inativacoesDoUsuario = await BuscaInativacaoPorIdDeUsuario(usuario.UsuarioId);

        foreach (var item in inativacoesDoUsuario)
        {
            if (item.DataFim > DateTime.Now || item.DataFim == null)
            {
                List<string> message = new List<string>();
                message.Add("Usuario ja esta inativo...");
                return message;
            }
        }

        await _inativacaoRepository.Insert(inativacao);
        return null;
        
    }

    public async Task<string?> AlteraInativacao(ObjectId id, DateTime dataFim)
    {
        var usuario = await _usuarioServices.BuscaUsuarioId(id);

        if (usuario is null)
        {          
            return("Usuario não encontrado...");
        }

        var inativacoesDoUsuario = await _inativacaoRepository.GetByUserId(id);

        foreach (var item in inativacoesDoUsuario)
        {
            if(item.DataFim > DateTime.Now || item.DataFim == null)
            {
                item.DataFim = dataFim;
                await _inativacaoRepository.Update(item.InativacaoId, item);
                return null;
            }
        }

        return("Usuario não esta inativado...");
    }

    public async Task<IEnumerable<Inativacao>> BuscaInativacaoPorIdDeUsuario(ObjectId id)
    {
        return await _inativacaoRepository.GetByUserId(id);
    }
}
