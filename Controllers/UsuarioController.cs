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
    private readonly IValidator<Inativacao> _validatorI;

    public UsuarioController(AppDbContext context, IValidator<Usuario> validator, IValidator<Inativacao> validatorI)
    {
        _context = context;
        _validator = validator;
        _validatorI = validatorI;
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

    [HttpGet("nome/{nome}")]
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

    [HttpGet("email/{email}")]
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

    [HttpPost("cadastro")]
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

    [HttpPost("inativacao")]
    public ActionResult Post(Inativacao inativacao)
    {
        InativacaoAppServices appServices = new InativacaoAppServices(_context, _validatorI);
        var error = appServices.InativaConta(inativacao);

        if(error is null)
        {
            return Ok("Usuario inativado!!");
        }

        return BadRequest(error);
    }

    [HttpPost("reativar/{id}:int")]
    public ActionResult Post(int id, DateTime dataFim)
    {
        InativacaoAppServices appServices = new InativacaoAppServices(_context, _validatorI);
        var error = appServices.AlteraInativacao(id, dataFim);

        if (error is null)
        {
            return Ok("Usuario reativado!");
        }

        return BadRequest(error);
    }

    [HttpPost("login/{email}")]
    public ActionResult<dynamic> Post(string email, string senha)
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
            usuario.Senha = "";
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
