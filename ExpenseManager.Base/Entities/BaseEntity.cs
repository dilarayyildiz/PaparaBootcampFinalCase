namespace ExpenseManager.Base;

public class BaseEntity
{
    public int Id { get; set; }
    public string CreateUser { get; set; }   
    public DateTime CreateDate { get; set; }
    public string? ModifyUser { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsActive { get; set; }
    
}