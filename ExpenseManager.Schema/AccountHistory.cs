using System.Transactions;

namespace ExpenseManager.Schema;

// bu kayıtları kullanıcı göndermez → sistem oluşturur.
public class AccountHistoryResponse
{
    public int Id { get; set; }
    public decimal BalanceAfterTransaction { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string ToIBAN { get; set; }
    public Guid ReferenceNumber { get; set; }
}