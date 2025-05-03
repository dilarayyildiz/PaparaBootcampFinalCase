using ExpenseManager.Api.Impl.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // sadece admin erişebilir
public  class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(user);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(user);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _mediator.Send(new DeleteUserCommand(id));
        return Ok(user);
    }
   
    
}