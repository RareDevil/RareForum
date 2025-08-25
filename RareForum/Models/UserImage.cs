using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class UserImage
{
    [Key]
    [ForeignKey(nameof(RareForum.Models.User))]
    public int UserId { get; set; }
    
    [Required]
    public byte[] Image { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string ImageType { get; set; }
    
    public virtual User? User { get; set; }
}

public class UserImageUpload
{
    public IFormFile Image { get; set; }
}