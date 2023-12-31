﻿using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Services.ProductService;
using Web_153504_Padvalnikau.Extensions;

namespace Web_153504_Padvalnikau.Controllers;

[Route("catalog")]
public class ProductController : Controller
{
    private readonly IProductService _service;
    private readonly ICategoryService _category;
    
    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _service = productService;
        _category = categoryService;
    }

    [Route("Catalog/{category?}")]
    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {

        ViewBag.CurrentCategory = category;

        var sneakerResponse = await _service.GetSneakerListAsync(category, pageNo);

        if (!sneakerResponse.Success)
            return NotFound(sneakerResponse.ErrorMessage);

        var categoryResponse = await _category.GetCategoryListAsync();

        if (!categoryResponse.Success)
            return NotFound(categoryResponse.ErrorMessage);

        ViewBag.Categories = categoryResponse.Data;

        string s = Request.Headers["x-requested-with"].ToString();

        if (Request is not null && Request.IsAjaxRequest())
            return PartialView(sneakerResponse.Data);
        else
        {
            return View(sneakerResponse.Data);   
        }
    }
}