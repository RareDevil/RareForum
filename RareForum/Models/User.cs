using System.ComponentModel.DataAnnotations;

namespace RareForum.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(50, ErrorMessage = "Email is too long.")]
    public string Email { get; set; }

    [MaxLength(50, ErrorMessage = "First name is too long.")]
    public string? FirstName { get; set; }
    
    [MaxLength(50, ErrorMessage = "Lastname is too long.")]
    public string? LastName { get; set; }
    
    [CheckBirthday]
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
    
    [MaxLength(50, ErrorMessage = "Location is too long.")]
    public string? Location { get; set; }
    
    public int? PhoneNumber { get; set; }
    
    public string? Signature { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<PollOptionVote> PollOptionVotes { get; set; } = new List<PollOptionVote>();
    
    public virtual UserImage? UserImage { get; set; }
    
}
/*
 * This is only used for password change!
 */
public class UserPasswordUpdate
{
    [Required(ErrorMessage = "Old password is required")]
    public string? OldPassword { get; set; }
    
    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; set; }
}