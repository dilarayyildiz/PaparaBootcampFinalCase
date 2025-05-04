using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountHistoryController : ControllerBase
{
    private readonly IMediator mediator;

    public AccountHistoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AccountHistoryResponse>>> GetAll()
    {
        var result = await mediator.Send(new GetAllAccountHistoriesQuery());
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<AccountHistoryResponse>> GetById(int id)
    {
        var result = await mediator.Send(new GetAccountHistoryByIdQuery(id));
        return result;
    }
}
//– Tüm hesap hareketlerini listeleme
// – Tek bir hareketi (ID ile) detaylı görme