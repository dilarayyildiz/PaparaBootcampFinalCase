using System.ComponentModel.DataAnnotations.Schema;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;
public enum ExpenseStatus { Pending = 1, Approved = 2, Rejected = 3 }

[Table("Expense", Schema = "dbo")]
public class Expense : BaseEntity
{ 
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int CategoryId { get; set; }
    public ExpenseCategory Category { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public ExpenseStatus ExpenseStatus { get; set; }
    public string? RejectionReason { get; set; }
    public string ReceiptUrl { get; set; }
     
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    
}
/*public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}*/

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CategoryId).IsRequired();
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ExpenseStatus).IsRequired();
        builder.Property(x => x.RejectionReason).HasMaxLength(50);
        builder.Property(x => x.ReceiptUrl).HasMaxLength(500);
        builder.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PaymentLocation).IsRequired().HasMaxLength(50); 
    }
}