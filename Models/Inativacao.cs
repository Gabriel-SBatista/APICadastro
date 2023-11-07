using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace APICadastro.Models;

public class Inativacao
{
    public int InativacaoID { get; set; }
    [Required]
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}

public class InativacaoValidator : AbstractValidator<Inativacao>
{
    public InativacaoValidator()
    {
        RuleFor(i => i.DataInicio).NotEmpty();
    }
}
