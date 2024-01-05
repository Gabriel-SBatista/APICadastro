using APICadastro.Context;
using APICadastro.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APICadastro.Repositories;

public class CompanyRepository
{
    private readonly MongoDbContext _context;

    public CompanyRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Empresa>> GetAll()
    {
        return await _context.Empresas.Find(c => true).ToListAsync();
    }

    public async Task<Empresa> GetById(ObjectId id)
    {
        return await _context.Empresas.Find(c => c.CompanyId == id).FirstOrDefaultAsync();
    }

    public async Task<Empresa> GetByCnpj(long cnpj)
    {
        return await _context.Empresas.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
    }

    public async Task Insert(Empresa company)
    {
        company.CompanyId = ObjectId.GenerateNewId();
        await _context.Empresas.InsertOneAsync(company);
    }

    public async Task Update(ObjectId id, Empresa company)
    {
        await _context.Empresas.ReplaceOneAsync(c => c.CompanyId == id, company);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.Empresas.DeleteOneAsync(c => c.CompanyId == id);
    }
}
