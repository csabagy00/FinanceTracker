
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class CreateTransactionDTO
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; } = null!;
}
