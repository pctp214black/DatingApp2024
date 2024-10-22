namespace API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper, IUserRepository userRepository)
    {
        _repository = userRepository;
        _mapper=mapper;
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
    public async Task<ActionResult<MemberReponse>> GetUserById(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return NotFound();
        return _mapper.Map<MemberReponse>(user);
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
