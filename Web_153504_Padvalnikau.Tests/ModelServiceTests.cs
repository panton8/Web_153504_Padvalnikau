using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Web_153504_Padvalnikau.API.Data;
using Web_153504_Padvalnikau.API.Services.ProductService;
using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.Tests_;

 public class ModelServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        #region CtorAndDispose
        public ModelServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_contextOptions);
            context.Database.Migrate();

            #region AddData

            var carBodies = new List<Category>
            {
                new Category() {Name="Беговые",
                    NormalizedName="running" },
                new Category() { Name="Баскетбольные",
                    NormalizedName="basketball" },
                new Category() { Name="Повседневные",
                    NormalizedName="casual"},
                new Category() { Name="Теннисные",
                    NormalizedName="tennis"},
                new Category() { Name="Туристические",
                    NormalizedName="tourist" }
            };

            context.AddRange(
                new Sneaker()
                {
                    Name = "NIKE Air Zoom ALPHAFLY Next",
                    Description = "Cамая высокая система амортизации, комфортно сидят на ноге",
                    Price = 435, Image = "Images/run2.jpg",
                    CategoryId = 1
                },
                new Sneaker()
                {
                    Id = 3, Name = "ClimaWarm Bounce",
                    Description = "Лёгкая амортизация, водоотталкивающий верх",
                    Price = 150,
                    Image = "Images/run4.jpg",
                    CategoryId = 1
                },
                new Sneaker()
                {
                    Id = 4, Name = "Air Jordan 5 «Oregon» Ducks Duckman",
                    Description = "Контрастные перфорированные панели по бокам",
                    Price = 325, Image = "Images/basket2.jpg",
                    CategoryId = 2
                },
                new Sneaker()
                {
                    Id = 5, Name = "Reebok Shaqnosis x Minions «Shaq-Fu»",
                    Description = "Воздушная подушка, капсулы с сжатым газом",
                    Price = 410, Image = "Images/basket3.jpg",
                    CategoryId = 2
                },
                new Sneaker()
                {
                    Id = 6, Name = "adidas Pharrell Williams Crazy BYW LVL",
                    Description = "Cмягчение удара при приземлении, оригинальность дизайна",
                    Price = 399,
                    Image = "Images/basket4.jpg",
                    CategoryId = 2
                },
                new Sneaker()
                {
                    Id = 7, Name = "Hoka One One",
                    Description = "Для нейтральной пронации стопы, классика всех времен",
                    Price = 125, Image = "Images/casual2.jpg",
                    CategoryId = 3
                },
                new Sneaker()
                {
                    Id = 8, Name = "New Balance 327",
                    Description = "Современный силуэт, ретро-эстетика, недорогой комфорт",
                    Price = 80, Image = "Images/casual3.jpg",
                    CategoryId = 3
                },
                new Sneaker()
                {
                    Id = 9, Name = "Alexander McQueen",
                    Description = "Плоская подошва из полиуретановой пены",
                    Price = 600,
                    Image = "Images/casual4.jpg",
                    CategoryId = 3
                },
                new Sneaker()
                {
                    Id = 10, Name = "Wilson",
                    Description = "Текстильная подкладка, формованная стелька",
                    Price = 95, Image = "Images/tennis2.jpg",
                    CategoryId = 4
                },
                new Sneaker()
                {
                    Id = 11, Name = "Nike Air Tech Challenge 2",
                    Description = "Эксклюзивный статус Tier Zero, лимитированная коллекция",
                    Price = 135, Image = "Images/tennis3.jpg",
                    CategoryId = 4
                },
                new Sneaker()
                {
                    Id = 12, Name = "ASICS Gel-Resolution 8",
                    Description = "Исключительной мягкостью и долговечность",
                    Price = 98,
                    Image = "Images/tennis4.jpg",
                    CategoryId = 4
                },
                new Sneaker()
                {
                    Id = 13, Name = "ASICS Fujitrabuco Sky",
                    Description = "Полная водо- и ветронепроницаемость",
                    Price = 142, Image = "Images/tour1.jpg",
                    CategoryId = 5
                },
                new Sneaker()
                {
                    Id = 14, Name = "LaSportiva Bushido II",
                    Description = "Быстро регулирующаяся шнуровка",
                    Price = 236, Image = "Images/tour2.jpg",
                    CategoryId = 5
                },
                new Sneaker()
                {
                    Id = 15,
                    Name = "Merrell Hydrotrekker",
                    Description = "Одно из лучших мембранных покрытий",
                    Price = 112,
                    Image = "Images/tour3.jpg",
                    CategoryId = 5
                });

            context.SaveChanges();
            context.AddRange(carBodies);
            context.SaveChanges();

            #endregion

        }
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        public void Dispose()
        {
            _connection.Dispose();
        }

        #endregion

        [Fact]
        public void GetCarModelListReturnPageNo1Size3()
        {
            // Arrange
            var context = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns("C:\\Test\\ContentRootPath");
            mockHostingEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("C:\\Test\\WebRootPath");


            var service = new ProductService(context, mockHttpContextAccessor.Object, mockHostingEnvironment.Object);

            // Act
            var result = service.GetSneakerListAsync(null).Result;

            // Assert
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(5, result.Data.TotalPages);
        }

        [Fact]
        public void GetCarModelListReturnCorrectPage()
        {
            // Arrange
            var context = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns("C:\\Test\\ContentRootPath");
            mockHostingEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("C:\\Test\\WebRootPath");


            var service = new ProductService(context, mockHttpContextAccessor.Object, mockHostingEnvironment.Object);


            // Act
            var result = service.GetSneakerListAsync(null, 2).Result;

            // Assert
            Assert.Equal(2, result.Data.CurrentPage);
        }

        [Fact]
        public void GetCarModelListReturnCorrectFilteredData()
        {
            // Arrange
            var context = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns("C:\\Test\\ContentRootPath");
            mockHostingEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("C:\\Test\\WebRootPath");


            var service = new ProductService(context, mockHttpContextAccessor.Object, mockHostingEnvironment.Object);


            // Act
            var result = service.GetSneakerListAsync("running").Result;

            // Assert
            Assert.True(result.Data.Items.All(item => item.CategoryId == 1));
        }

        [Fact]
        public void GetCarModelListMaxPageSize()
        {
            // Arrange
            var context = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns("C:\\Test\\ContentRootPath");
            mockHostingEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("C:\\Test\\WebRootPath");


            var service = new ProductService(context, mockHttpContextAccessor.Object, mockHostingEnvironment.Object);


            // Act
            var result = service.GetSneakerListAsync(null, pageSize: 15).Result;

            // Assert
            Assert.Equal(1, result.Data.TotalPages);
        }

        [Fact]
        public void GetCarModelListInvalidPageNoReturnSuccessFalse()
        {
            // Arrange
            var context = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns("C:\\Test\\ContentRootPath");
            mockHostingEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("C:\\Test\\WebRootPath");


            var service = new ProductService(context, mockHttpContextAccessor.Object, mockHostingEnvironment.Object);


            // Act
            var result = service.GetSneakerListAsync(null, 100).Result;

            // Assert
            Assert.False(result.Success);
        }
    }