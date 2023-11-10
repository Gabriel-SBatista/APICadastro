using APICadastro.Context;
using APICadastro.Migrations;
using APICadastro.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APICadastro.Services;

public class InativacaoAppServices
{
    private readonly AppDbContext _context;
    private readonly IValidator<Inativacao> _validator;

    public InativacaoAppServices(AppDbContext context, IValidator<Inativacao> validator)
    {
        _context = context;
        _validator = validator;
    }

    public IEnumerable<string> InativaConta(Inativacao inativacao)
    {
        
        var result = _validator.Validate(inativacao);

        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var usuario = _context.Usuarios.Include(u => u.Inativacoes).FirstOrDefault(u => u.UsuarioId ==  inativacao.UsuarioId);

        if (usuario is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        foreach (var item in usuario.Inativacoes)
        {
            if (item.DataFim > DateTime.Now || item.DataFim == null)
            {
                List<string> message = new List<string>();
                message.Add("Usuario ja esta inativo...");
                return message;
            }
        }

        _context.Inativacoes.Add(inativacao);
        _context.SaveChanges();
        return null;
        
    }

    public IEnumerable<string> AlteraInativacao(int id, DateTime dataFim)
    {
        var usuario = _context.Usuarios.Include(u => u.Inativacoes).FirstOrDefault(u => u.UsuarioId == id);
        List<string> message = new List<string>();

        if (usuario is null)
        {          
            message.Add("Usuario não encontrado...");
            return message;
        }

        foreach (var item in usuario.Inativacoes)
        {
            if(item.DataFim > DateTime.Now || item.DataFim == null)
            {
                item.DataFim = dataFim;
                _context.Update(item);
                _context.SaveChanges();
                return null;
            }
        }

        message.Add("O usuario não esta inativo");
        return message;
    }
}
