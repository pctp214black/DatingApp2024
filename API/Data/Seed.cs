namespace API.Data;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

public class Seed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        var userData = await File.ReadAllTextAsync("Data/UsersSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, ReadOptions);

        if (users == null)
        {
            return;
        }

        var roles = new List<AppRole>{
            new() {Name = "Admin"},
            new() {Name = "Member"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
        var admin = new AppUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",
            City = "",
            Country = ""
        };
        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);

        foreach (var user in users)
        {
            // using var hmac = new HMACSHA512();

            // user.UserName = user.UserName.ToLowerInvariant();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
            // user.PasswordSalt = hmac.Key;

            // context.Users.Add(user);
            user.UserName = user.UserName!.ToLowerInvariant();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        // await context.SaveChangesAsync();
    }

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        AllowTrailingCommas = true
    };
}