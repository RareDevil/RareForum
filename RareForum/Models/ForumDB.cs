using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RareForum.Models;

public class ForumDB(DbContextOptions<ForumDB> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserImage> UserImages { get; set; }
    // Forum part
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    // Optional
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<PollOptionVote> PollOptionVotes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User Table changes
        modelBuilder.Entity<User>()
                    .Property(b => b.CreatedDate)
                    // This is how you set default in the sql server
                    .HasDefaultValueSql("getdate()") 
                    // This will make sure that the value is not updated after insert.
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); 
        #endregion
        
        #region Category Table changes
        modelBuilder.Entity<Category>()
                    .Property(b => b.CreatedDate)
                    .HasDefaultValueSql("getdate()")
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); 
        modelBuilder.Entity<Category>()
                    .Property(b => b.ParentCategoryId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Category>()
                    .Property(b => b.UserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        #endregion
        
        #region Post Table changes
        modelBuilder.Entity<Post>()
                    .Property(b => b.CreatedDate)
                    .HasDefaultValueSql("getdate()")
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Post>()
                    .Property(b => b.CategoryId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Post>()
                    .Property(b => b.UserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        
        modelBuilder.Entity<Post>()
                    .HasOne(e => e.Author)
                    .WithMany(e => e.Posts)
                    .OnDelete(DeleteBehavior.NoAction);
        #endregion
        
        #region Comment Table changes
        modelBuilder.Entity<Comment>()
                    .Property(b => b.CreatedDate)
                    .HasDefaultValueSql("getdate()")
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Comment>()
                    .Property(b => b.PostId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<Comment>()
                    .Property(b => b.UserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        
        modelBuilder.Entity<Comment>()
                    .HasOne(e => e.Author)
                    .WithMany(e => e.Comments)
                    .OnDelete(DeleteBehavior.NoAction);
        #endregion
        
        #region Poll option Table changes
        modelBuilder.Entity<PollOption>()
                    .Property(b => b.PostId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        #endregion
        
        #region Poll option vote table changes
        modelBuilder.Entity<PollOptionVote>()
                    .Property(b => b.VotedDate)
                    .HasDefaultValueSql("getdate()")
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<PollOptionVote>()
                    .Property(b => b.PollOptionId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<PollOptionVote>()
                    .Property(b => b.UserId)
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        
        modelBuilder.Entity<PollOptionVote>()
                    .HasOne(e => e.User)
                    .WithMany(e => e.PollOptionVotes)
                    .OnDelete(DeleteBehavior.NoAction);
        #endregion
    }
}