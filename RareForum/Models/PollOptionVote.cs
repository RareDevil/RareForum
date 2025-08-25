using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class PollOptionVote
{
    [Key]
    public int PollOptionVoteId { get; set; }

    [ForeignKey(nameof(PollOption))]
    public int PollOptionId { get; set; }

    [ForeignKey(nameof(RareForum.Models.User))]
    public int UserId { get; set; }

    public DateTime? VotedDate { get; set; }
    
    public virtual PollOption? ParentPollOption { get; set; }
    
    public virtual User? User { get; set; }
}