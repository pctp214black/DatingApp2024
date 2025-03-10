namespace API.Controllers;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AccountController(
    DataContext context
    , ITokenService tokenService,
    IMapper mapper) : BaseApiController
{


    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (await UserExistsAsync(request.UserName))
            return BadRequest("Username already exists");
        using var hmac = new HMACSHA512();
        var user = mapper.Map<AppUser>(request);
        user.UserName = request.UserName;
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        user.PasswordSalt = hmac.Key;

        // var user = new AppUser
        // {
        //     UserName = request.UserName,
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
        //     PasswordSalt = hmac.Key
        // };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
    {
        var user = await context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x =>
            x.UserName.ToLower() == request.UserName.ToLower()
        );
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i])

            {
                return Unauthorized("Invalid username or password");
            }
        }

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            KnownAs = user.KnownAs
        };
    }

    private async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(
            user => user.UserName.ToUpper() == username.ToUpper()
        );
    }
}