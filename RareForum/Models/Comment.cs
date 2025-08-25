using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [ForeignKey(nameof(RareForum.Models.Post))]
    public int PostId { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Message must be more then 3 characters.")]
    public string Content { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    public virtual User? Author { get; set; }
    
    public virtual Post? Post { get; set; }
}