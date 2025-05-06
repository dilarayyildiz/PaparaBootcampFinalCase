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
    private readonly IMediator _mediator;

    public AccountHistoryController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("MyAccountTransactions")]
    public async Task<ApiResponse<List<AccountHistoryResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAccountHistoriesQuery());
        return result;
    }

    [HttpGet("AccountTransactionDetail/{id}")]
    public async Task<ApiResponse<AccountHistoryResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetAccountHistoryByIdQuery(id));
        return result;
    }
}
//– Tüm hesap hareketlerini listeleme
// – Tek bir hareketi (ID ile) detaylı görme