using APICadastro.Models;
using APICadastro.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace APICadastro.Controllers;

[ApiController]
public class CompanyController : ControllerBase
{
    private readonly CompanyServices _companyServices;

    public CompanyController(CompanyServices companyServices)
    {
        _companyServices = companyServices;
    }

    [HttpGet("empresas")]
    public async Task<ActionResult> Get()
    {
        var companies = await _companyServices.FindCompanies();

        if (!companies.Any() || companies is null)
        {
            return NotFound();
        }

        return Ok(companies);
    }

    [HttpGet("empresa/{id}")]
    public async Task<ActionResult> GetId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var company = await _companyServices.FindCompanyById(objectId);

        if (company is null)
        {
            return NotFound("Empresa não encontrada...");
        }

        return Ok(company);
    }

    [HttpPost("empresa")]
    public async Task<ActionResult> Post(Company company)
    {
        var errors = await _companyServices.RegisterCompany(company);

        if (errors is null)
        {
            return Ok(company);
        }

        return BadRequest(errors);
    }

    [HttpPut("empresa/{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] Company company)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var errors = await _companyServices.UpdateCompany(objectId, company);

        if (errors is null)
        {
            return Ok(company);
        }

        return BadRequest(errors);
    }

    [HttpDelete("empresa/{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var error = await _companyServices.DeleteCompany(objectId);

        if (error)
        {
            return NotFound("Empresa não encontrada...");
        }

        return Ok("Empresa deletada com sucesso!");
    }
}
