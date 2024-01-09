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

    public async Task<IEnumerable<Company>> GetAll()
    {
        return await _context.Companies.Find(c => true).ToListAsync();
    }

    public async Task<Company> GetById(ObjectId? id)
    {
        return await _context.Companies.Find(c => c.CompanyId == id).FirstOrDefaultAsync();
    }

    public async Task<Company> GetByCnpj(long cnpj)
    {
        return await _context.Companies.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
    }

    public async Task Insert(Company company)
    {
        company.CompanyId = ObjectId.GenerateNewId();
        await _context.Companies.InsertOneAsync(company);
    }

    public async Task Update(ObjectId id, Company company)
    {
        await _context.Companies.ReplaceOneAsync(c => c.CompanyId == id, company);
    }

    public async Task Delete(ObjectId id)
    {
        await _context.Companies.DeleteOneAsync(c => c.CompanyId == id);
    }
}
