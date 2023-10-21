using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Services.ProductService;

namespace Web_153504_Padvalnikau.Areas.Admin.Pages.Sneakers;

public class DetailsModel : PageModel
{
    private readonly IProductService _sneakerService;

    public DetailsModel(IProductService sneakerService)
    {
        _sneakerService = sneakerService;
    }

    public Sneaker Sneaker { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var response = await _sneakerService.GetByIdAsync((int)id);
        if (response.Success) 
        {
            Sneaker = response.Data;
        }
        else
        {
            return NotFound();
        }

        return Page();
    }
}