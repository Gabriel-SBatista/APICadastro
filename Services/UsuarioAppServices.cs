using APICadastro.Context;
using APICadastro.Models;
using FluentValidation;
using FluentValidation.Results;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace APICadastro.Services;

public class UsuarioAppServices
{
    private readonly AppDbContext _context;
    private readonly IValidator<Usuario> _validator;
    public UsuarioAppServices(AppDbContext context, IValidator<Usuario> validator)
    {
        _context = context;
        _validator = validator;
    }
    public IEnumerable<string> CadastraUsuario(Usuario usuario)
    {
        ValidationResult result = _validator.Validate(usuario);
        if(!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var usuarioEmail = _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);

        if (usuarioEmail != null)
        {
            List<string> message = new List<string>();
            message.Add("Email ja esta em uso...");
            return message;
        }

        usuario.Senha = Argon2.Hash(usuario.Senha);

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return null;
    }

    public IEnumerable<string> AtualizaUsuario(int id, Usuario usuario)
    {
        var usuarioOriginal = _context.Usuarios.Find(id);
        if(usuarioOriginal is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        var usuarioEmail = _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);

        if (usuarioEmail != null)
        {
            List<string> message = new List<string>();
            message.Add("Email ja esta em uso...");
            return message;
        }

        ValidationResult result = _validator.Validate(usuario);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        usuarioOriginal.Nome = usuario.Nome;
        usuarioOriginal.Senha = Argon2.Hash(usuario.Senha);
        usuarioOriginal.Email = usuario.Email;

        _context.Update(usuarioOriginal);
        _context.SaveChanges();
        return null;
    }

    public dynamic LogaUsuario(string email, string senha)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
        if (usuario is null)
        {
            return null;
        }

        if (Argon2.Verify(usuario.Senha, senha))
        {
            var token = TokenAppServices.GenerateToken(usuario);
            return new
            {
                usuario = usuario,
                token = token
            };
        }
        else
            return null;
    }

    public bool DeletaUsuario(int id)
    {
        Usuario usuario = _context.Usuarios.Find(id);
        if (usuario is null)
        {
            return true;
        }

        _context.Remove(usuario);
        _context.SaveChanges();
        return false;
    }

    public List<Usuario> BuscaUsuarios()
    {
        var usuarios = _context.Usuarios.AsNoTrackingWithIdentityResolution().ToList();

        return usuarios;
    }

    public List<Usuario> BuscaUsuariosNome(string nome)
    {
        var usuarios = _context.Usuarios.AsNoTrackingWithIdentityResolution().Where(u => u.Nome == nome).ToList();

        return usuarios;
    }

    public Usuario BuscaUsuarioEmail(string email)
    {
        var usuario = _context.Usuarios.AsNoTrackingWithIdentityResolution().FirstOrDefault(u => u.Email == email);

        return usuario;
    }

    public Usuario BuscaUsuarioId(int id)
    {
        var usuario = _context.Usuarios.AsNoTrackingWithIdentityResolution().FirstOrDefault(u => u.UsuarioId == id);

        return usuario;
    }

    public List<Usuario> BuscaUsuariosInativados()
    {
        var usuarios = _context.Usuarios.AsNoTrackingWithIdentityResolution().Include(u => u.Inativacoes).ToList();

        return usuarios;
    }
}
