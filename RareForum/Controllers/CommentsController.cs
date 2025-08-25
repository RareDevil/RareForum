using Microsoft.AspNetCore.Mvc;
using RareForum.Models;
using RareForum.Static;

namespace RareForum.Controllers;

[Route("/forum/categories/{categoryId:int}/posts/{postId:int}/comments")]
public class CommentsController(CAuth auth, ForumDB context) : Controller
{
    private readonly ForumDB _db = context;
    private readonly CAuth _auth = auth;
    
    #region Comment Create

    [HttpPost("create")]
    public async Task<IActionResult> CreateComment(int categoryId, int postId, Comment comment)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized();
        }
        
        if (postId != comment.PostId || 
            !_db.Categories.Any(c => c.CategoryId == categoryId) || 
            !_db.Posts.Any(p => p.PostId == postId) || 
            !ModelState.IsValid)
        {
            return BadRequest();
        }

        comment.UserId = _auth.User!.UserId;
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        
        // Save the comment 
        return RedirectToAction(nameof(PostsController.Index), "Posts", new { id = postId, categoryId = categoryId });
    }

    #endregion
    #region Comment Read

    // What did you expect? A custom view for a comment? 

    #endregion
    #region Comment Update

    [Route("update/{id:int}")]
    public IActionResult UpdateComment(int categoryId, int postId, int id)
    {
        IActionResult? test = _validateGetCall(categoryId, postId, id, out Comment? comment);
        if (test != null)
        {
            return test;
        }

        ViewBag.CategoryId = categoryId;
        
        return View(comment);
    }
    
    [HttpPost("update/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateComment(int categoryId, int postId, int id, Comment comment)
    {
        IActionResult? test = _validatePostCall(categoryId, postId, id, out Comment? dbComment);
        if (test != null)
        {
            return test;
        }
        
        if (ModelState.IsValid)
        { // dbComment cant not be null here.
            dbComment!.Content = comment.Content;
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(PostsController.Index), "Posts", new { id = postId, categoryId = categoryId });
        }

        ViewBag.CategoryId = categoryId;
        
        return View(comment);
    }

    #endregion
    #region Comment Delete

    [Route("delete/{id:int}")]
    public IActionResult DeleteComment(int categoryId, int postId, int id)
    {
        IActionResult? test = _validateGetCall(categoryId, postId, id, out Comment? comment);
        if (test != null)
        {
            return test;
        }
        
        ViewBag.CategoryId = categoryId;
        return View(comment);
    }
    
    [HttpPost("delete/{id:int}"), ValidateAntiForgeryToken, ActionName(nameof(DeleteComment))]
    public async Task<IActionResult> DeleteCommentConfirmation(int categoryId, int postId, int id)
    {
        IActionResult? test = _validatePostCall(categoryId, postId, id, out Comment? comment);
        if (test != null)
        {
            return test;
        }
        
        _db.Remove(comment!);
        await _db.SaveChangesAsync();
        
        return RedirectToAction(nameof(PostsController.Index), "Posts", new { id = postId, categoryId = categoryId });
    }

    #endregion

    #region Validation functions
    
    private IActionResult? _validateGetCall(int categoryId, int postId, int id, out Comment? dbComment)
    {
        dbComment = null;
        if (!_auth.Autherized ||
            !_db.Categories.Any(c => c.CategoryId == categoryId))
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }

        if (!_db.Posts.Any(p => p.PostId == postId))
        {
            return RedirectToAction(nameof(CategoriesController.Category), "Categories", new { id = categoryId });
        }
        
        dbComment = _db.Comments.FirstOrDefault(c => c.CommentId == id);
        if (dbComment == null || 
            dbComment.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(PostsController.Index), "Posts", new { id = postId, categoryId = categoryId });
        }

        return null;
    }

    private IActionResult? _validatePostCall(int categoryId, int postId, int id, out Comment? dbComment)
    {
        dbComment = null;
        if (!_auth.Autherized)
        {
            return Unauthorized();
        }
        if (!_db.Categories.Any(c => c.CategoryId == categoryId) || 
            !_db.Posts.Any(p => p.PostId == postId))
        {
            return BadRequest();
        }
        
        dbComment = _db.Comments.FirstOrDefault(c => c.CommentId == id);
        if (dbComment == null)
        {
            return BadRequest();
        }

        if (dbComment.UserId != _auth.User!.UserId)
        {
            return Unauthorized();
        }

        return null;
    }
    
    #endregion
    
}