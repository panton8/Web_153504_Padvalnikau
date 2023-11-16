using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.Components;

public class CartViewComponent : ViewComponent
{
    public Cart Cart { get; set; }

    public CartViewComponent(Cart cart)
    {
        Cart = cart;
    }

    public IViewComponentResult Invoke()
    {
        return View(Cart);
    }
}