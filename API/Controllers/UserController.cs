namespace API.Controllers;

using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;
    private readonly IPhotoService _photoService;

    private readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
    {
        _repository = userRepository;
        _photoService = photoService;
        _mapper = mapper;

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MemberReponse>>> GetUsersAsync(UserParams userParams)
    {
        var members = await _repository.GetMembersAsync(userParams);
        Response.AddPaginationHeader(members);
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
    [HttpGet("{username}", Name = "GetByUsername")]
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
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
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

    [HttpPost("photo")]
    public async Task<ActionResult<PhotoResponse>> AddPhoto(IFormFile file)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("Cannot update user");
        }
        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }
        user.Photos.Add(photo);
        if (await _repository.SaveAllAsync())
        {
            return CreatedAtAction("GetByUsername", new { username = user.UserName }, _mapper.Map<PhotoResponse>(photo));
        }
        return BadRequest("Problem adding the photo");
    }

    [HttpPut("photo/{photoId:int}")]
    public async Task<ActionResult> SetPhotoAsMain(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("User not found");
        }
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null || photo.IsMain)
            return BadRequest("Cannot set this photo as the main one");
        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null)
        {
            currentMain.IsMain = false;
        }
        photo.IsMain = true;
        if (await _repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("There was a problem");
    }

    [HttpDelete("photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("User not found");
        }
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null || photo.IsMain)
        {
            return BadRequest("This photo cant be deleted");
        }

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null)
                return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if (await _repository.SaveAllAsync())
            return Ok();
        return BadRequest("There was a problem when deleting the photo");
    }

}
