using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class PollOption
{
    [Key]
    public int PollOptionId { get; set; }

    [ForeignKey(nameof(RareForum.Models.Post))]
    public int PostId { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Option must be between 3 and 50 characters.")]
    public string Title { get; set; }
    
    public virtual Post? Post { get; set; }
    
    public virtual ICollection<PollOptionVote> Votes { get; set; } = new List<PollOptionVote>();
}