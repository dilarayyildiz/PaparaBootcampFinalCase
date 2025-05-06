using System.Data;
using Dapper;
using System.Data.SqlClient;
using ExpenseManager.Schema.ReportDTO;

namespace ExpenseManager.Api.Services.ReportService;

public class ReportService : IReportService
{
    private readonly IConfiguration _configuration;

    public ReportService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<UserExpenseReportDto>> GetUserExpenses(int userId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("ExpenseManagerDbConnection"));
        var sql = "SELECT * FROM vw_UserExpenses WHERE UserId = @UserId";
        var result = await connection.QueryAsync<UserExpenseReportDto>(sql, new { UserId = userId });
        return result;
    }

    public async Task<IEnumerable<CompanyExpenseSummaryDto>> GetCompanyExpenseSummaryByDateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date.");

        using var connection = new SqlConnection(_configuration.GetConnectionString("ExpenseManagerDbConnection"));
        var result = await connection.QueryAsync<CompanyExpenseSummaryDto>(
            "sp_CompanyExpenseSummaryByDateRange",
            new { StartDate = startDate, EndDate = endDate }, 
            commandType: CommandType.StoredProcedure);
        return result;
    }
    
    public async Task<IEnumerable<EmployeeExpenseSummaryDto>> GetEmployeeExpenseSummaryByDateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date.");

        using var connection = new SqlConnection(_configuration.GetConnectionString("ExpenseManagerDbConnection"));
        var result = await connection.QueryAsync<EmployeeExpenseSummaryDto>(
            "sp_EmployeeExpenseSummaryByDateRange",
            new { StartDate = startDate, EndDate = endDate },
            commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<ApprovalRejectionSummaryDto>> GetApprovalRejectionSummaryByDateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date.");

        using var connection = new SqlConnection(_configuration.GetConnectionString("ExpenseManagerDbConnection"));
        var result = await connection.QueryAsync<ApprovalRejectionSummaryDto>(
            "sp_ApprovalRejectionSummaryByDateRange",
            new { StartDate = startDate, EndDate = endDate },
            commandType: CommandType.StoredProcedure);
        return result;
    }
}
