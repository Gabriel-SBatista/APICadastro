using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class UserRepository
{
    private readonly MongoDbContext _context;

    public UserRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.Find(u => true).ToListAsync();
    }

    public async Task<User> GetById(ObjectId id)
    {
        return await _context.Users.Find(u => u.UserId == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync(); 
    }

    public async Task Insert(User user)
    {
        user.UserId = ObjectId.GenerateNewId();
        await _context.Users.InsertOneAsync(user);
    }

    public async Task Update(ObjectId id, User user)
    {
        await _context.Users.ReplaceOneAsync(u => u.UserId == id, user);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.Users.DeleteOneAsync(u => u.UserId == id);
    }
}
