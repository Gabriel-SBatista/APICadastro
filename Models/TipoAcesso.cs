using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICadastro.Models;

public class TipoAcesso
{
    [Column("Id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId TipoAcessoId { get; set; }
    [Column("Tipo_Acesso")]
    public string Tipos { get; set; }
}
