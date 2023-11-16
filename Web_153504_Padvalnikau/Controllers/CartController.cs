using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Services.ProductService;

namespace Web_153504_Padvalnikau.Controllers;

public class CartController : Controller
{
    private readonly IProductService _sneakerService;
    private readonly Cart _cart;

    public CartController(IProductService sneakerService, Cart cart)
    {
        _sneakerService = sneakerService;
        _cart = cart;
    }

    public IActionResult Index()
    {
        return View(_cart);
    }

    [Route("[controller]/add/{id:int}")]
    public async Task<ActionResult> Add(int id, string returnUrl)
    {
        var data = await _sneakerService.GetByIdAsync(id);
        if (data.Success)
        {
            _cart.AddToCart(data.Data!);
        }
        return Redirect(returnUrl);
    }

    public IActionResult RemoveItem(int id, string redirectUrl)
    {
        _cart.RemoveItems(id);
        return Redirect(redirectUrl);
    }
}