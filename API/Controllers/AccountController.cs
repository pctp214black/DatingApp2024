namespace API.Controllers;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;

public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await UserExistsAsync(request.Username))
        {
            return BadRequest("Username already in use");
        }

        using var hmac = new HMACSHA512();
        var user = mapper.Map<AppUser>(request);
        user.UserName = request.Username.ToLowerInvariant();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        // user.PasswordSalt = hmac.Key;

        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == request.UserName.ToUpperInvariant());

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        // using var hmac = new HMACSHA512(user.PasswordSalt);
        // var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        // for (var i = 0; i < computeHash.Length; i++)
        // {
        //     if (computeHash[i] != user.PasswordHash[i])
        //     {
        //         return Unauthorized("Invalid username or password");
        //     }
        // }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return Unauthorized("Invalid username or password");
        }

        return new UserResponse
        {
            Username = user.UserName!,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };
    }

    private async Task<bool> UserExistsAsync(string username) =>
        await userManager.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpperInvariant());
}