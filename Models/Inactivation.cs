using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class Inactivation
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InactivationId { get; set; }
    [Column("Data_Inicio")]
    public DateTime StartDate { get; set; }
    [Column("Data_Fim")]
    public DateTime? EndDate { get; set; }
    [Column("Usuario")]
    public string UserId { get; set; }
}

public class InactivationValidator : AbstractValidator<Inactivation>
{
    public InactivationValidator()
    {
        RuleFor(i => i.StartDate).NotEmpty();
        RuleFor(i => i.UserId).NotEmpty();
    }
}
