using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Web_153504_Padvalnikau.Components;

[ViewComponent]
public class Cart : ViewComponent
{

    public IViewComponentResult Invoke()
    {
        return View();
    }
}