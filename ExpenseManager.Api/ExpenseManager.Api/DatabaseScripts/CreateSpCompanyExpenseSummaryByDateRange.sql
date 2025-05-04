USE ExpenseManager
go

CREATE PROCEDURE sp_CompanyExpenseSummaryByDateRange
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        CAST(CreateDate AS DATE) AS Date,
        SUM(CASE WHEN ExpenseStatus = 2 THEN Amount ELSE 0 END) AS ApprovedAmount,
        SUM(CASE WHEN ExpenseStatus = 3 THEN Amount ELSE 0 END) AS RejectedAmount
    FROM Expense
    WHERE IsActive = 1 AND CreateDate BETWEEN @StartDate AND @EndDate
    GROUP BY CAST(CreateDate AS DATE)
END