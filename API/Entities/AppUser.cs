using System.ComponentModel.DataAnnotations;

namespace API.Entities;


public class AppUser{
    //Las data anotation es como decirle al compilador donde se encuentra determinada caracteristica de la tabla
    //En este caso especificamos que la llave de la tabla es idUsuario
    /*
    [Key]
    public int IdUser {get;set;}
    */
    public int Id {get;set;}
    public required string UserName {get;set;}
    
}