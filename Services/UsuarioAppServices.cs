using APICadastro.Context;
using APICadastro.Models;
using APICadastro.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace APICadastro.Services;

public class UsuarioAppServices
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly IValidator<Usuario> _validator;
    private readonly InativacaoRepository _inativacaoRepository;
    public UsuarioAppServices(UsuarioRepository usuarioRepository, IValidator<Usuario> validator, InativacaoRepository inativacaoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _validator = validator;
        _inativacaoRepository = inativacaoRepository;
    }
    public async Task<IEnumerable<string>?> CadastraUsuario(Usuario usuario)
    {
        ValidationResult result = _validator.Validate(usuario);
        if(!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var findByEmail = await _usuarioRepository.GetByEmail(usuario.Email);

        if (findByEmail != null)
        {
            List<string> message = new List<string>();
            message.Add("Email ja esta em uso...");
            return message;
        }

        usuario.Senha = Argon2.Hash(usuario.Senha);

        await _usuarioRepository.Insert(usuario);
        return null;
    }

    public async Task<IEnumerable<string>?> AtualizaUsuario(ObjectId id, Usuario usuario)
    {
        var usuarioOriginal = await _usuarioRepository.GetById(id);
        if(usuarioOriginal is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        if(usuarioOriginal.Email != usuario.Email)
        {
            var findByEmail = await _usuarioRepository.GetByEmail(usuario.Email);

            if (findByEmail != null)
            {
                List<string> message = new List<string>();
                message.Add("Email ja esta em uso...");
                return message;
            }
        }      

        ValidationResult result = _validator.Validate(usuario);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        await _usuarioRepository.Update(id, usuario);
        return null;
    }

    public async Task<dynamic?> LogaUsuario(string email, string senha)
    {
        var usuario = await _usuarioRepository.GetByEmail(email);
        if (usuario is null)
        {
            return null;
        }

        if (Argon2.Verify(usuario.Senha, senha))
        {
            var inativacooesDoUsuario = await _inativacaoRepository.GetByUserId(usuario.UsuarioId);

            foreach (var inativacao in inativacooesDoUsuario)
            {
                if (inativacao.DataFim > DateTime.Now)
                {
                    return null;
                }
            }

            var token = TokenAppServices.GenerateToken(usuario);
            return new
            {
                nome = usuario.Nome,
                email = usuario.Email,
                token = token
            };
        }
        else
            return null;
    }

    public async Task<bool> DeletaUsuario(ObjectId id)
    {
        Usuario usuario = await _usuarioRepository.GetById(id);
        if (usuario is null)
        {
            return true;
        }

        await _usuarioRepository.Delete(id);
        return false;
    }

    public async Task<IEnumerable<Usuario>> BuscaUsuarios()
    {
        var usuarios = await _usuarioRepository.GetAll();

        return usuarios;
    }

    public async Task<Usuario> BuscaUsuarioId(ObjectId id)
    {
        var usuario = await _usuarioRepository.GetById(id);

        return usuario;
    }

    /*public List<Usuario> BuscaInativacoes()
    {
        var usuarios = _context.Usuarios.AsNoTrackingWithIdentityResolution().Include(u => u.Inativacoes).ToList();

        return usuarios;
    }*/
}
