using APICadastro.Models;
using APICadastro.Repositories;
using APICadastro.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace APICadastro.Controllers;

[ApiController]
public class TipoAcessoContoller : ControllerBase
{
    private readonly TipoAcessoRepository _repository;

    public TipoAcessoContoller(TipoAcessoRepository repository)
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

    [HttpGet("tipoAcesso/{id}")]
    public async Task<ActionResult> GetId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var tipoAcesso = await _repository.GetById(objectId);

        if (tipoAcesso is null)
        {
            return NotFound("Tipo não encontrado...");
        }

        return Ok(tipoAcesso);
    }

    [HttpPost("tipoAcesso")]
    public async Task<ActionResult> Post(TipoAcesso tipoAcesso)
    {
        await _repository.Insert(tipoAcesso);

        return Ok(tipoAcesso);
    }

    [HttpPut("tipoAcesso/{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] TipoAcesso tipoAcesso)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        await _repository.Update(objectId, tipoAcesso);

        return Ok(tipoAcesso);
    }

    [HttpDelete("tipoAcesso/{id}")]
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
