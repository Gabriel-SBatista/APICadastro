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

    public IMongoCollection<User> Users => _database.GetCollection<User>("Usuarios");
    public IMongoCollection<Company> Companies => _database.GetCollection<Company>("Empresas");
    public IMongoCollection<Inactivation> Inactivations => _database.GetCollection<Inactivation>("Inativacoes");
    public IMongoCollection<AccessType> AccessTypes => _database.GetCollection<AccessType>("TiposAcesso");
}
