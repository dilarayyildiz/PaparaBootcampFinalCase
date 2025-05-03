namespace ExpenseManager.Schema;

public class ExpenseRequest
{
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string ExpenseStatus { get; set; } // enum string
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string ReceiptUrl { get; set; } // fiş/fatura yükleme bağlantısı
}

//Status girilmez → backend başta Pending atar.
// RejectionReason girilmez → ancak reddedilirse backend doldurur.
public class ExpenseResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } // User.Name + " " + User.Surname
    public string CategoryName { get; set; } // Category.Name
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string ExpenseStatus { get; set; } // enum string
    public string? RejectionReason { get; set; }
    public string ReceiptUrl { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public DateTime InsertedDate { get; set; }
}