namespace Web_153504_Padvalnikau.API.Services.CategoryService;
using Microsoft.EntityFrameworkCore;
using Web_153504_Padvalnikau.API.Data;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return new ResponseData<List<Category>>()
        {
            Data = categories
        };
    }
}