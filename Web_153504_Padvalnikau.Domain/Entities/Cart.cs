using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_153504_Padvalnikau.Domain.Models;

namespace Web_153504_Padvalnikau.Domain.Entities;

public class Cart
{
    /// <summary>
    /// Список объектов в корзине
    /// key - идентификатор объекта
    /// </summary>
    public Dictionary<int, CartItem> CartItems { get; set; } = new();
    /// <summary>
    /// Добавить объект в корзину
    /// </summary>
    /// <param name="sneaker">Добавляемый объект</param>
    public virtual void AddToCart(Sneaker sneaker)
    {
        var cartItem = CartItems.GetValueOrDefault(sneaker.Id, new CartItem(sneaker, 0));
        cartItem.Quantity++;
        CartItems[sneaker.Id] = cartItem;
    }
    /// <summary>
    /// Удалить объект из корзины
    /// </summary>
    /// <param name="id"> id удаляемого объекта</param>
    public virtual void RemoveItems(int id)
    {
        CartItems.Remove(id);
    }
    /// <summary>
    /// Очистить корзину
    /// </summary>
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }
    /// <summary>
    /// Количество объектов в корзине
    /// </summary>
    public int Count { get => CartItems.Sum(item => item.Value.Quantity); }
    /// <summary>
    /// Общая стоимость
    /// </summary>
    public double TotalPrice { get => CartItems.Sum(item => item.Value.Sneaker.Price * item.Value.Quantity); }
}