using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RareForum.Models;

namespace RareForum.Controllers;

public class HomeController(ILogger<HomeController> logger, ForumDB context) : Controller
{
    private readonly ForumDB _db = context;
    
    public IActionResult Index()
    {
        List<Post> top9Posts = _db.Posts.OrderByDescending(p => p.CreatedDate).Take(9).ToList();
        
        return View(top9Posts);
    }

    [Route("/about")]
    public IActionResult About()
    {
        return View();
    }

    [Route("/privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}