namespace API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;
    private readonly IUserRepository _repository;

    public UsersController(DataContext context, IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MemberReponse>>> GetUsersAsync()
    {
        var users = await _repository.GetAllAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUserById(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return NotFound();
        return user;
    }
    [HttpGet("{username}")]
    public async Task<ActionResult<AppUser>> GetByUsernameAsync(string username)
    {
        var user = await _repository.GetByUsernameAsync(username);

        if (user == null)
            return NotFound();
        return user;
    }
}
