using APICadastro.Models;
using APICadastro.Repositories;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson;

namespace APICadastro.Services;

public class CompanyAppServices
{
    private readonly CompanyRepository _companyRepository;
    private readonly IValidator<Empresa> _validator;

    public CompanyAppServices(CompanyRepository companyRepository, IValidator<Empresa> validator)
    {
        _companyRepository = companyRepository;
        _validator = validator;
    }

    public async Task<IEnumerable<string>?> RegisterCompany(Empresa company)
    {
        ValidationResult result = _validator.Validate(company);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        var findByCnpj = _companyRepository.GetByCnpj(company.Cnpj);

        if (findByCnpj != null)
        {
            List<string> message = new List<string>();
            message.Add("Cnpj ja cadastrado...");
            return message;
        }

        await _companyRepository.Insert(company);
        return null;
    }

    public async Task<IEnumerable<string>?> UpdateCompany(ObjectId id, Empresa company)
    {
        var actualCompany = await _companyRepository.GetById(id);
        if (actualCompany is null)
        {
            List<string> message = new List<string>();
            message.Add("Empresa não encontrada...");
            return message;
        }

        if (actualCompany.Cnpj != company.Cnpj)
        {
            List<string> message = new List<string>();
            message.Add("Cnpj não pode ser alterado...");
            return message;
        }

        ValidationResult result = _validator.Validate(company);
        if (!result.IsValid)
        {
            var message = result.Errors.Select(e => e.ErrorMessage);
            return message;
        }

        await _companyRepository.Update(id, company);
        return null;
    }

    public async Task<bool> DeleteCompany(ObjectId id)
    {
        var company = await _companyRepository.GetById(id);
        if (company is null)
        {
            return true;
        }

        await _companyRepository.Delete(id);
        return false;
    }

    public async Task<IEnumerable<Empresa>> FindCompanies()
    { 
        var companies = await _companyRepository.GetAll();

        return companies;
    }

    public async Task<Empresa> FindCompanyById(ObjectId id)
    {
        var company = await _companyRepository.GetById(id);

        return company;
    }
}
