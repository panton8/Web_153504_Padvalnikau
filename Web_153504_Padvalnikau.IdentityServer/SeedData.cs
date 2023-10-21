using System.Security.Claims;
using IdentityModel;
using Web_153504_Padvalnikau.IdentityServer.Data;
using Web_153504_Padvalnikau.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Web_153504_Padvalnikau.IdentityServer;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await AddRoles(roleManager);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await AddUsers(userManager);
        }
    }

    private static async Task AddUsers(UserManager<ApplicationUser> userManager)
    {
        var alice = userManager.FindByNameAsync("alice").Result;
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(alice, "Pass123$");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userManager.AddClaimsAsync(alice, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }


        var user = userManager.FindByNameAsync("user").Result;
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "user",
                Email = "user@gmail.com",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Pass123$");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await userManager.AddToRoleAsync(user, "user");

            Log.Debug("user created");
        }
        else
        {
            Log.Debug("user already exists");
        }


        var admin = userManager.FindByNameAsync("admin").Result;
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(admin, "Pass123$");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await userManager.AddToRoleAsync(admin, "admin");

            Log.Debug("user created");
        }
        else
        {
            Log.Debug("user already exists");
        }
    }

    private static async Task AddRoles(RoleManager<IdentityRole> roleManager)
    {
        var userRole = roleManager.FindByNameAsync("user").Result;
        if (userRole == null)
        {
            userRole = new IdentityRole
            {
                Name = "user",
            };
            var result = await roleManager.CreateAsync(userRole);
            if (result.Errors.Any())
                throw new Exception(result.Errors.First().Description);
        }
        else
        {
            Log.Debug("user role already exists");
        }

        var adminRole = roleManager.FindByNameAsync("admin").Result;
        if (adminRole == null)
        {
            adminRole = new IdentityRole
            {
                Name = "admin",
            };
            var result = await roleManager.CreateAsync(adminRole);
            if (result.Errors.Any())
                throw new Exception(result.Errors.First().Description);
        }
        else
        {
            Log.Debug("admin role already exists");
        }
    }
}