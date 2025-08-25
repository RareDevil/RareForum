using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class Post
{
    [Key]
    public int PostId { get; set; }

    [ForeignKey(nameof(RareForum.Models.Category))]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
    public string Title { get; set; }
    
    [Required]
    [MinLength(3, ErrorMessage = "Message must be more then 3 characters.")]
    public string Content { get; set; }
    
    public DateTime? CreatedDate { get; set; }

    public bool IsLocked { get; set; } = false;

    public bool IsPoll { get; set; } = false;
    
    public virtual Category? Category { get; set; }
    
    public virtual User? Author { get; set; }
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public virtual ICollection<PollOption> PollOptions { get; set; } = new List<PollOption>();
}