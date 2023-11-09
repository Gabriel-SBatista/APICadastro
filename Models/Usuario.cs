using FluentValidation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICadastro.Models;

public class Usuario
{

    public Usuario()
    {
        Inativacoes = new Collection<Inativacao>();
    }
    public int UsuarioId { get; set; }
    [Required]
    [MaxLength(20)]
    public string Nome { get; set; }
    [Required]
    [MaxLength(30)]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }
    public ICollection<Inativacao> Inativacoes { get; set; }

    public bool VerificaSenha()
    {
        int numero = 0;
        int maiscula = 0;
        int minuscula = 0;

        foreach (var c in this.Senha)
        {
            if (Char.IsDigit(c))
                numero++;
            else if (Char.IsLower(c))
                minuscula++;
            else if (Char.IsUpper(c))
                maiscula++;
        }

        if (numero > 0 && maiscula > 0 && minuscula > 0)
            return true;
        return false;
    }
}

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nome).NotEmpty();
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Senha).MinimumLength(7);
        RuleFor(u => u.VerificaSenha()).Must(u => u == true).WithMessage("Senha deve conter letra minuscula, maiuscula e numero");
    }
}
