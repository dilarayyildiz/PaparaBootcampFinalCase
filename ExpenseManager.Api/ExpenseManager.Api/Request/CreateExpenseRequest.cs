using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Api.Request;

public class CreateExpenseRequest
{ 
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } 
    public string PaymentMethod { get; set; } 
    public string PaymentLocation { get; set; }
    [Required(ErrorMessage = "ReceiptFile is required.")]
    public IFormFile ReceiptFile { get; set; }  // dosya alanı 
}

public class CreateExpenseResponse
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
