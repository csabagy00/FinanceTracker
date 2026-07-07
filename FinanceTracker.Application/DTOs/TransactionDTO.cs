using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class TransactionDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; } = null!;
}
