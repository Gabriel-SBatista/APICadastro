using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICadastro.Models;

public class Inativacao
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InativacaoId { get; set; }
    [Column("Data_Inicio")]
    public DateTime DataInicio { get; set; }
    [Column("Data_Fim")]
    public DateTime? DataFim { get; set; }
    [Column("Usuario")]
    public ObjectId UsuarioId { get; set; }
}

public class InativacaoValidator : AbstractValidator<Inativacao>
{
    public InativacaoValidator()
    {
        RuleFor(i => i.DataInicio).NotEmpty();
        RuleFor(i => i.UsuarioId).NotEmpty();
    }
}
