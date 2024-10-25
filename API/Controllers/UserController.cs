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

    public UsersController(IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MemberReponse>>> GetUsersAsync()
    {
        var members = await _repository.GetMembersAsync();
        return Ok(members);
    }

    // [Authorize]
    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<MemberReponse>> GetUserById(int id)
    // {
    //     var user = await _repository.GetByIdAsync(id);

    //     if (user == null)
    //         return NotFound();
    //     return _mapper.Map<MemberReponse>(user);
    // }
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberReponse>> GetByUsernameAsync(string username)
    {
        var member = await _repository.GetMemeberAsync(username);

        if (member == null)
            return NotFound();
        return member;
    }
}
