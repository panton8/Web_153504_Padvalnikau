namespace Web_153504_Padvalnikau.API.Services.CategoryService;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

public interface ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();   
}