using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.Domain.Models;

public class CartItem
{
    public Sneaker Sneaker { get; set; }
    public int Quantity { get; set; }

    public CartItem(Sneaker sneaker, int quantity)
    {
        Sneaker = sneaker;
        Quantity = quantity;
    }
}