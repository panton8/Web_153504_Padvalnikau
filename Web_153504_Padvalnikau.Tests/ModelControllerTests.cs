using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Controllers;
using Web_153504_Padvalnikau.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.Domain.Models;
using Web_153504_Padvalnikau.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit.Sdk;

namespace Web_153504_Padvalnikau.Tests_;

public class ModelControllerTests
{
    [Fact]
    public async void InvalidCarModelListReturn404()
    {
        // Arange
        var carBodyTypeService = new Mock<ICategoryService>();
        var carModelService = new Mock<IProductService>();

        string errorMessage = "Error Message";

        var carModels = Task.FromResult(new ResponseData<ListModel<Sneaker>>() { Data = new(), Success = false, ErrorMessage = errorMessage });
        carModelService.Setup(m => 
            m.GetSneakerListAsync(It.IsAny<string?>(), It.IsAny<int>())).Returns(carModels);

        var controller = new ProductController(carModelService.Object, carBodyTypeService.Object);

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(errorMessage, (result as NotFoundObjectResult).Value);

    }

    [Fact]
    public void InvalidCarBodyTypeReturn404()
    {
        // Arange
        var carBodyTypeService = new Mock<ICategoryService>();
        var carModelService = new Mock<IProductService>();

        string errorMessage = "Error Message";

        var carModels = Task.FromResult(new ResponseData<ListModel<Sneaker>>() { Data = new(), Success = true, ErrorMessage = "" });
        carModelService.Setup(m =>
            m.GetSneakerListAsync(It.IsAny<string?>(), It.IsAny<int>())).Returns(carModels);

        var carBodyTypes = Task.FromResult(new ResponseData<List<Category>>() { Data = new(), Success = false, ErrorMessage = errorMessage });
        carBodyTypeService.Setup(m =>
            m.GetCategoryListAsync()).Returns(carBodyTypes);

        var controller = new ProductController(carModelService.Object, carBodyTypeService.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };


        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(errorMessage, (result as NotFoundObjectResult).Value);

    }

    [Fact]
    public void SuccessfullAcceptedDataReturnView()
    {
        // Arange
        var carBodyTypeService = new Mock<ICategoryService>();
        var carModelService = new Mock<IProductService>();

        var carModels = Task.FromResult(new ResponseData<ListModel<Sneaker>>() { Data = new(), Success = true, ErrorMessage = "" });
        carModelService.Setup(m =>
            m.GetSneakerListAsync(It.IsAny<string?>(), It.IsAny<int>())).Returns(carModels);

        var carBodyTypes = Task.FromResult(new ResponseData<List<Category>>() { Data = new(), Success = true, ErrorMessage = "" });
        carBodyTypeService.Setup(m =>
            m.GetCategoryListAsync()).Returns(carBodyTypes);

        var controller = new ProductController(carModelService.Object, carBodyTypeService.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-requested-with"] = "XMLHttpRequest";
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.NotNull(result);
        var viewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal(carBodyTypes.Result.Data, controller.ViewBag.Categories);
        Assert.Null(controller.ViewBag.CurrentCategory);
        Assert.Equal(carModels.Result.Data, viewResult.Model);

    }

}