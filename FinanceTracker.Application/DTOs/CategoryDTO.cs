using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.DTOs;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
