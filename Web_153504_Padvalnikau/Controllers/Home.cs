using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_153504_Padvalnikau.Controllers;

public class ListDemo
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class Home : Controller
{
    public IActionResult Index()
    {
        ViewBag.List = new List<string>() { "Элемент 0 списка", "Элемент 1 списка","Элемент 2 списка", "Элемент 3 списка","Элемент 4 списка" };

        ViewData["LabNum"] = "Лабораторная работа 2";

        var elements = new List<ListDemo>
        {
            new ListDemo { Id = 0, Name = "Item1" },
            new ListDemo { Id = 1, Name = "Item2" },
            new ListDemo { Id = 2, Name = "Item3" }
        };
        var selectList = new SelectList(elements, "Id", "Name");
            
        return View(selectList);
    }
}