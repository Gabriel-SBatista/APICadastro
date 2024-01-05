using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class UsuarioRepository
{
    private readonly MongoDbContext _context;

    public UsuarioRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAll()
    {
        return await _context.Usuarios.Find(u => true).ToListAsync();
    }

    public async Task<Usuario> GetById(ObjectId id)
    {
        return await _context.Usuarios.Find(u => u.UsuarioId == id).FirstOrDefaultAsync();
    }

    public async Task<Usuario> GetByEmail(string email)
    {
        return await _context.Usuarios.Find(u => u.Email == email).FirstOrDefaultAsync(); 
    }

    public async Task Insert(Usuario usuario)
    {
        usuario.UsuarioId = ObjectId.GenerateNewId();
        await _context.Usuarios.InsertOneAsync(usuario);
    }

    public async Task Update(ObjectId id, Usuario usuario)
    {
        await _context.Usuarios.ReplaceOneAsync(u => u.UsuarioId == id, usuario);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.Usuarios.DeleteOneAsync(u => u.UsuarioId == id);
    }
}
