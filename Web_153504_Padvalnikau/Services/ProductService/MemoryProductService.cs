using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;
using Web_153504_Padvalnikau.Services.CategoryService;

namespace Web_153504_Padvalnikau.Services.ProductService;
public class MemoryProductService : IProductService
{
    private List<Sneaker> _sneakers;
    private List<Category> _categories;
    private readonly int _itemsPerPage;

    public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
    {
        _categories = categoryService.GetCategoryListAsync()
            .Result
            .Data;
        _itemsPerPage = (int)config.GetValue(typeof(int),"ItemsPerPage");
        SetupData();
    }

    /// <summary>
    /// Инициализация списков
    /// </summary>
    private void SetupData()
    {
        _sneakers = new List<Sneaker>
        {
            new Sneaker
            {
                Id = 1, Name = "NIKE Air Zoom ALPHAFLY Next",
                Description = "Cамая высокая система амортизации, комфортно сидят на ноге",
                Price = 435, Image = "Images/run2.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Id = 2, Name = "New Balance Fresh Foam 1080",
                Description = "Cцепление с отскоком,  ультрасовременный дизайн",
                Price = 195, Image = "Images/run3.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Id = 3, Name = "ClimaWarm Bounce",
                Description = "Лёгкая амортизация, водоотталкивающий верх",
                Price = 150,
                Image = "Images/run4.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Id = 4, Name = "Air Jordan 5 «Oregon» Ducks Duckman",
                Description = "Контрастные перфорированные панели по бокам",
                Price = 325, Image = "Images/basket2.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Id = 5, Name = "Reebok Shaqnosis x Minions «Shaq-Fu»",
                Description = "Воздушная подушка, капсулы с сжатым газом",
                Price = 410, Image = "Images/basket3.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Id = 6, Name = "adidas Pharrell Williams Crazy BYW LVL",
                Description = "Cмягчение удара при приземлении, оригинальность дизайна",
                Price = 399,
                Image = "Images/basket4.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Id = 7, Name = "Hoka One One",
                Description = "Для нейтральной пронации стопы, классика всех времен",
                Price = 125, Image = "Images/casual2.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Id = 8, Name = "New Balance 327",
                Description = "Современный силуэт, ретро-эстетика, недорогой комфорт",
                Price = 80, Image = "Images/casual3.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Id = 9, Name = "Alexander McQueen",
                Description = "Плоская подошва из полиуретановой пены",
                Price = 600,
                Image = "Images/casual4.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Id = 10, Name = "Wilson",
                Description = "Текстильная подкладка, формованная стелька",
                Price = 95, Image = "Images/tennis2.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Id = 11, Name = "Nike Air Tech Challenge 2",
                Description = "Эксклюзивный статус Tier Zero, лимитированная коллекция",
                Price = 135, Image = "Images/tennis3.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Id = 12, Name = "ASICS Gel-Resolution 8",
                Description = "Исключительной мягкостью и долговечность",
                Price = 98,
                Image = "Images/tennis4.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Id = 13, Name = "ASICS Fujitrabuco Sky",
                Description = "Полная водо- и ветронепроницаемость",
                Price = 142, Image = "Images/tour1.jpg",
                CategoryId = 5
            },
            new Sneaker
            {
                Id = 14, Name = "LaSportiva Bushido II",
                Description = "Быстро регулирующаяся шнуровка",
                Price = 236, Image = "Images/tour2.jpg",
                CategoryId = 5
            },
            new Sneaker
            {
                Id = 15, Name = "Merrell Hydrotrekker",
                Description = "Одно из лучших мембранных покрытий",
                Price = 112,
                Image = "Images/tour3.jpg",
                CategoryId = 5
            }
        };
    }

    private int ComputeTotalPages(int items)
    {
        return (items + _itemsPerPage - 1) / _itemsPerPage;
    }

    public Task<ResponseData<ListModel<Sneaker>>> GetSneakerListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var data = new ListModel<Sneaker>();
        Category? category = _categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName));
        var neededSneakers = _sneakers
            .Where(s => categoryNormalizedName == null || s.CategoryId == category?.Id)
            .ToList();
        data.Items = neededSneakers
            .Skip((pageNo - 1) * _itemsPerPage)
            .Take(_itemsPerPage)
            .ToList();
        data.TotalPages = ComputeTotalPages(neededSneakers.Count);
        data.CurrentPage = pageNo;

        var result = new ResponseData<ListModel<Sneaker>>{Data = data};

        return Task.FromResult(result);
        /*ListModel<Sneaker> listModel = new ListModel<Sneaker>(){Items = _sneakers};
        ResponseData<ListModel<Sneaker>> data = new ResponseData<ListModel<Sneaker>>(){Data = listModel}; 
        return Task.Run(() => data);*/

    }

    public Task<ResponseData<Sneaker>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(int id, Sneaker product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Sneaker>> CreateAsync(Sneaker product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
