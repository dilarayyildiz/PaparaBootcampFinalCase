namespace ExpenseManager.Schema.ReportDTO;

public class EmployeeExpenseSummaryDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
}