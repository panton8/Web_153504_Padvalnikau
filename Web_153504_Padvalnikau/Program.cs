using Serilog;
using Web_153504_Padvalnikau.Models;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Extensions;
using Web_153504_Padvalnikau.Services;
using Web_153504_Padvalnikau.Services.CategoryService;
using Web_153504_Padvalnikau.Services.ProductService;
using Web_153504_Padvalnikau.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
UriData? uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// подтягиваем настройки  
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
builder.Services.AddScoped(SessionCart.GetCart);
builder.Services.AddScoped<PagerTagHelper>();


// add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = "cookie";
        opt.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority =
            builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.RequireHttpsMetadata = false;
        options.ClientId =
            builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret =
            builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        // Получить Claims пользователя
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });
builder.Services.AddRazorPages();


var app = builder.Build();

    
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection().UseStaticFiles().UseRouting();

app.UseLogging();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages().RequireAuthorization();

app.Run();