using Microsoft.EntityFrameworkCore;
using Web_153504_Padvalnikau.Domain.Entities;

namespace Web_153504_Padvalnikau.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Sneaker> Sneakers { get; set; }
    public DbSet<Category> Categories { get; set; } 
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}