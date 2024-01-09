using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class InactivationRepository
{
    private readonly MongoDbContext _context;

    public InactivationRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inactivation>> GetAll()
    {
        return await _context.Inactivations.Find(i => true).ToListAsync();
    }

    public async Task<Inactivation> GetById(ObjectId id)
    {
        return await _context.Inactivations.Find(i => i.InactivationId == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Inactivation>> GetByUserId(string id)
    {
        return await _context.Inactivations.Find(i => i.UserId == id).ToListAsync();
    }

    public async Task Insert(Inactivation inactivation)
    {
        inactivation.InactivationId = ObjectId.GenerateNewId();
        await _context.Inactivations.InsertOneAsync(inactivation);
    }

    public async Task Update(ObjectId id, Inactivation inactivation)
    {
        await _context.Inactivations.ReplaceOneAsync(i => i.InactivationId == id, inactivation);
    }
}
