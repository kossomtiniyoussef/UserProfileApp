using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserProfileApp.Controllers;
using UserProfileApp.Data;
using UserProfileApp.Models;
using Xunit;

namespace UserProfileApp.Tests
{
    public class ProfileControllerTests
    {
        
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithUser_WhenUserExists()
        {
            
            var context = GetInMemoryDbContext();
            var testUser = new User
            {
                UserId = 1,
                Username = "janedoe",
                Email = "jane.doe@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                PasswordHash = "dummy_hash",
                Bio = "Software developer testing environment", 
                ProfilePictureUrl = "https://via.placeholder.com/150" 
            };
            
            context.Users.Add(testUser);
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<ProfileController>>();
            var controller = new ProfileController(context, mockLogger.Object);

            
            var result = await controller.Details(1);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.Model);
            Assert.Equal("janedoe", model.Username);
            Assert.Equal("Jane", model.FirstName);
        }

        [Fact]
        public async Task Details_RedirectsToSampleUser_WhenUserNotFound()
        {
            
            var context = GetInMemoryDbContext(); 
            var mockLogger = new Mock<ILogger<ProfileController>>();
            var controller = new ProfileController(context, mockLogger.Object);

            
            var result = await controller.Details(99); 

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(ProfileController.CreateSampleUser), redirectToActionResult.ActionName);
        }
    }
}