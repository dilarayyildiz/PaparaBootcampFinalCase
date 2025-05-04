using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;

public class Account : BaseEntity
{
    public long CustomerId { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    //public ICollection<AccountHistory> Expenses { get; set; }
    public virtual List<AccountHistory> AccountHistories { get; set; }
}

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    { 
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50); 
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Balance).IsRequired();
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);
        builder.Property(x => x.OpenDate).IsRequired();
        builder.Property(x => x.CloseDate).IsRequired(false);
    }
}   