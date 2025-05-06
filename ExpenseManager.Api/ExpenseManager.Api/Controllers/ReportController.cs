using ExpenseManager.Api.Services.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("UserExpenses/{userId}")]
    public async Task<IActionResult> GetUserExpenses(int userId)
    {
        var result = await _reportService.GetUserExpenses(userId);
        return Ok(result);
    }

    [HttpGet("CompanyExpenseSummary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCompanyExpenseSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportService.GetCompanyExpenseSummaryByDateRange(startDate, endDate);
        return Ok(result);
    }
    
    [HttpGet("EmployeeExpenseSummary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEmployeeExpenseSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportService.GetEmployeeExpenseSummaryByDateRange(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("ApprovalRejectionSummary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetApprovalRejectionSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportService.GetApprovalRejectionSummaryByDateRange(startDate, endDate);
        return Ok(result);
    }
}