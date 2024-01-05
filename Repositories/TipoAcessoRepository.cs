using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class TipoAcessoRepository
{
    private readonly MongoDbContext _context;

    public TipoAcessoRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoAcesso>> GetAll()
    {
        return await _context.TiposAcesso.Find(ta => true).ToListAsync();
    }

    public async Task<TipoAcesso> GetById(ObjectId id)
    {
        return await _context.TiposAcesso.Find(ta => ta.TipoAcessoId == id).FirstOrDefaultAsync();
    }

    public async Task Insert(TipoAcesso tipoAcesso)
    {
        tipoAcesso.TipoAcessoId = ObjectId.GenerateNewId();
        await _context.TiposAcesso.InsertOneAsync(tipoAcesso);
    }

    public async Task Update(ObjectId id, TipoAcesso tipoAcesso)
    {
        await _context.TiposAcesso.ReplaceOneAsync(ta => ta.TipoAcessoId == id, tipoAcesso);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.TiposAcesso.DeleteOneAsync(ta => ta.TipoAcessoId== id);
    }
}
