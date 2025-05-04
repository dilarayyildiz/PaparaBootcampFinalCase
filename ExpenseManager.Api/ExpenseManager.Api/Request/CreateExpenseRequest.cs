using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Api.Request;

public class CreateExpenseRequest
{
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string ExpenseStatus { get; set; } // enum string
    public string PaymentMethod { get; set; }
    public string? RejectionReason { get; set; }
    public string PaymentLocation { get; set; }
    [Required(ErrorMessage = "ReceiptFile is required.")]
    public IFormFile ReceiptFile { get; set; }  // dosya alanı
    //public string ReceiptUrl { get; set; } // fiş/fatura yükleme bağlantısı
}
