using APICadastro.Context;
using APICadastro.Models;
using Microsoft.AspNetCore.Mvc;
using APICadastro.Services;
using FluentValidation;

namespace APICadastro.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<Usuario> _validator;

    public UsuarioController(AppDbContext context, IValidator<Usuario> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Usuario>> Get()
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var usuarios = appServices.BuscaUsuarios();

        if (usuarios is null)
        {
            return NotFound();
        }

        return usuarios;
    }

    [HttpGet("nome/{nome:string}")]
    public ActionResult<IEnumerable<Usuario>> GetNome(string nome)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var usuarios = appServices.BuscaUsuariosNome(nome);

        if (usuarios is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        return usuarios;
    }

    [HttpGet("email/{email:string}")]
    public ActionResult<Usuario> GetEmail(string email)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var usuario = appServices.BuscaUsuarioEmail(email);

        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        return usuario;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Usuario> GetId(int id)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var usuario = appServices.BuscaUsuarioId(id);

        if (usuario is null)
        {
            return NotFound("Usuario não encontrado...");
        }

        return usuario;
    }

    [HttpPost]
    public ActionResult Post(Usuario usuario)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var error = appServices.CadastraUsuario(usuario);

        if(error is null)
        {
            return Ok(usuario);
        }

        return BadRequest(error);
    }

    [HttpPost("login")]
    public ActionResult<Usuario> Post([FromBody] string email, string senha)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var usuario = appServices.LogaUsuario(email, senha);

        if (usuario is null)
        {
            return BadRequest("Email ou senha incorreto");
        }

        return Ok(usuario);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Usuario usuario)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var error = appServices.AtualizaUsuario(id, usuario);

        if (error is null)
        {
            return Ok(usuario);
        }

        return BadRequest(error);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        UsuarioAppServices appServices = new UsuarioAppServices(_context, _validator);
        var error = appServices.DeletaUsuario(id);

        if(error)
        {
            return NotFound("Usuario não encontrado...");
        }

        return Ok("Usuario deletado com sucesso!");
    }
}
