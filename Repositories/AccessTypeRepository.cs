using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class AccessTypeRepository
{
    private readonly MongoDbContext _context;

    public AccessTypeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccessType>> GetAll()
    {
        return await _context.AccessTypes.Find(ta => true).ToListAsync();
    }

    public async Task<AccessType> GetById(ObjectId id)
    {
        return await _context.AccessTypes.Find(ta => ta.AccessTypeId == id).FirstOrDefaultAsync();
    }

    public async Task Insert(AccessType accessType)
    {
        accessType.AccessTypeId = ObjectId.GenerateNewId();
        await _context.AccessTypes.InsertOneAsync(accessType);
    }

    public async Task Update(ObjectId id, AccessType accessType)
    {
        await _context.AccessTypes.ReplaceOneAsync(ta => ta.AccessTypeId == id, accessType);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.AccessTypes.DeleteOneAsync(ta => ta.AccessTypeId== id);
    }
}
