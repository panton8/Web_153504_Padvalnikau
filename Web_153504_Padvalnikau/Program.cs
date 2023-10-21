using Web_153504_Padvalnikau.Models;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
UriData? uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IProductService, ApiProductService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();

builder.Services.AddRazorPages();


var app = builder.Build();

    
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();