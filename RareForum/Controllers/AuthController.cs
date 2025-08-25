using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RareForum.Models;
using RareForum.Static;

namespace RareForum.Controllers;

public class AuthController(CAuth auth, ForumDB context) : Controller
{
    private readonly ForumDB _db = context;
    private readonly CAuth _auth = auth;
    
    // GET
    [Route("/user")]
    public IActionResult Index()
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Login));
        }
        
        return View(_auth.User);
    }

    [HttpPost("update")]
    [ValidateAntiForgeryToken] // [Bind("Username,Email")]
    public async Task<IActionResult> Update(User user)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Login));
        }
        // Only the logged in user can change their own details.
        if (user.UserId != _auth.User!.UserId)
        {
            return Unauthorized("You are not authorized to update other users!");
        }
        // Password is not allowed to be updated here
        ModelState.Remove(nameof(RareForum.Models.User.Password));
        // This is to make sure that the user does not try to update the password.
        user.Password = _auth.User!.Password;
        if (ModelState.IsValid)
        {
            User? exsisting = _db.Users.FirstOrDefault(u => (u.Username.ToLower() == user.Username.ToLower() || 
                                                             u.Email.ToLower() == user.Email.ToLower()) && 
                                                            u.UserId != user.UserId); // Should not find the current user
            if (exsisting == null)
            {
                try
                {
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    // Make sure that auth user is updated.
                    _auth.User = user;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Sanity check just to be totally sure!
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else if (exsisting.UserId != user.UserId)
            {
                if (exsisting.Username == user.Username)
                {
                    ModelState.AddModelError("Username", "Username already taken");
                }

                if (exsisting.Email == user.Email)
                {
                    ModelState.AddModelError("Email", "Email already taken");
                }
            }
        }
        
        return RedirectToAction(nameof(Index));
    }

    [Route("/updatepassword")]
    public IActionResult UpdatePassword()
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Login));
        }

        return View();
    }
    
    [HttpPost("updatepassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(UserPasswordUpdate userPu)
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Login));
        }
        
        if (ModelState.IsValid)
        {
            if (userPu.OldPassword == userPu.NewPassword)
            {
                ModelState.AddModelError("NewPassword", "Match new password");
                ModelState.AddModelError("OldPassword", "Match old password");
                return View(userPu);
            }
            
            User? dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == _auth.User!.UserId);
            if (dbUser != null)
            {
                if (dbUser.Password != userPu.OldPassword)
                {
                    ModelState.AddModelError("OldPassword", "Does not match old password");
                    return View(userPu);
                }
                // This is just a sanity check.
                if (dbUser.Password == userPu.NewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Match old password");
                    return View(userPu);
                }
                
                // new password can not be null here...
                dbUser.Password = userPu.NewPassword!;
                await _db.SaveChangesAsync();
                // Just to make sure that is has been updated..
                _auth.User = dbUser;
                
                return RedirectToAction(nameof(Index));
            }
        }
        return View(userPu);
    }
    
    [Route("/login")]
    public IActionResult Login()
    {
        if (_auth.Autherized)
        {
            return RedirectToAction(nameof(Index));
        }
        
        return View();
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Username,Password")] User user)
    {
        // Fjern Email fra ModelState så den ikke bliver tjekket for om den valid
        ModelState.Remove(nameof(RareForum.Models.User.Email));
        if (ModelState.IsValid)
        {
            User? dbuser = await _db.Users.FirstOrDefaultAsync(u => 
                                                        u.Username.ToLower() == user.Username.ToLower() && 
                                                        u.Password == user.Password);
            if (dbuser != null)
            {
                _auth.User = dbuser;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Username or password is incorrect.");
            }
        }
        return View(user);
    }

    [Route("/register")] 
    public IActionResult Register()
    {
        if (_auth.Autherized)
        {
            return RedirectToAction(nameof(Index));
        }
        
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            // BRUGER - bruger
            User? exsisting = _db.Users.FirstOrDefault(u => 
                                                           u.Username.ToLower() == user.Username.ToLower() || 
                                                           u.Email.ToLower() == user.Email.ToLower());
            if (exsisting == null)
            {
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                // user.Id bliver automatisk sat efter _db.SaveChangesAsync() i følge hvad jeg kan finde på nettet.
                _auth.User = user; 
                // Hop til auth index 
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Username or Email is already in use.");
            }
        }
        return View(user);
    }

    [Route("/logout")] 
    public IActionResult Logout()
    {
        if (!_auth.Autherized)
        {
            return RedirectToAction(nameof(Login));
        }
        
        _auth.User = null;
        return View();
    }

    [HttpGet("user/image/{userId:int}")]
    public FileResult ProfileImage(int userId)
    {
        // This is the default profile image if a user does not have a image
        byte[] img = System.IO.File.ReadAllBytes("static/profile_img.jpg");
        // The content type for the default image is image/jpeg
        string contentType = "image/jpeg";
        // Load image from database if exists

        UserImage? userImage = _db.UserImages.FirstOrDefault(u => u.UserId == userId);
        if (userImage != null)
        {
            img = userImage.Image;
            contentType = userImage.ImageType;
        }
        
        return File(img, contentType);
    }

    private readonly HashSet<string> _allowedContentTypes = ["image/jpeg", "image/png"];
    
    [HttpPost("user/image/upload"), ValidateAntiForgeryToken]
    public async Task<IActionResult> ProfileImageUpload(UserImageUpload data)
    {
        if (!_auth.Autherized)
        {
            return Unauthorized();
        }
        
        IFormFile image = data.Image;
        // Do not allow images that do not have content type image/jpeg or image/png
        if (!_allowedContentTypes.Contains(image.ContentType.ToLower()))
        {
            return BadRequest("Invalid image type");
        }
        // Limit the file size to 1mb
        if (image.Length < 0 || image.Length > 1000000)
        {
            return BadRequest("File is too large.");
        }
        
        using MemoryStream memoryStream = new MemoryStream();
        await data.Image.CopyToAsync(memoryStream);
        // Validate that the file size is not over 1mb!
        
        byte[] fileBytes = memoryStream.ToArray();
        // Insert or Update database here... 

        if (_db.UserImages.Any(u => u.UserId == _auth.User!.UserId))
        {
            _db.UserImages.Update(new UserImage()
            {
                UserId = _auth.User!.UserId,
                Image = fileBytes,
                ImageType = image.ContentType
            });
        }
        else
        {
            _db.UserImages.Add(new UserImage()
            {
                UserId = _auth.User!.UserId,
                Image = fileBytes,
                ImageType = image.ContentType
            });
        }

        await _db.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    private bool UserExists(int id)
    {
        return _db.Users.Any(e => e.UserId == id);
    }
}