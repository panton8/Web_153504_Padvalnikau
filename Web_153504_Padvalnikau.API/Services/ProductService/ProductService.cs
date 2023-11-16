namespace Web_153504_Padvalnikau.API.Services.ProductService;
using Web_153504_Padvalnikau.API.Data;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;


public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _imagesPath;

    public ProductService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
    {
        _context = context;
        _imagesPath = Path.Combine(env.WebRootPath, "Images");
        _httpContextAccessor = httpContextAccessor;
        
    }

    public async Task DeleteAsync(int id)
    {
        Sneaker? sneaker = await _context.Sneakers.FindAsync(id);
        if (sneaker != null)
        {
            _context.Sneakers.Remove(sneaker);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ResponseData<Sneaker>> GetByIdAsync(int id)
    {
        Sneaker? sneaker = await _context.Sneakers.FindAsync(id);
        if (sneaker == null)
        {
            return new ResponseData<Sneaker>()
            {
                Success = false,
                ErrorMessage = $"Sneaker with id={id} not found"
            };
        }
        return new ResponseData<Sneaker>()
        {
            Data = sneaker
        };
    }

    public Task<ResponseData<ListModel<Sneaker>>> GetSneakerListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageNo < 1)
        {
            return Task.FromResult(
                new ResponseData<ListModel<Sneaker>>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                }
            );
        }

        var data = new ListModel<Sneaker>();
        Category? category = _context.Categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName));
    
        var neededSneakers = _context.Sneakers
            .Where(p => categoryNormalizedName == null || p.CategoryId == category.Id)
            .ToList();
        var totalPages = ComputeTotalPages(neededSneakers.Count, pageSize);

        if (pageNo > totalPages)
        {
            return Task.FromResult(
                new ResponseData<ListModel<Sneaker>>()
                {
                    Success = false,
                    ErrorMessage = "Page number must be less than total number of pages"
                }
            );
        }

        data.Items = neededSneakers
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        data.TotalPages = totalPages;
        data.CurrentPage = pageNo;

        return Task.FromResult(
            new ResponseData<ListModel<Sneaker>>()
            {
                Success = true,
                Data = data
            }
        );
    }

    private static int ComputeTotalPages(int all, int pageSize)
    {
        return (all + pageSize - 1) / pageSize;
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var product = await _context.Sneakers.FindAsync(id);
        if (product == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "Such product doesn't exist";
            return responseData;
        }
        var host = "http://" + _httpContextAccessor.HttpContext.Request.Host;
        if (formFile != null)
        {
            // Удалить предыдущее изображение
            if (!string.IsNullOrEmpty(product.Image))
            {
                var prevImage = Path.GetFileName(product.Image);
                var prevImagePath = Path.Combine(_imagesPath, prevImage);

                if (File.Exists(prevImagePath))
                {
                    File.Delete(prevImagePath);
                }
            }
            // Создать имя файла
            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            // Сохранить файл
            using (var fileStream = new FileStream($"{_imagesPath}/{fName}", FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            // Указать имя файла в объекте
            product.Image = $"{host}/Images/{fName}";
            await _context.SaveChangesAsync();
        }
        responseData.Data = product.Image;
        return responseData;
    }

    public async Task UpdateAsync(int id, Sneaker sneaker)
    {
        Sneaker? productToUpdate = await _context.Sneakers.FindAsync(id);
        if (productToUpdate != null)
        {
            productToUpdate.Description = sneaker.Description;
            productToUpdate.CategoryId = sneaker.CategoryId;
            productToUpdate.Price = sneaker.Price;
            if (sneaker.Image != null)
                productToUpdate.Image = sneaker.Image;
            _context.Update(productToUpdate);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ResponseData<Sneaker>> CreateAsync(Sneaker sneaker)
    {
        _context.Sneakers.Add(sneaker);
        await _context.SaveChangesAsync();
        return new ResponseData<Sneaker>
        {
            Data = sneaker,
            Success = true
        };
    }
}