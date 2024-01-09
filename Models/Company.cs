using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class Company
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId CompanyId { get; set; }
    [Column("Nome")]
    public string Name { get; set; }
    [Column("Cnpj")]
    public long Cnpj { get; set; }

    public bool ValidateCnpj()
    {
        string stringCnpj = Cnpj.ToString();

        if (stringCnpj.Count() != 14)
        {
            return false;
        }

        return true;
    }
}

public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Cnpj).NotEmpty();
        RuleFor(c => c.ValidateCnpj()).Must(c => c == true).WithMessage("Cnpj invalido");
    }
}
