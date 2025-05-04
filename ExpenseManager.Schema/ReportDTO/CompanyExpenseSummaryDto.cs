namespace ExpenseManager.Schema.ReportDTO;

public class CompanyExpenseSummaryDto
{
    public DateTime Date { get; set; }
    public decimal ApprovedAmount { get; set; }
    public decimal RejectedAmount { get; set; }
}