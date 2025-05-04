using ExpenseManager.Schema.ReportDTO;

namespace ExpenseManager.Api.Services.ReportService;

public interface IReportService
{
    Task<IEnumerable<UserExpenseReportDto>> GetUserExpenses(int userId);
    Task<IEnumerable<CompanyExpenseSummaryDto>> GetCompanyExpenseSummaryByDateRange(DateTime startDate, DateTime endDate);
    Task<IEnumerable<EmployeeExpenseSummaryDto>> GetEmployeeExpenseSummaryByDateRange(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ApprovalRejectionSummaryDto>> GetApprovalRejectionSummaryByDateRange(DateTime startDate, DateTime endDate);

}