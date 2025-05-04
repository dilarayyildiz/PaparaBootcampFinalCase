namespace ExpenseManager.Schema.ReportDTO;

public class UserExpenseReportDto
{
    public int ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string ExpenseStatus { get; set; }
    public DateTime CreateDate { get; set; }
}