using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RareForum.Models;
using RareForum.Static;

namespace RareForum.Controllers;

[Route("/forum/categories")]
public class CategoriesController(CAuth auth, ForumDB context) : Controller
{
    private readonly ForumDB _db = context;
    private readonly CAuth _auth = auth;

    #region Category Create

    [Route("{id:int}/create")]
    public IActionResult CreateCategoryWithParentId(int id)
    {
        return _CreateCategory(id);
    }
    [Route("create")]
    public IActionResult CreateCategory()
    {
        return _CreateCategory();
    }
    
    private IActionResult _CreateCategory(int? id = null)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Category), new { id });
        }
        return View(nameof(CreateCategory), new Category() {ParentCategoryId = id});
    }
    
    [HttpPost("{id:int}/create"),HttpPost("create"),ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(int? id, Category category)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized("You are not logged in.");
        }
        
        category.ParentCategoryId = id;
        
        if (ModelState.IsValid)
        {
            category.UserId = _auth.User!.UserId;
            
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Category), new { id = category.CategoryId});
        }
        
        return View(category);
    }
    
    #endregion
    #region Category Read

    [Route("/forum")]
    public IActionResult Index()
    {
        List<Category> categories = _db.Categories
                                       .Include(service => service.ChildCategories)
                                       .Include(service => service.Posts)
                                       .Where(c => c.ParentCategoryId == null).ToList();
        return View(categories);
    }
    
    [Route("{id:int}")]
    public IActionResult Category(int? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }
        Category? category = _db.Categories
                                .Include(service => service.ChildCategories)
                                .ThenInclude(service => service.ChildCategories)
                                .Include(service => service.Posts)
                                .ThenInclude(service => service.Comments)
                                .Include(service => service.Posts)
                                .ThenInclude(service => service.Author)
                                .FirstOrDefault(x => x.CategoryId == id);
        if (category == null)
        {
            return RedirectToAction(nameof(Index));
        }
        
        return View(category);
    }

    #endregion
    #region Category Update

    [Route("update/{id:int}")]
    public IActionResult UpdateCategory(int id)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Index));
        }
        Category? category = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category == null)
        {
            return RedirectToAction(nameof(Index));
        }
        if (category.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(Index));
        }
        
        return View(category);
    }

    [HttpPost("update/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized("You are not logged in.");
        }
        Category? cat = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (cat == null)
        {
            return NotFound();
        }
        if (cat.UserId != _auth.User!.UserId)
        {
            return Unauthorized();
        }

        if (ModelState.IsValid)
        {
            cat.Title = category.Title;
            cat.Description = category.Description;
            await _db.SaveChangesAsync();
                
            if (category.ParentCategoryId != null)
            {
                return RedirectToAction(nameof(Category), new { id = cat.ParentCategoryId });
            }

            return RedirectToAction(nameof(Index));
        }
        
        return View(category);
    }
    
    #endregion
    #region Category Delete
    
    [Route("delete/{id:int}")]
    public IActionResult DeleteCategory(int id)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Index));
        }
        Category? category = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category == null)
        {
            return RedirectToAction(nameof(Index));
        }
        if (category.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(Index));
        }
        
        return View(category);
    }
    
    [HttpPost("delete/{id:int}"), ValidateAntiForgeryToken, ActionName(nameof(DeleteCategory))]
    public async Task<IActionResult> DeleteCategoryConfirm(int id)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized("You are not logged in.");
        }
        Category? cat = _db.Categories
                           .Include(x => x.ChildCategories)
                           .FirstOrDefault(c => c.CategoryId == id);
        if (cat == null)
        {
            return RedirectToAction(nameof(Index));
        }
        
        if (cat.UserId != _auth.User!.UserId)
        {
            return RedirectToAction(nameof(Index));
        }
        
        // _db.Categories.Remove(cat);
        RecursiveDelete(cat);
        await _db.SaveChangesAsync();
        if (cat.ParentCategoryId != null)
        {
            return RedirectToAction(nameof(Category), new { id = cat.ParentCategoryId });
        }
        return RedirectToAction(nameof(Index));
    }
    
    private void RecursiveDelete(Category parent)
    {
        if (parent.ChildCategories.Any())
        {
            IQueryable<Category> children = _db.Categories
                                               .Include(x => x.ChildCategories)
                                               .Where(x => x.ParentCategoryId == parent.CategoryId);

            foreach (Category child in children)
            {
                RecursiveDelete(child);
            }
        }

        _db.Remove(parent);
    }
    
    #endregion
}