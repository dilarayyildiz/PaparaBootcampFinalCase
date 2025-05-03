using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Filter;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Services.Token;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IMediator mediator;
    public AuthorizationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("Token")]
    [TypeFilter(typeof(LogResourceFilter))]
    [TypeFilter(typeof(LogActionFilter))]
    [TypeFilter(typeof(LogAuthorizationFilter))]
    [TypeFilter(typeof(LogResultFilter))]
    [TypeFilter(typeof(LogExceptionFilter))]
    public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest request)
    {
        var operation = new CreateAuthorizationTokenCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

}