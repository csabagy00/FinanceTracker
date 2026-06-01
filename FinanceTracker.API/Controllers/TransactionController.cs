using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var transactions = await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();

        return Ok(transactions);
    }

    [HttpPost("AddTransaction")]
    public async Task<IActionResult> CreateTransaction(Transaction transaction)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        transaction.UserId = userId!;

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return Ok(transaction);
    }
}
