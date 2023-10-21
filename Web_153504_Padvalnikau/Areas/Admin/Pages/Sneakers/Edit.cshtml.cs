using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Services.ProductService;

namespace Web_153504_Padvalnikau.Areas.Admin.Pages.Sneakers;

public class EditModel : PageModel
{
    private readonly IProductService _sneakerService;
    private readonly ICategoryService _categoryService;

    public EditModel(IProductService sneakerService, ICategoryService categoryService)
    {
        _sneakerService = sneakerService;
        _categoryService = categoryService;
    }

    [BindProperty]
    public Sneaker Sneaker { get; set; } = default!;

    [BindProperty]
    public IFormFile? Image { get; set; } = null;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sneaker = await _sneakerService.GetByIdAsync((int)id);
        ViewData["categories"] = (await _categoryService.GetCategoryListAsync()).Data;

        if (sneaker == null)
        {
            return NotFound();
        }
        Sneaker = sneaker.Data;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
            
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _sneakerService.UpdateAsync(Sneaker.Id, Sneaker, Image);

        return RedirectToPage("./Index");
    }
}