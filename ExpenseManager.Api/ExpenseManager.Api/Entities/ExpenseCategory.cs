using System.ComponentModel.DataAnnotations.Schema;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;
[Table("ExpenseCategory", Schema = "dbo")]
public class ExpenseCategory : BaseEntity
{ 
    public string Name { get; set; } = null!; 
    public ICollection<Expense> Expenses { get; set; } 
    
}
public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IsActive).IsRequired();
        
        builder.HasMany(x => x.Expenses)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}