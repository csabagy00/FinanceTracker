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
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Categories")]
    public async Task<IActionResult> GetCategories()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var categories = await _context.Categories.Where(c => c.UserId == userId)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

        return Ok(categories);
    }

    [HttpGet("CategoryExists")]
    public async Task<IActionResult> DoesCategoryExists(string categoryName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var categories = await _context.Categories.Where(c => c.UserId == userId).ToListAsync();

        bool exists = categories.Any(c => c.Name == categoryName);

        return Ok(exists);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryNameDTO categoryDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var category = new Category
        {
            Id = new Guid(),
            Name = categoryDto.Name,
            UserId = userId!
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
            return NotFound("Category was not found");

        if (category.Transactions.Count > 0)
            return Conflict("Cannot delete a category that has transactions assigned to it");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok($"Category {category.Name} deleted successfully");
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateCategory(CategoryNameDTO categoryNameDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var category = _context.Categories.FirstOrDefault(c => c.Id == categoryNameDto.Id && c.UserId == userId);

        if (category == null)
            return NotFound("Category not found");

        if (category.Transactions.Count > 0)
        {
            var transactions = await _context.Transactions.Where(t => t.UserId == userId && t.Category == category).ToListAsync(); ;

            foreach(var trans in transactions)
                trans.Category.Name = categoryNameDto.Name;
        }

        _context.Entry(category).CurrentValues.SetValues(categoryNameDto);
        await _context.SaveChangesAsync();

        return Ok("Updated category");
    }
}
