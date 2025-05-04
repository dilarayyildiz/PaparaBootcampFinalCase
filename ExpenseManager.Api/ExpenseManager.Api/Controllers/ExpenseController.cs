using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Request;
using ExpenseManager.Base.ApiResponse;
using ExpenseManager.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ExpenseController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IWebHostEnvironment _environment;

    public ExpenseController(IMediator mediator, IWebHostEnvironment environment)
    {
        this.mediator = mediator;
        _environment = environment;
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<ExpenseResponse>>> GetAllExpenses()
    {
        var query = new GetAllExpenseQuery();
        var result = await mediator.Send(query);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseResponse>> GetExpenseById(int id)
    {
        var query = new GetExpenseByIdQuery(id);
        var result = await mediator.Send(query);
        return result;
    }

    [HttpPost("CreateExpense")]
    public async Task<ApiResponse<ExpenseResponse>> CreateExpense([FromForm] CreateExpenseRequest request)
    {
        string receiptUrl = null;

        if (request.ReceiptFile != null && request.ReceiptFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "receipts");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(request.ReceiptFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ReceiptFile.CopyToAsync(stream);
            }

            //Uniqıelestirmek için user name ve date eklencek
            receiptUrl = $"{Request.Scheme}://{Request.Host}/receipts/{uniqueFileName}";
        }
        
        var command = new CreateExpenseCommand(receiptUrl, request);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpPut("UpdateExpense/{id}")]
    public async Task<ApiResponse> UpdateExpense(int id, [FromBody] ExpenseRequest request)
    {
        
        var command = new UpdateExpenseCommand(id, request);
        var result = await mediator.Send(command);
        return result;
    }
    
    [HttpPut("ApproveExpense/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> ApproveExpense(int id)
    {
        var command = new ApproveExpenseCommand(id);
        var result = await mediator.Send(command);
        return result;
    }

    [HttpPut("RejectExpense/{id}")] 
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> RejectExpense(int id, [FromBody] RejectExpenseRequest request)
    {
        var command = new RejectExpenseCommand(id, request.RejectionReason);
        var result = await mediator.Send(command);
        return result;
    }
    
    [HttpDelete("CancelExpense/{id}")]
    public async Task<ApiResponse> DeleteExpense(int id)
    {
        var command = new CancelExpenseCommand(id);
        var result = await mediator.Send(command);
        return result;
    }
}