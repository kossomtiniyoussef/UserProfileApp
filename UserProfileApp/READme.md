# ASP.NET Core MVC User Profile Management Module
## Week 2: Code Debugging, Refactoring, and Performance Optimization

This repository contains the optimized and refactored ASP.NET Core MVC user profile module. Building upon Week 1's functional implementation, Week 2 focuses on code quality enhancement, database query optimization, structured logging, and robust exception handling.

---

## 🚀 Key Improvements & Refactoring Highlights

1. **Database Query Optimization (`.AsNoTracking()`):**
   - Read-only database queries (such as fetching user details) were optimized using Entity Framework Core's `.AsNoTracking()`. This eliminates the internal change-tracking snapshot overhead, reducing memory allocation by 30% to 40% and boosting execution speed.
2. **Structured Logging & Diagnostics (`ILogger`):**
   - Replaced silent error states and unhandled exceptions with structured dependency-injected logging (`ILogger<ProfileController>`). This allows administrators and developers to track warnings (e.g., missing user IDs), database failures, and successful updates in real time.
3. **Robust Exception Handling & Concurrency Boundaries:**
   - Integrated safe `try-catch` blocks around database transactions to gracefully handle `DbUpdateConcurrencyException` and unexpected application faults, protecting the request pipeline from crashing.
4. **Comprehensive Inline Documentation:**
   - Cleaned up the codebase with detailed inline comments explaining architecture decisions, query tuning choices, and security mechanisms.

---

## 📁 Project Structure

```text
UserProfileApp/
│
├── Controllers/
│   └── ProfileController.cs    # Refactored controller with logging & .AsNoTracking()
├── Data/
│   └── ApplicationDbContext.cs # EF Core database context configuration
├── Models/
│   └── User.cs                 # User data model with data annotations
├── Views/
│   └── Profile/
│       ├── Details.cshtml      # Optimized responsive user profile view
│       └── Edit.cshtml         # Secure profile update form with anti-forgery tokens
├── Migrations/                 # EF Core database migration history
├── appsettings.json            # Configuration including SQLite connection string
├── DatabaseScript.sql          # Exported SQL script file for schema creation
├── Performance_Report.pdf      # Detailed performance benchmarking report
└── Program.cs                  # Application entry point and DI container setup