namespace ExpenseManager.Api.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; }
    public string EntityId { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserName { get; set; }
    public string ChangedValues { get; set; }
    public string OriginalValues { get; set; }
    
}