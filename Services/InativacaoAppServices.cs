using APICadastro.Context;
using APICadastro.Models;
using FluentValidation;

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

        _context.Inativacoes.Add(inativacao);
        _context.SaveChanges();
        return null;
    }

    public IEnumerable<string> AlteraInativacao(int id, Inativacao inativacao)
    {    
        var result = _validator.Validate(inativacao);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var inativacaoOriginal = _context.Inativacoes.Find(id);

        inativacaoOriginal.DataFim = inativacao.DataFim;
        _context.Update(inativacaoOriginal);
        _context.SaveChanges();
        return null;
    }
}
