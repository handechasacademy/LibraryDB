-- LibraryDB Full SQL Script

USE [LibraryDB];
GO

-- Tables

-- Book table
CREATE TABLE IF NOT EXISTS Book (
    BookID          INT             NOT NULL PRIMARY KEY,
    BookTitle       NVARCHAR(200)   NOT NULL,
    Author          NVARCHAR(200)   NOT NULL,
    Publisher       NVARCHAR(200)   NULL,
    YearPublished   INT             NULL,
    Category        NVARCHAR(100)   NULL
);

-- Member table
CREATE TABLE IF NOT EXISTS Member (
    MemberID        INT             NOT NULL PRIMARY KEY,
    MemberName      NVARCHAR(200)   NOT NULL,
    Address         NVARCHAR(300)   NULL,
    PhoneNumber     NVARCHAR(50)    NULL,
    Email           NVARCHAR(200)   NULL,
    MembershipDate  DATE            NOT NULL
);

-- Loan table
CREATE TABLE IF NOT EXISTS Loan (
    LoanID      INT         NOT NULL PRIMARY KEY,
    FkBookID    INT         NOT NULL,
    FkMemberID  INT         NOT NULL,
    LoanDate    DATE        NOT NULL,
    DueDate     DATE        NOT NULL,
    ReturnDate  DATE        NULL,
    CONSTRAINT FK_Loan_Book FOREIGN KEY (FkBookID) REFERENCES Book(BookID),
    CONSTRAINT FK_Loan_Member FOREIGN KEY (FkMemberID) REFERENCES Member(MemberID)
);

-- Initial Data

-- Books
INSERT INTO Book (BookID, BookTitle, Author, Publisher, YearPublished, Category)
VALUES
(1, 'The Fellowship of the Ring', 'J.R.R. Tolkien', 'Allen & Unwin', 1954, 'Fantasy'),
(2, 'The Two Towers', 'J.R.R. Tolkien', 'Allen & Unwin', 1954, 'Fantasy'),
(3, 'The Return of the King', 'J.R.R. Tolkien', 'Allen & Unwin', 1955, 'Fantasy'),
(4, 'The Return of the King', 'J.R.R. Tolkien', 'Allen & Unwin', 1955, 'Fantasy'),
(5, 'The Silmarillion', 'J.R.R. Tolkien', 'Allen & Unwin', 1977, 'Fantasy'),
(6, 'Unfinished Tales', 'J.R.R. Tolkien', 'Allen & Unwin', 1980, 'Fantasy'),
(7, 'The Children of Húrin', 'J.R.R. Tolkien', 'HarperCollins', 2007, 'Fantasy'),
(8, 'Beren and Lúthien', 'J.R.R. Tolkien', 'HarperCollins', 2017, 'Fantasy'),
(9, 'The Fall of Gondolin', 'J.R.R. Tolkien', 'HarperCollins', 2018, 'Fantasy');

-- Members
INSERT INTO Member (MemberID, MemberName, Address, PhoneNumber, Email, MembershipDate)
VALUES
(1, 'Frodo Baggins', 'Bag End, Hobbiton, The Shire', '000-000-0001', 'frodo@shire.me', '2025-01-01'),
(2, 'Samwise Gamgee', 'Number 3 Bagshot Row, Hobbiton', '000-000-0002', 'sam@shire.me', '2025-01-02'),
(3, 'Gandalf the Grey', 'Wandering Wizard, Middle-earth', '000-000-0003', 'gandalf@istari.org', '2025-01-03'),
(4, 'Aragorn Elessar', 'The Wild / Gondor', '000-000-0004', 'aragorn@gondor.gov', '2025-01-04'),
(5, 'Legolas Greenleaf', 'Woodland Realm, Mirkwood', '000-000-0005', 'legolas@mirkwood.el', '2025-01-05');

-- Loans
INSERT INTO Loan (LoanID, FkBookID, FkMemberID, LoanDate, DueDate, ReturnDate)
VALUES
(1, 1, 1, '2025-02-01', '2025-02-15', NULL),
(2, 2, 2, '2025-02-03', '2025-02-17', '2025-02-16'),
(3, 3, 4, '2025-02-05', '2025-02-19', NULL),
(4, 1, 3, '2025-02-10', '2025-02-24', NULL),
(5, 2, 5, '2025-02-12', '2025-02-26', NULL);

-- Example of a new loan
INSERT INTO Loan (FkBookID, FkMemberID, LoanDate, DueDate, ReturnDate)
VALUES (5, 1, '2026-01-02', '2026-01-16', NULL);

-- Example of a book returned
UPDATE Loan
SET ReturnDate = GETDATE()
WHERE LoanID = 4;

-- Views

-- Active Loans
CREATE OR ALTER VIEW View_ActiveLoans AS
SELECT 
    L.LoanID,
    M.MemberName,
    M.Email,
    B.BookTitle,
    L.LoanDate,
    L.DueDate
FROM Loan L
JOIN Member M ON L.FkMemberID = M.MemberID
JOIN Book B ON L.FkBookID = B.BookID
WHERE L.ReturnDate IS NULL;
GO

-- Book Status
CREATE OR ALTER VIEW View_BookStatus AS
SELECT 
    B.BookID,
    B.BookTitle,
    B.Author,
    B.Category,
    CASE 
        WHEN L.LoanID IS NOT NULL AND L.ReturnDate IS NULL THEN 'Loaned'
        ELSE 'Available'
    END AS Status
FROM Book B
LEFT JOIN Loan L ON B.BookID = L.FkBookID AND L.ReturnDate IS NULL;
GO

-- Stored Procedures

-- Register a New Loan
CREATE OR ALTER PROCEDURE sp_RegisterLoan
    @BookID INT,
    @MemberID INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Loan WHERE FkBookID = @BookID AND ReturnDate IS NULL)
        BEGIN
            PRINT 'Error: Book is already loaned out.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO Loan (FkBookID, FkMemberID, LoanDate, DueDate, ReturnDate)
        VALUES (@BookID, @MemberID, GETDATE(), DATEADD(day, 14, GETDATE()), NULL);

        COMMIT TRANSACTION;
        PRINT 'Loan registered successfully.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'An error occurred. Transaction rolled back.';
    END CATCH
END;
GO

-- Register a Book Return
CREATE OR ALTER PROCEDURE sp_RegisterReturn
    @LoanID INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Loan 
        SET ReturnDate = GETDATE() 
        WHERE LoanID = @LoanID;

        COMMIT TRANSACTION;
        PRINT 'Return registered successfully.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'Error during return. Transaction rolled back.';
    END CATCH
END;
GO

-- Search Books by Title or Author
CREATE OR ALTER PROCEDURE GetBooks
    @SearchText NVARCHAR(200)
AS
BEGIN
    SELECT
        BookID,
        BookTitle,
        Author,
        Publisher,
        YearPublished,
        Category
    FROM Book
    WHERE BookTitle LIKE '%' + @SearchText + '%'
       OR Author LIKE '%' + @SearchText + '%';
END;
GO

-- Check if a Book is Currently Loaned
CREATE OR ALTER PROCEDURE IsBookLoaned
    @BookID INT
AS
BEGIN
    SELECT COUNT(*) AS ActiveLoans
    FROM Loan
    WHERE FkBookID = @BookID
      AND ReturnDate IS NULL;
END;
GO
