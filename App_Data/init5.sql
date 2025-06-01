-- Create users table
CREATE TABLE "users" (
    "Id" INTEGER NOT NULL CONSTRAINT "Id" PRIMARY KEY AUTOINCREMENT,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "Phone" TEXT NULL,
    "PostalCode" TEXT NULL,
    "Address" TEXT NULL,
    "House" TEXT NULL,
    "Stairs" TEXT NULL,
    "Door" TEXT NULL,
    "Bell" TEXT NULL,
    "Comment" TEXT NULL
); 