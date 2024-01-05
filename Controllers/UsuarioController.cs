using APICadastro.Models;
using Microsoft.AspNetCore.Mvc;
using APICadastro.Services;
using MongoDB.Bson;

namespace APICadastro.Controllers;

[ApiController]
public class UsuarioController : ControllerBase
{   
    private readonly UsuarioAppServices _usuarioAppServices;
    private readonly InativacaoAppServices _inativacaoAppServices;

    public UsuarioController(UsuarioAppServices usuarioAppServices, InativacaoAppServices inativacaoAppServices)
    {
        _usuarioAppServices = usuarioAppServices;
        _inativacaoAppServices = inativacaoAppServices;
    }

    [HttpGet("usuarios")]
    public async Task<ActionResult> Get()
    {
        var usuarios = await _usuarioAppServices.BuscaUsuarios();

        if (!usuarios.Any() || usuarios is null)
        {
            return NotFound();
        }

        return Ok(usuarios);
    }

    [HttpGet("inativacoes/usuario/{id}")]
    public async Task<ActionResult> GetInativacaoByUserId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var usuarios = await _inativacaoAppServices.BuscaInativacaoPorIdDeUsuario(objectId);

        if (!usuarios.Any() || usuarios is null)
        {
            return NotFound();
        }

        return Ok(usuarios);
    }

    [HttpGet("usuario/{id}")]
    public async Task<ActionResult> GetId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var usuario = await _usuarioAppServices.BuscaUsuarioId(objectId);

        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        return Ok(usuario);
    }

    [HttpPost("usuario")]
    public async Task<ActionResult> Post(Usuario usuario)
    {
        var errors = await _usuarioAppServices.CadastraUsuario(usuario);

        if(errors is null)
        {
            return Ok(usuario);
        }

        return BadRequest(errors);
    }

    [HttpPost("inativacao")]
    public ActionResult Post(Inativacao inativacao)
    {
        var error = _inativacaoAppServices.InativaConta(inativacao);

        if(error is null)
        {
            return Ok("Usuario inativado!!");
        }

        return BadRequest(error);
    }

    [HttpPut("inativacao/{id}")]
    public ActionResult Post(string id, [FromBody] DateTime dataFim)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var error = _inativacaoAppServices.AlteraInativacao(objectId, dataFim);

        if (error is null)
        {
            return Ok("Usuario reativado!");
        }

        return BadRequest(error);
    }

    /*[HttpPost("login")]
    public ActionResult<dynamic> Post(string email, string senha)
    {

        var login = _usuarioAppServices.LogaUsuario(email, senha);

        if (login is null)
        {
            return BadRequest("Não foi possivel concluir o login");
        }

        return Ok(login);
    }*/

    [HttpPost("validacao")]
    public ActionResult Post(Token token)
    {
        var dadosUsuario = TokenAppServices.ValidateToken(token.Key);

        if (dadosUsuario == null)
        {
            return Unauthorized();
        }

        return Ok(dadosUsuario);
    }

    [HttpPut("usuario/{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] Usuario usuario)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var errors = await _usuarioAppServices.AtualizaUsuario(objectId, usuario);

        if (errors is null)
        {
            usuario.Senha = "";
            return Ok(usuario);
        }

        return BadRequest(errors);
    }

    [HttpDelete("usuario/{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var error = await _usuarioAppServices.DeletaUsuario(objectId);

        if(error)
        {
            return NotFound("Usuario não encontrado...");
        }

        return Ok("Usuario deletado com sucesso!");
    }
}
