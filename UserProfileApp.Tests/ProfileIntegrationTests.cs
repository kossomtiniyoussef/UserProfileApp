using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserProfileApp.Data;
using UserProfileApp.Models;
using Xunit;

namespace UserProfileApp.Tests
{
    public class ProfileIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly SqliteConnection _connection;

        public ProfileIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Open an in-memory Sqlite connection. Keeping it open keeps the database alive.
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                // Correctly set the environment to Testing for WebApplicationBuilder
                builder.UseSetting(WebHostDefaults.EnvironmentKey, "Testing");

                builder.ConfigureServices(services =>
                {
                    // Register ApplicationDbContext using our open in-memory Sqlite connection
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite(_connection);
                    });

                    // Build service provider and ensure tables are created and seeded
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    db.Database.EnsureCreated();

                    if (!db.Users.Any())
                    {
                        db.Users.Add(new User
                        {
                            UserId = 1,
                            Username = "integrationuser",
                            Email = "integration@example.com",
                            PasswordHash = "hash",
                            FirstName = "Integration",
                            LastName = "Test",
                            Bio = "Integration testing user",
                            ProfilePictureUrl = "https://via.placeholder.com/150"
                        });
                        db.SaveChanges();
                    }
                });
            });
        }

        [Fact]
        public async Task Get_ProfileDetailsEndpoint_ReturnsSuccessAndHtmlContent()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });

            // Act
            var response = await client.GetAsync("/Profile/Details/1");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Verify response contains expected content
            Assert.Contains("Integration", responseString);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _factory.Dispose();
        }
    }
}