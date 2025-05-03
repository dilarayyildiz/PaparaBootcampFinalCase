using System.ComponentModel.DataAnnotations.Schema;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;
[Table("AccountHistory", Schema = "bank")]
public class AccountHistory : BaseEntity
{ 
    public decimal  Balance { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string IBAN { get; set; }
    public Guid ReferenceNumber { get; set; }
 
}

public class AccountHistoryConfiguration : IEntityTypeConfiguration<AccountHistory>
{
    public void Configure(EntityTypeBuilder<AccountHistory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Balance).IsRequired();
        builder.Property(x => x.TransactionAmount).IsRequired();
        builder.Property(x => x.TransactionDate).IsRequired();
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ReferenceNumber).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}