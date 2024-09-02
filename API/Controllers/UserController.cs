using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController:ControllerBase{
    private readonly DataContext _context;

    public UsersController(DataContext context){
        _context=context;
    }

    [HttpGet]
    public ActionResult<List<AppUser>> GetUsers(){
        var users=_context.Users.ToList();
        return users;
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUserById(int id){
        var user=_context.Users.Find(id);

        if(user==null) return NotFound();
        return user;
    }
}
