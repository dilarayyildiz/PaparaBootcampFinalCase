namespace ExpenseManager.Schema;

public class ExpenseCategoryRequest
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

public class ExpenseCategoryResponse
{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
}

    
