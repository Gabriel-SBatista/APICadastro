using APICadastro.Context;
using APICadastro.Models;
using Microsoft.AspNetCore.Mvc;
using APICadastro.Services;
using FluentValidation;

namespace APICadastro.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{   
    private readonly UsuarioAppServices _usuarioAppServices;
    private readonly InativacaoAppServices _inativacaoAppServices;

    public UsuarioController(UsuarioAppServices usuarioAppServices, InativacaoAppServices inativacaoAppServices)
    {
        _usuarioAppServices = usuarioAppServices;
        _inativacaoAppServices = inativacaoAppServices;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Usuario>> Get()
    {
        var usuarios = _usuarioAppServices.BuscaUsuarios();

        if (usuarios.Count == 0)
        {
            return NotFound();
        }

        return usuarios;
    }

    [HttpGet("inativacao")]
    public ActionResult<IEnumerable<Usuario>> GetInativo()
    {
        var usuarios = _usuarioAppServices.BuscaInativacoes();

        if (usuarios.Count == 0)
        {
            return NotFound();
        }

        return usuarios;
    }

    [HttpGet("{nome}")]
    public ActionResult<IEnumerable<Usuario>> GetNome(string nome)
    {
        var usuarios = _usuarioAppServices.BuscaUsuariosNome(nome);

        if (usuarios.Count == 0)
        {
            return NotFound("Usuario não encontrado...");
        }

        return usuarios;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Usuario> GetId(int id)
    {
        var usuario = _usuarioAppServices.BuscaUsuarioId(id);

        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        return usuario;
    }

    [HttpPost]
    public ActionResult Post(Usuario usuario)
    {
        var error = _usuarioAppServices.CadastraUsuario(usuario);

        if(error is null)
        {
            return Ok(usuario);
        }

        return BadRequest(error);
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

    [HttpPut("inativacao/{id:int}")]
    public ActionResult Post(int id, DateTime dataFim)
    {
        var error = _inativacaoAppServices.AlteraInativacao(id, dataFim);

        if (error is null)
        {
            return Ok("Usuario reativado!");
        }

        return BadRequest(error);
    }

    [HttpPost("login")]
    public ActionResult<dynamic> Post(string email, string senha)
    {

        var login = _usuarioAppServices.LogaUsuario(email, senha);

        if (login is null)
        {
            return BadRequest("Não foi possivel concluir o login");
        }

        return Ok(login);
    }

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

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Usuario usuario)
    {
        var error = _usuarioAppServices.AtualizaUsuario(id, usuario);

        if (error is null)
        {
            usuario.Senha = "";
            return Ok(usuario);
        }

        return BadRequest(error);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var error = _usuarioAppServices.DeletaUsuario(id);

        if(error)
        {
            return NotFound("Usuario não encontrado...");
        }

        return Ok("Usuario deletado com sucesso!");
    }
}
