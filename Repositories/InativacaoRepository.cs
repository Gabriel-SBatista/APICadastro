using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class InativacaoRepository
{
    private readonly MongoDbContext _context;

    public InativacaoRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inativacao>> GetAll()
    {
        return await _context.Inativacoes.Find(i => true).ToListAsync();
    }

    public async Task<Inativacao> GetById(ObjectId id)
    {
        return await _context.Inativacoes.Find(i => i.InativacaoId == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Inativacao>> GetByUserId(ObjectId id)
    {
        return await _context.Inativacoes.Find(i => i.UsuarioId == id).ToListAsync();
    }

    public async Task Insert(Inativacao inativacao)
    {
        inativacao.InativacaoId = ObjectId.GenerateNewId();
        await _context.Inativacoes.InsertOneAsync(inativacao);
    }

    public async Task Update(ObjectId id, Inativacao inativacao)
    {
        await _context.Inativacoes.ReplaceOneAsync(i => i.InativacaoId == id, inativacao);
    }
}
