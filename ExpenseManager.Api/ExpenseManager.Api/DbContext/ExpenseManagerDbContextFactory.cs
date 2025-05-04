
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ExpenseManager.Api;

public class ExpenseManagerDbContextFactory : IDesignTimeDbContextFactory<ExpenseManagerDbContext>
{
    public ExpenseManagerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ExpenseManagerDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ExpenseManagerDbConnection"));

        // IHttpContextAccessor yok → null geçiyoruz
        return new ExpenseManagerDbContext(optionsBuilder.Options, null);
    }
}
