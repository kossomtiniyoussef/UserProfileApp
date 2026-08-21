# Week 1 Design Document: ASP.NET MVC User Profile Feature

## 1. Architecture & Design Decisions
- **Framework:** Built using ASP.NET Core MVC following the Model-View-Controller architectural pattern to ensure clear separation of concerns.
- **Database & ORM:** Utilized Entity Framework Core with SQLite for lightweight, reliable local data management and zero-friction development.
- **Data Modeling:** Designed a robust `User` model equipped with data annotations (`[Required]`, `[StringLength]`, `[EmailAddress]`) to enforce validation rules at both code and database levels.

## 2. Security Measures
- **Anti-Forgery Protection:** All state-changing POST requests (such as profile edits) utilize `[ValidateAntiForgeryToken]` to defend against Cross-Site Request Forgery (CSRF) attacks.
- **Data Integrity:** Used hidden input fields for immutable attributes (`UserId`, `CreatedAt`, `PasswordHash`) to prevent tampering during form submission.

## 3. Challenges Encountered & Solutions
- **Challenge:** Initial design phase required a structured approach to context configuration without external GUI database explorer tools.
- **Solution:** Leveraged Entity Framework Core CLI tools and automated auto-seeding logic within the controller to streamline setup and immediate UI verification.