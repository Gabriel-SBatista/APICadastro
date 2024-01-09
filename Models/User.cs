using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class User
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId UserId { get; set; }
    [Column("Nome")]
    public string Name { get; set; }
    [Column("Email")]
    public string Email { get; set; }
    [Column("Senha")]
    public string Password { get; set; }
    [Column("TipoAcesso")]
    public string AccessTypeId { get; set; }
    [Column("Empresa")]
    public string CompanyId { get; set; }

    public bool VerifyPassword()
    {
        int number = 0;
        int uppercase = 0;
        int lowercase = 0;

        foreach (var c in Password)
        {
            if (Char.IsDigit(c))
                number++;
            else if (Char.IsLower(c))
                lowercase++;
            else if (Char.IsUpper(c))
                uppercase++;
        }

        if (number > 0 && uppercase > 0 && lowercase > 0)
            return true;
        return false;
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Name).NotEmpty();
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).MinimumLength(7);
        RuleFor(u => u.VerifyPassword()).Must(u => u == true).WithMessage("Senha deve conter letra minuscula, maiuscula e numero");
    }
}
