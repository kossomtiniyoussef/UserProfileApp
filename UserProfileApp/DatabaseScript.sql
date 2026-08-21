CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Users" (
    "UserId" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "Username" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Bio" TEXT NOT NULL,
    "ProfilePictureUrl" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260821095036_InitialCreate', '10.0.11');

COMMIT;

