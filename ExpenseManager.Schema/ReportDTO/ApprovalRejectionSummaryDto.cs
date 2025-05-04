namespace ExpenseManager.Schema.ReportDTO;

public class ApprovalRejectionSummaryDto
{
    public DateTime Date { get; set; }
    public int ExpenseStatus { get; set; } // 2 = Approved, 3 = Rejected
    public decimal TotalAmount { get; set; }
}