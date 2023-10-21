using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;
using Web_153504_Padvalnikau.Services.ProductService;

namespace Web_153504_Padvalnikau.Areas.Admin.Pages.Sneakers;

public class IndexModel : PageModel
{
    private readonly IProductService _sneakerService;

    public IndexModel(IProductService sneakerService)
    {
        _sneakerService = sneakerService;
    }

    public ListModel<Sneaker> Sneaker { get; set; } = new ListModel<Sneaker>();

    public async Task OnGetAsync(int pageNo = 1)
    {
        var response = await _sneakerService.GetSneakerListAsync(null, pageNo);
        if (response.Success)
        {
            Sneaker = response.Data;
        }
    }
}