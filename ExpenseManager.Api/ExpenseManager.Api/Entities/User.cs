using System.ComponentModel.DataAnnotations.Schema;
using ExpenseManager.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Api.Entities;
public enum UserRole { Admin = 1, Personnel = 2 }

[Table("User", Schema = "dbo")]
public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public string Secret { get; set; }
    public UserRole Role { get; set; }
    public string IBAN { get; set; } 

    public ICollection<Expense> Expenses { get; set; }
    
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);    
        //builder.Property(x => x.Id).ValueGeneratedOnAdd();
        //builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Secret).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.IBAN).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IsActive).IsRequired();
        
        /*builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);*/
      
    }
}