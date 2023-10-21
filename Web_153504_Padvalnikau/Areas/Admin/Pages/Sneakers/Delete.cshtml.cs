using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Services.ProductService;


namespace Web_153504_Padvalnikau.Areas.Admin.Pages.Sneakers;

public class DeleteModel : PageModel
{
    private readonly IProductService _sneakerService;

    public DeleteModel(IProductService sneakerService)
    {
        _sneakerService = sneakerService;
    }

    [BindProperty]
    public Sneaker Sneaker { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var response = await _sneakerService.GetByIdAsync((int)id);

        if (!response.Success)
        {
            return NotFound();
        }

        Sneaker = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await _sneakerService.DeleteAsync((int)id);

        return RedirectToPage("./Index");
    }
}