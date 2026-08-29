using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileApp.Data;
using UserProfileApp.Models;

namespace UserProfileApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

                public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                int targetId = id ?? 1; 

                var user = await _context.Users
                    .AsNoTracking() 
                    .FirstOrDefaultAsync(m => m.UserId == targetId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found. Redirecting to sample creation.", targetId);
                    return RedirectToAction(nameof(CreateSampleUser));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user profile details for ID {Id}", id);
                return StatusCode(500, "Internal server error occurred while retrieving the profile.");
            }
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) id = 1;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Edit attempt failed: User ID {Id} not found.", id);
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
                _logger.LogWarning("Concurrency/ID mismatch error: URL ID {UrlId} does not match Form ID {FormId}", id, user.UserId);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UpdatedAt = DateTime.UtcNow;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Successfully updated profile for user ID {UserId}", user.UserId);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!UserExists(user.UserId))
                    {
                        _logger.LogWarning(ex, "Concurrency update failed: User ID {UserId} no longer exists.", user.UserId);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Concurrency exception encountered while updating user ID {UserId}", user.UserId);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error saving profile changes for user ID {UserId}", user.UserId);
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving. Please try again.");
                    return View(user);
                }

                return RedirectToAction(nameof(Details), new { id = user.UserId });
            }

            _logger.LogWarning("Model validation failed during profile update for user ID {UserId}", user.UserId);
            return View(user);
        }

        
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        
        public async Task<IActionResult> CreateSampleUser()
        {
            if (!await _context.Users.AnyAsync())
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
                _logger.LogInformation("Default sample user successfully seeded.");
            }
            return RedirectToAction(nameof(Details), new { id = 1 });
        }
    }
}