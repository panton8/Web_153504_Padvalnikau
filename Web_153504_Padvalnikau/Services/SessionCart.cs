using System.Text.Json.Serialization;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Extensions;

namespace Web_153504_Padvalnikau.Services;

public class SessionCart : Cart
{
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
        SessionCart cart = session?.Get<SessionCart>("CartViewComponent") ?? new SessionCart();
        cart.Session = session;
        return cart;
    }

    [JsonIgnore] public ISession? Session { get; set; }

    public override void AddToCart(Sneaker sneaker)
    {
        base.AddToCart(sneaker);
        Session?.Set("CartViewComponent", this);
    }

    public override void RemoveItems(int id)
    {
        base.RemoveItems(id);
        Session?.Set("CartViewComponent", this);
    }

    public override void ClearAll()
    {
        base.ClearAll();
        Session?.Remove("CartViewComponent");
    }
}