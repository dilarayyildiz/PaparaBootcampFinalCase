USE ExpenseManager
go

CREATE PROCEDURE sp_EmployeeExpenseSummaryByDateRange
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        u.Id AS UserId,
        u.FirstName,
        u.LastName,
        CAST(e.CreateDate AS DATE) AS Date,
        SUM(e.Amount) AS TotalAmount
    FROM Expense e
    JOIN [User] u ON e.UserId = u.Id
    WHERE e.IsActive = 1 AND e.CreateDate BETWEEN @StartDate AND @EndDate
    GROUP BY u.Id, u.FirstName, u.LastName, CAST(e.CreateDate AS DATE)
END
