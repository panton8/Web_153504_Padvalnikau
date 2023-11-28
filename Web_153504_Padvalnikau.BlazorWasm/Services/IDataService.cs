using System;
using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.BlazorWasm.Services;

    public interface IDataService
    {
        event Action DataLoaded;

        List<Category> Categories { get; set; }

        List<Sneaker> ObjectsList { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }

        int TotalPages { get; set; }

        int CurrentPage { get; set; }

        public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        public Task<Sneaker> GetProductByIdAsync(int id);
        
        public Task GetCategoryListAsync();
    }
