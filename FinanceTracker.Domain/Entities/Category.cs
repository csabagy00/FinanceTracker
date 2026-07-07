
namespace FinanceTracker.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
