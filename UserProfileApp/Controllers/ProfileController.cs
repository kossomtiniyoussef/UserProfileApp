using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileApp.Data;
using UserProfileApp.Models;

namespace UserProfileApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                id = 1; 
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                // If no user exists yet, seed a default one for easy testing
                return RedirectToAction(nameof(CreateSampleUser));
            }

            return View(user);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) id = 1;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,FirstName,LastName,Bio,ProfilePictureUrl,PasswordHash,CreatedAt")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UpdatedAt = DateTime.UtcNow;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.UserId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = user.UserId });
            }
            return View(user);
        }

        
        public async Task<IActionResult> CreateSampleUser()
        {
            if (!_context.Users.Any())
            {
                var sampleUser = new User
                {
                    Username = "johndoe",
                    Email = "john.doe@example.com",
                    PasswordHash = "hashed_dummy_password",
                    FirstName = "John",
                    LastName = "Doe",
                    Bio = "Cloud enthusiast and DevOps practitioner.",
                    ProfilePictureUrl = "https://via.placeholder.com/150"
                };
                _context.Users.Add(sampleUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = 1 });
        }
    }
}