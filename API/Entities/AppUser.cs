using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities;


public class AppUser
{
    //Las data anotation es como decirle al compilador donde se encuentra determinada caracteristica de la tabla
    //En este caso especificamos que la llave de la tabla es idUsuario
    /*
    [Key]
    public int IdUser {get;set;}
    */
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateOnly BirthDay { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public List<Photo> Photos { get; set; } = [];

    public List<UserLike> LikedByUsers { get; set; } = [];

    public List<UserLike> LikedUsers { get; set; } = [];

    // public int GetAge() => BirthDay.CalculateAge();
}