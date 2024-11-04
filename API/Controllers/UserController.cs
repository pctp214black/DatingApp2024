namespace API.Controllers;

using System.Security.Claims;
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
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _repository = userRepository;
        _mapper = mapper;
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

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateRequest request)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (username == null)
            return BadRequest("No username found in token");

        var user = await _repository.GetByUsernameAsync(username);
        if (user == null)
        {
            return BadRequest("Could not find user");
        }
        _mapper.Map(request, user);
        _repository.Update(user);
        if (await _repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Update user failed");
    }
}
