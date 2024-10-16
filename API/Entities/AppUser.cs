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

    public required byte[] PasswordHash { get; set; } = [];
    public required byte[] PasswordSalt { get; set; } = [];
    public DateOnly BirthDay { get; set; }
    public required string KnowAs { get; set; }
    public DateTime Created { get; set; }
    public DateTime LasActive { get; set; }
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<Photo> Photosp { get; set; } = [];
    public int GetAge() => BirthDay.CalculateAge();
}