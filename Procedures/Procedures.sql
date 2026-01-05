GO
CREATE PROCEDURE GetBooks
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
END
GO

CREATE PROCEDURE IsBookLoaned
    @BookID INT
AS
BEGIN
    SELECT COUNT(*) AS ActiveLoans
    FROM Loan
    WHERE FkBookID = @BookID
      AND ReturnDate IS NULL;
END
GO
