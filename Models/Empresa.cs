using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class Empresa
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId CompanyId { get; set; }
    [Column("Nome")]
    public string Nome { get; set; }
    [Column("Cnpj")]
    public long Cnpj { get; set; }
    [Column("Usuarios")]
    public ICollection<Usuario> Usuarios { get; set; }

    public bool ValidarCnpj()
    {
        string stringCnpj = Cnpj.ToString();

        if (stringCnpj.Count() != 14)
        {
            return false;
        }

        return true;
    }
}

public class EmpresaValidator : AbstractValidator<Empresa>
{
    public EmpresaValidator()
    {
        RuleFor(c => c.Nome).NotEmpty();
        RuleFor(c => c.Cnpj).NotEmpty();
        RuleFor(c => c.ValidarCnpj()).Must(c => c == true).WithMessage("Cnpj invalido");
    }
}
