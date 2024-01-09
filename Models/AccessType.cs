using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class AccessType
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId AccessTypeId { get; set; }
    [Column("Tipo_Acesso")]
    public string Type { get; set; }
}
