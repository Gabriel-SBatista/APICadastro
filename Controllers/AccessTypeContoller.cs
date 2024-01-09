using APICadastro.Models;
using APICadastro.Repositories;
using APICadastro.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace APICadastro.Controllers;

[ApiController]
public class AccessTypeContoller : ControllerBase
{
    private readonly AccessTypeRepository _repository;

    public AccessTypeContoller(AccessTypeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("tiposAcesso")]
    public async Task<ActionResult> Get()
    {
        var tiposAcesso = await _repository.GetAll();

        if (!tiposAcesso.Any() || tiposAcesso is null)
        {
            return NotFound();
        }

        return Ok(tiposAcesso);
    }

    [HttpGet("accessType/{id}")]
    public async Task<ActionResult> GetId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var accessType = await _repository.GetById(objectId);

        if (accessType is null)
        {
            return NotFound("Tipo não encontrado...");
        }

        return Ok(accessType);
    }

    [HttpPost("accessType")]
    public async Task<ActionResult> Post(AccessType accessType)
    {
        await _repository.Insert(accessType);

        return Ok(accessType);
    }

    [HttpPut("accessType/{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] AccessType accessType)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        await _repository.Update(objectId, accessType);

        return Ok(accessType);
    }

    [HttpDelete("accessType/{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        await _repository.Delete(objectId);

        return Ok("Tipo deletado com sucesso!");
    }
}
