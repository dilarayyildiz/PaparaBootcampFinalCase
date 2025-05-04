using System.ComponentModel.DataAnnotations.Schema;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;

[Table("AccountHistory", Schema = "dbo")]
public class AccountHistory : BaseEntity
{ 
    public decimal  Balance { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    //personel IBAN bilgisi (ödeme alan iban)
    public string ToIBAN { get; set; }
    //şirket IBAN bilgisi (ödeme yappan iban)
    public string FromIBAN { get; set; }
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
        builder.Property(x => x.ToIBAN).IsRequired().HasMaxLength(50);
        builder.Property(x => x.FromIBAN).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ReferenceNumber).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}