USE ExpenseManager
go

CREATE PROCEDURE sp_ApprovalRejectionSummaryByDateRange
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        CAST(CreateDate AS DATE) AS Date,
        ExpenseStatus,
        SUM(Amount) AS TotalAmount
    FROM Expense
    WHERE IsActive = 1 AND CreateDate BETWEEN @StartDate AND @EndDate
    GROUP BY CAST(CreateDate AS DATE), ExpenseStatus
END