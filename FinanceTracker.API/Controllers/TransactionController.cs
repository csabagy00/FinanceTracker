using FinanceTracker.Application.DTOs;
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

        var transactions = await _context.Transactions.Where(t => t.UserId == userId)
            .Select(t => new TransactionDTO
            {
                Id = t.Id,
                Title = t.Title,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                Category = t.Category.Name,

            }).ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("TransactionById")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var transaction = await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);

        if (transaction == null)
            return NotFound("Transaction not found");

        var dto = new TransactionDTO
        {
            Id = transaction.Id,
            Title = transaction.Title,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type,
            Category = transaction.Category.Name
        };

        return Ok(dto);
    }

    [HttpPost("AddTransaction")]
    public async Task<IActionResult> CreateTransaction(CreateTransactionDTO transactionDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var category = _context.Categories.FirstOrDefault(c => c.Name == transactionDto.Category);

        // Feature to automatically create a category that does not exist || To create only if the user want to else abort the creation
        if (category == null)
            return BadRequest("Category does not exist");

        var transaction = new Transaction
        {
            Id = new Guid(),
            Title = transactionDto.Title,
            Amount = transactionDto.Amount,
            Date = transactionDto.Date,
            Type = transactionDto.Type,
            Category = category!,
            UserId = userId!
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return Ok(transactionDto);
    }

    [HttpDelete("DeleteTransaction")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);

        if (transaction == null)
            return NotFound("Transaction not found");

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return Ok($"Transaction {transaction.Title} deleted successfully");
    }

    [HttpPatch("UdpateTransaction")]
    public async Task<IActionResult> UpdateTransaction(TransactionDTO transactionDto)
    {
        var transaction = _context.Transactions.FirstOrDefault(t => t.Id == transactionDto.Id);

        if (transaction == null)
            return NotFound("Transaction not found");

        _context.Entry(transaction).CurrentValues.SetValues(transactionDto);

        await _context.SaveChangesAsync();

        return Ok("Updated transaction");
    }
}
