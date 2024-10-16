namespace API.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    //EF Navigation Properties
    public int AppUserId { get; set; }
    //El ! forza que se tome como valor nulo
    public AppUser AppUser { get; set; } = null!;
}