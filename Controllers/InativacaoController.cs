using APICadastro.Context;
using APICadastro.Models;
using APICadastro.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace APICadastro.Controllers;

[ApiController]
[Route("[controller]")]
public class InativacaoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<Inativacao> _validator;

    public InativacaoController(AppDbContext context, IValidator<Inativacao> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpPost]
    public ActionResult Post(Inativacao inativacao)
    {
        InativacaoAppServices appServices = new InativacaoAppServices(_context, _validator);
        var error = appServices.InativaConta(inativacao);

        if (error is null)
        {
            return Ok("Usuario inativado!");
        }

        return BadRequest(error);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id,  Inativacao inativacao)
    {
        InativacaoAppServices appServices = new InativacaoAppServices(_context, _validator);
        var error = appServices.AlteraInativacao(id, inativacao);

        if (error is null)
        {
            return Ok("Inativacao alterada com sucesso!");
        }

        return BadRequest(error);
    }
}
