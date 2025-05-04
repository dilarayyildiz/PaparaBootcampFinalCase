USE ExpenseManager
GO


CREATE VIEW vw_UserExpenses AS
SELECT 
    u.Id AS UserId,
    u.FirstName,
    u.LastName,
    e.Id AS ExpenseId,
    e.Amount,
    e.Description,
    e.ExpenseStatus,
    e.CreateDate
FROM Expense e
JOIN [User] u ON e.UserId = u.Id
WHERE e.IsActive = 1
