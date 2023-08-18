using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccManagment.API.Entities;

public class User 
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(256)]
    public string Username { get; set; }
    [Required]
    public bool Has2Fac { get; set; } = false;
    public string Password { get; set; }
    public string? SecretCode { get; set; } = null;
    public string? Token { get; set; } = null;
    
    public byte[] FaceIdImageData { get; set; }

}