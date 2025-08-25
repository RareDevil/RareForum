using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RareForum.Models;
using RareForum.Static;

namespace RareForum.Controllers;

[Route("/forum/categories/{categoryId:int}/posts")]
public class PostsController(CAuth auth, ForumDB context) : Controller
{
    private readonly ForumDB _db = context;
    private readonly CAuth _auth = auth;

    #region Post Create

    [Route("create")]
    public IActionResult CreatePost(int categoryId)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        
        if (!_db.Categories.Any(c => c.CategoryId == categoryId))
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }

        return View(new Post { CategoryId = categoryId });
    }
    
    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(int categoryId, Post post)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        if (!_db.Categories.Any(c => c.CategoryId == categoryId))
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }

        if (ModelState.IsValid)
        {
            post.CategoryId = categoryId;
            post.UserId = _auth.User!.UserId;
            
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId, id = post.PostId });
        }
        
        return View(post);
    }

    #endregion
    #region Post Read

    [Route("{id:int}")]
    public IActionResult Index(int categoryId, int? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        
        Post? post = _db.Posts
                        .Include(p => p.Comments)
                        .ThenInclude(c => c.Author)
                        .ThenInclude(u => u!.Posts)
                        .Include(p => p.Author)
                        .ThenInclude(u => u!.Posts)
                        .FirstOrDefault(p => p.PostId == id);
        if (post == null)
        {
            return RedirectToAction(nameof(CategoriesController.Index),"Categories");
        }
        return View(post);
    }
    
    #endregion
    #region Post Update
    
    [Route("update/{id:int}")]
    public IActionResult UpdatePost(int categoryId, int id)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        Post? post = _db.Posts.FirstOrDefault(p => p.PostId == id);
        if (post == null || 
            post.CategoryId != categoryId || 
            post.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        
        return View(post);
    }

    [HttpPost("update/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePost(int categoryId, int id, Post post)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized();
        }
        Post? dbPost = _db.Posts.FirstOrDefault(p => p.PostId == id);
        if (dbPost == null || dbPost.CategoryId != categoryId)
        {
            return NotFound();
        }

        if (dbPost.UserId != _auth.User!.UserId)
        {
            return Unauthorized();
        }

        if (ModelState.IsValid)
        {
            dbPost.Title = post.Title;
            dbPost.Content = post.Content;
            
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index), new { categoryId = dbPost.CategoryId, id = dbPost.PostId });
        }
        
        post.CategoryId = categoryId;
        post.PostId = id;
        return View(post);
    }
    
    #endregion
    #region Post Delete

    [Route("delete/{id:int}")]
    public IActionResult DeletePost(int categoryId, int id)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        Post? post = _db.Posts.FirstOrDefault(p => p.PostId == id);
        if (post == null || 
            post.CategoryId != categoryId || 
            post.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(CategoriesController.Index), "Categories");
        }
        
        return View(new Post { CategoryId = categoryId, PostId = id });
    }
    
    [HttpPost("delete/{id:int}"), ValidateAntiForgeryToken, ActionName(nameof(DeletePost))]
    public async Task<IActionResult> DeletePostConfirm(int categoryId, int id)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized();
        }
        Post? dbPost = _db.Posts.FirstOrDefault(p => p.PostId == id);
        if (dbPost == null || dbPost.CategoryId != categoryId)
        {
            return NotFound();
        }

        if (dbPost.UserId != _auth.User!.UserId)
        {
            return Unauthorized();
        }
        
        _db.Remove(dbPost);
        await _db.SaveChangesAsync();
        
        return RedirectToAction(nameof(CategoriesController.Category), "Categories", new { id = categoryId });
    }
    
    #endregion
    
}