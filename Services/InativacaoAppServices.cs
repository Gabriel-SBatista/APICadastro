using APICadastro.Context;
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
        var usuario = _context.Usuarios.Include(u => u.Inativacoes).FirstOrDefault(u => u.UsuarioId ==  inativacao.UsuarioId);

        if(usuario is null)
        {
            List<string> message = new List<string>();
            message.Add("Usuario não encontrado...");
            return message;
        }

        var inativacoes = usuario.Inativacoes;

        foreach (var item in inativacoes)
        {
            if (item.DataFim == null || item.DataFim > DateTime.Now)
            {
                List<string> message = new List<string>();
                message.Add("Usuario ja inativo...");
                return message;
            }
        }

        var result = _validator.Validate(inativacao);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
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

        var inativacoes = usuario.Inativacoes;

        foreach (var item in inativacoes)
        {
            if (item.DataFim != null || item.DataFim > DateTime.Now)
            {
                item.DataFim = dataFim;
                _context.Update(item);
                return null;
            }
        }

        message.Add("Usuario ja ativo...");
        return message;
    }
}
