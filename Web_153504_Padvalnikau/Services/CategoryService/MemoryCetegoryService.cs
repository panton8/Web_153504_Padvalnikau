using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

namespace Web_153504_Padvalnikau.Services.CategoryService;


    public class MemoryCategoryService : ICategoryService
    { 
        public Task<ResponseData<List<Category>>> 
            GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Беговые",
                    NormalizedName="running"},
                new Category {Id=2, Name="Баскетбольные",
                    NormalizedName="basketball"},
                new Category {Id=3, Name="Повседневные",
                NormalizedName="casual"},
                new Category {Id=4, Name="Теннисные",
                NormalizedName="tennis"},
                new Category {Id=5, Name="Туристические",
                    NormalizedName="tourist"},
            };
            var result = new ResponseData<List<Category>>();
            result.Data=categories;
            return Task.FromResult(result);
        }
    }
