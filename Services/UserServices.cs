using APICadastro.Models;
using APICadastro.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Isopoh.Cryptography.Argon2;
using MongoDB.Bson;

namespace APICadastro.Services;

public class UserServices
{
    private readonly UserRepository _userRepository;
    private readonly IValidator<User> _validator;
    private readonly InactivationRepository _inactivationRepository;
    private readonly TokenServices _tokenServices;
    public UserServices(UserRepository userRepository, IValidator<User> validator, InactivationRepository inactivationRepository, TokenServices tokenServices)
    {
        _userRepository = userRepository;
        _validator = validator;
        _inactivationRepository = inactivationRepository;
        _tokenServices = tokenServices;
    }
    public async Task<IEnumerable<string>?> RegisterUser(User user)
    {
        ValidationResult result = _validator.Validate(user);
        if(!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var findByEmail = await _userRepository.GetByEmail(user.Email);

        if (findByEmail != null)
        {
            List<string> message = new List<string>();
            message.Add("Email ja esta em uso...");
            return message;
        }

        user.Password = Argon2.Hash(user.Password);

        await _userRepository.Insert(user);
        return null;
    }

    public async Task<IEnumerable<string>?> UpdateUser(ObjectId id, User user)
    {
        var actualUser = await _userRepository.GetById(id);
        if(actualUser is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        if(actualUser.Email != user.Email)
        {
            var findByEmail = await _userRepository.GetByEmail(user.Email);

            if (findByEmail != null)
            {
                List<string> message = new List<string>();
                message.Add("Email ja esta em uso...");
                return message;
            }
        }      

        ValidationResult result = _validator.Validate(user);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        await _userRepository.Update(id, user);
        return null;
    }

    public async Task<dynamic?> LoginUser(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user is null)
        {
            return null;
        }

        if (Argon2.Verify(user.Password, password))
        {
            var userInactivations = await _inactivationRepository.GetByUserId(user.UserId.ToString());

            foreach (var inactivation in userInactivations)
            {
                if (inactivation.EndDate > DateTime.UtcNow || inactivation.EndDate is null)
                {
                    return null;
                }
            }

            var token = await _tokenServices.GenerateToken(user);
            return new
            {
                nome = user.Name,
                email = user.Email,
                token = token
            };
        }
        else
            return null;
    }

    public async Task<bool> DeleteUser(ObjectId id)
    {
        User usuario = await _userRepository.GetById(id);
        if (usuario is null)
        {
            return true;
        }

        await _userRepository.Delete(id);
        return false;
    }

    public async Task<IEnumerable<User>> SearchUsers()
    {
        var usuarios = await _userRepository.GetAll();

        return usuarios;
    }

    public async Task<User> SearchUserById(ObjectId id)
    {
        var usuario = await _userRepository.GetById(id);

        return usuario;
    }
}
