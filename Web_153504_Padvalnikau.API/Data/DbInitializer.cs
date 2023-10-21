using Microsoft.EntityFrameworkCore;
using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.API.Data;

public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //context.Database.EnsureDeleted();
        var imgUrl = app.Configuration.GetValue<string>("ImageUrl");
        await context.Database.MigrateAsync();
        
        
        if (!context.Categories.Any())
        {
            var categories = new Category[]
            {
                new Category 
                {
                    Name="Беговые",
                    NormalizedName="running"
                },
                new Category 
                {
                    Name="Баскетбольные",
                    NormalizedName="basketball"
                    
                },
                new Category 
                {
                    Name="Повседневные",
                    NormalizedName="casual"
                    
                },
                new Category 
                {
                    Name="Теннисные",
                    NormalizedName="tennis"
                    
                },
                new Category 
                {
                    Name="Туристические",
                    NormalizedName="tourist"
                    
                }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        if (!context.Sneakers.Any())
        {
            var sneakers = new List<Sneaker>()
            { 
                new Sneaker
            {
                Name = "NIKE Air Zoom ALPHAFLY Next",
                Description = "Cамая высокая система амортизации, комфортно сидят на ноге",
                Price = 435, 
                Image = $"{imgUrl}run2.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Name = "New Balance Fresh Foam 1080",
                Description = "Cцепление с отскоком,  ультрасовременный дизайн",
                Price = 195, 
                Image = $"{imgUrl}run3.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Name = "ClimaWarm Bounce",
                Description = "Лёгкая амортизация, водоотталкивающий верх",
                Price = 150,
                Image = $"{imgUrl}run4.jpg",
                CategoryId = 1
            },
            new Sneaker
            {
                Name = "Air Jordan 5 «Oregon» Ducks Duckman",
                Description = "Контрастные перфорированные панели по бокам",
                Price = 325,
                Image = $"{imgUrl}basket2.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Name = "Reebok Shaqnosis x Minions «Shaq-Fu»",
                Description = "Воздушная подушка, капсулы с сжатым газом",
                Price = 410, 
                Image = $"{imgUrl}basket3.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Name = "adidas Pharrell Williams Crazy BYW LVL",
                Description = "Cмягчение удара при приземлении, оригинальность дизайна",
                Price = 399,
                Image = $"{imgUrl}basket4.jpg",
                CategoryId = 2
            },
            new Sneaker
            {
                Name = "Hoka One One",
                Description = "Для нейтральной пронации стопы, классика всех времен",
                Price = 125, 
                Image = $"{imgUrl}casual2.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Name = "New Balance 327",
                Description = "Современный силуэт, ретро-эстетика, недорогой комфорт",
                Price = 80, 
                Image = $"{imgUrl}casual3.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Name = "Alexander McQueen",
                Description = "Плоская подошва из полиуретановой пены",
                Price = 600,
                Image = $"{imgUrl}casual4.jpg",
                CategoryId = 3
            },
            new Sneaker
            {
                Name = "Wilson",
                Description = "Текстильная подкладка, формованная стелька",
                Price = 95, 
                Image = $"{imgUrl}tennis2.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Name = "Nike Air Tech Challenge 2",
                Description = "Эксклюзивный статус Tier Zero, лимитированная коллекция",
                Price = 135,
                Image = $"{imgUrl}tennis3.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Name = "ASICS Gel-Resolution 8",
                Description = "Исключительной мягкостью и долговечность",
                Price = 98,
                Image = $"{imgUrl}tennis4.jpg",
                CategoryId = 4
            },
            new Sneaker
            {
                Name = "ASICS Fujitrabuco Sky",
                Description = "Полная водо- и ветронепроницаемость",
                Price = 142,
                Image = $"{imgUrl}tour1.jpg",
                CategoryId = 5
            },
            new Sneaker
            {
                Name = "LaSportiva Bushido II",
                Description = "Быстро регулирующаяся шнуровка",
                Price = 236,
                Image = $"{imgUrl}tour2.jpg",
                CategoryId = 5
            },
            new Sneaker
            {
                Name = "Merrell Hydrotrekker",
                Description = "Одно из лучших мембранных покрытий",
                Price = 112,
                Image = $"{imgUrl}tour3.jpg",
                CategoryId = 5
            }
            };
            
            context.Sneakers.AddRange(sneakers);
            await context.SaveChangesAsync();
        }
        
    }
}