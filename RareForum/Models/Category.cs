using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareForum.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(Category))]
    public int? ParentCategoryId { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 50 characters.")]
    public string Title { get; set; }

    [Required]
    [StringLength(250, ErrorMessage = "Description must be less then 250 characters.")]
    public string Description { get; set; }

    public DateTime? CreatedDate { get; set; }
    
    public virtual Category? ParentCategory { get; set; }

    public virtual User? Creator { get; set; } 
    
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
    
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}