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
builder.Services.AddHttpContextAccessor();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages().RequireAuthorization();

app.Run();