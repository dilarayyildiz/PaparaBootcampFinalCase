using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ExpenseManager.Api.Entities;
using ExpenseManager.Base; 

namespace ExpenseManager.Api;

public class ExpenseManagerDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ApplicationUser _applicationUser;

    public ExpenseManagerDbContext(DbContextOptions<ExpenseManagerDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        this._applicationUser = serviceProvider.GetService<ApplicationUser>();
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entyList = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity
         && (e.State == EntityState.Deleted || e.State == EntityState.Added || e.State == EntityState.Modified));

        var auditLogs = new List<AuditLog>();

        foreach (var entry in entyList)
        {
            var baseEntity = (BaseEntity)entry.Entity;
            var properties = entry.Properties.ToList();
            var changedProperties = properties.Where(p => p.IsModified).ToList();
            var changedValues = changedProperties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
            var originalValues = properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
            var changedValuesString = JsonConvert.SerializeObject(changedValues.Select(kvp => new { Key = kvp.Key, Value = kvp.Value }));
            var originalValuesString = JsonConvert.SerializeObject(originalValues.Select(kvp => new { Key = kvp.Key, Value = kvp.Value }));


            var auditLog = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = baseEntity.Id.ToString(),
                Action = entry.State.ToString(),
                Timestamp = DateTime.Now,
                UserName = _applicationUser?.UserName ?? "anonymous",
                ChangedValues = changedValuesString,
                OriginalValues = originalValuesString,
            };

            if (entry.State == EntityState.Added)
            {
                baseEntity.CreateDate = DateTime.Now;
                baseEntity.CreateUser = _applicationUser?.UserName ?? "anonymous";
                baseEntity.IsActive = true;
            }
            else if (entry.State == EntityState.Modified)
            {
                baseEntity.ModifyDate = DateTime.Now;
                baseEntity.ModifyUser = _applicationUser?.UserName ?? "anonymous";
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                baseEntity.IsActive = false;
                baseEntity.ModifyDate = DateTime.Now;
                baseEntity.ModifyUser = _applicationUser?.UserName ?? "anonymous";
            }

            auditLogs.Add(auditLog);
        }

        if (auditLogs.Any())
        {
            Set<AuditLog>().AddRange(auditLogs);
        }

        return base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseManagerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
