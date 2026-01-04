using LibraryApp.Exceptions;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repositories
{
    public class LibraryRepository
    {
        private readonly LibraryContext _context;

        public LibraryRepository(LibraryContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            if (_context.Books.Any(b => b.BookID == book.BookID))
                throw new DuplicateException($"Book with ID {book.BookID} already exists.");

            _context.Books.Add(book);
            _context.SaveChanges();
        }


        public List<Book> SearchBooks(string query)
        {
            return _context.Books
                .Where(b => b.BookTitle.Contains(query) || b.Author.Contains(query))
                .ToList();
        }

        public void AddMember(Member member)
        {
            if (_context.Members.Any(m => m.MemberID == member.MemberID))
                throw new DuplicateException($"Member with ID {member.MemberID} already exists.");

            _context.Members.Add(member);
            _context.SaveChanges();
        }



        public void RegisterLoan(int loanId, int bookId, int memberId)
        {
            if (_context.Loans.Any(l => l.LoanID == loanId))
                throw new DuplicateException(
                    $"Loan with ID {loanId} already exists.");

            var book = _context.Books.Find(bookId);
            if (book == null)
                throw new NotFoundException("Book not found.");

            var member = _context.Members.Find(memberId);
            if (member == null)
                throw new NotFoundException("Member not found.");

            bool isLoaned = _context.Loans
                .Any(l => l.FkBookID == bookId && l.ReturnDate == null);

            if (isLoaned)
                throw new RuleException("This book is already loaned.");

            var loan = new Loan
            {
                LoanID = loanId,
                FkBookID = bookId,
                FkMemberID = memberId,
                LoanDate = DateOnly.FromDateTime(DateTime.Now),
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14))
            };

            _context.Loans.Add(loan);
            _context.SaveChanges();
        }

        public void RegisterReturn(int loanId)
        {
            var loan = _context.Loans.Find(loanId);

            if (loan == null)
                throw new NotFoundException("Loan not found.");

            if (loan.ReturnDate != null)
                throw new RuleException("This loan has already been returned.");

            loan.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            _context.SaveChanges();
        }

        public List<Loan> GetActiveLoans()
        {
            return _context.Loans
                .Include(l => l.FkBook)
                .Include(l => l.FkMember)
                .Where(l => l.ReturnDate == null)
                .ToList();
        }
    }
}
