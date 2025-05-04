using ExpenseManager.Api.Impl.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public  class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")] // sadece admin erişebilir
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")] // sadece admin erişebilir
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")] // sadece admin erişebilir
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(user);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")] // sadece admin erişebilir
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return Ok(user);
    }
    
    [HttpPut("ChangePassword")]
    [Authorize(Roles = "Employee")]  
    public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordCommand request)
    {
        var user = await _mediator.Send(request);
        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _mediator.Send(new DeleteUserCommand(id));
        return Ok(user);
    }
   
    
}