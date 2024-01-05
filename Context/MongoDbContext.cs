using APICadastro.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace APICadastro.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
    public IMongoCollection<Empresa> Empresas => _database.GetCollection<Empresa>("Empresas");
    public IMongoCollection<Inativacao> Inativacoes => _database.GetCollection<Inativacao>("Inativacoes");
    public IMongoCollection<TipoAcesso> TiposAcesso => _database.GetCollection<TipoAcesso>("TiposAcesso");
}
