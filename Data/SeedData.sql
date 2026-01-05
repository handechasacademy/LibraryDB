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
