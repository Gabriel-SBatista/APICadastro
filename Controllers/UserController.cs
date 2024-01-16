using APICadastro.Models;
using Microsoft.AspNetCore.Mvc;
using APICadastro.Services;
using MongoDB.Bson;

namespace APICadastro.Controllers;

[ApiController]
public class UserController : ControllerBase
{   
    private readonly UserServices _userAppServices;
    private readonly InactivationServices _inactivationAppServices;
    private readonly TokenServices _tokenAppServices;

    public UserController(UserServices userAppServices, InactivationServices inactivationAppServices, TokenServices tokenAppServices)
    {
        _userAppServices = userAppServices;
        _inactivationAppServices = inactivationAppServices;
        _tokenAppServices = tokenAppServices;
    }

    [HttpGet("usuarios")]
    public async Task<ActionResult> Get()
    {
        var users = await _userAppServices.SearchUsers();

        if (!users.Any() || users is null)
        {
            return NotFound();
        }

        return Ok(users);
    }

    [HttpGet("inativacoes/usuario/{id}")]
    public async Task<ActionResult> GetinactivationByUserId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var users = await _inactivationAppServices.SearchInactivationByUserId(id);

        if (!users.Any() || users is null)
        {
            return NotFound();
        }

        return Ok(users);
    }

    [HttpGet("usuario/{id}")]
    public async Task<ActionResult> GetId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var user = await _userAppServices.SearchUserById(objectId);

        if (user is null)
        {
            return NotFound("usuario não encontrado...");
        }

        return Ok(user);
    }

    [HttpPost("usuario")]
    public async Task<ActionResult> Post(User user)
    {
        var errors = await _userAppServices.RegisterUser(user);

        if(errors is null)
        {
            return Ok(user);
        }

        return BadRequest(errors);
    }

    [HttpPost("inativacao")]
    public async Task<ActionResult> Post(Inactivation inactivation)
    {
        var error = await _inactivationAppServices.InactiveAccount(inactivation);

        if(error is null)
        {
            return Ok("usuario inativado!!");
        }

        return BadRequest(error);
    }

    [HttpPut("inactivation/{id}")]
    public async Task<ActionResult> Post(string id, [FromBody] DateTime? endDate)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var error = await _inactivationAppServices.AlterInactivation(id, endDate);

        if (error is null)
        {
            return Ok("usuario reativado!");
        }

        return BadRequest(error);
    }

    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Post(string email, string password)
    {

        var login = await _userAppServices.LoginUser(email, password);

        if (login is null)
        {
            return BadRequest("Não foi possivel concluir o login");
        }

        return Ok(login);
    }

    [HttpPost("validacao")]
    public ActionResult Post(Token token)
    {
        var dataUser = _tokenAppServices.ValidateToken(token.Key);

        if (dataUser == null)
        {
            return Unauthorized();
        }

        return Ok(dataUser);
    }

    [HttpPut("usuario/{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] User user)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Formato de id invalido");
        }

        var errors = await _userAppServices.UpdateUser(objectId, user);

        if (errors is null)
        {
            user.Password = "";
            return Ok(user);
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

        var error = await _userAppServices.DeleteUser(objectId);

        if(error)
        {
            return NotFound("user não encontrado...");
        }

        return Ok("user deletado com sucesso!");
    }
}
