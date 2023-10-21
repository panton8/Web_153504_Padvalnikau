using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Services.ProductService;

namespace Web_153504_Padvalnikau.Areas.Admin.Pages.Sneakers;

public class CreateModel : PageModel
{
    private readonly IProductService _sneakerService;
    private readonly ICategoryService _categoryService;

    public CreateModel(IProductService prodctService, ICategoryService categoryService)
    {
        _sneakerService = prodctService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var categories = await _categoryService.GetCategoryListAsync();
        ViewData["categories"] = categories.Data;
        return Page();
    }

    [BindProperty]
    public Sneaker Sneaker { get; set; } = default!;

    [BindProperty]
    public IFormFile? Image { get; set; }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid || Sneaker == null)
        {
            return Page();
        }

        await _sneakerService.CreateAsync(Sneaker, Image);

        return RedirectToPage("./Index");
    }
}