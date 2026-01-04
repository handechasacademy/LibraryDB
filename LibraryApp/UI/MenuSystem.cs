using LibraryApp.Exceptions;
using LibraryApp.Models;
using LibraryApp.Repositories;

namespace LibraryApp.UI
{
    public class MenuSystem
    {
        private readonly LibraryRepository _repo;

        public MenuSystem(LibraryRepository repo)
        {
            _repo = repo;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== LIBRARY MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Register New Book");
                Console.WriteLine("2. Register New Member");
                Console.WriteLine("3. Register Loan");
                Console.WriteLine("4. Register Return");
                Console.WriteLine("5. View Active Loans");
                Console.WriteLine("6. Search Books");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1": AddBook(); break;
                    case "2": AddMember(); break;
                    case "3": RegisterLoan(); break;
                    case "4": RegisterReturn(); break;
                    case "5": ViewActiveLoans(); break;
                    case "6": SearchBooks(); break;
                    case "0": running = false; break;
                    default: Pause("Invalid choice."); break;
                }
            }
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Warn("Please enter a valid number.");
            }
        }

        private int ReadYear(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int year))
                {
                    if (year >= 1000 && year <= 9999)
                        return year;

                    Warn("Year must be a 4-digit number.");
                }
                else
                {
                    Warn("Please enter a valid number for the year.");
                }
            }
        }

        private string ReadRequired(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Warn("This field cannot be empty.");
            }
        }

        private void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void Pause(string message)
        {
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private void AddBook()
        {
            try
            {
                string title = ReadRequired("Enter Title: ");
                string author = ReadRequired("Enter Author: ");
                string publisher = ReadRequired("Enter Publisher: ");
                string category = ReadRequired("Enter Category: ");
                int year = ReadYear("Enter Publishment Year (YYYY): ");

                var book = new Book
                {
                    BookTitle = title,
                    Author = author,
                    Publisher = publisher,
                    Category = category,
                    YearPublished = year
                };

                _repo.AddBook(book);
                Pause("Book added successfully!");
            }
            catch (LibraryApp.Exceptions.LibraryException ex)
            {
                Pause(ex.Message);
            }
        }



        private void AddMember()
        {
            try
            {
                var member = new Member
                {
                    MemberID = ReadInt("Member ID: "),
                    MemberName = ReadRequired("Name: "),
                    Address = Console.ReadLine(),
                    Email = Console.ReadLine(),
                    PhoneNumber = Console.ReadLine(),
                    MembershipDate = DateOnly.FromDateTime(DateTime.Now)
                };

                _repo.AddMember(member);
                Pause("Member registered successfully!");
            }
            catch (LibraryException ex)
            {
                Pause(ex.Message);
            }
        }

        private void RegisterLoan()
        {
            try
            {
                int loanId = ReadInt("Loan ID: ");
                int bookId = ReadInt("Book ID: ");
                int memberId = ReadInt("Member ID: ");

                _repo.RegisterLoan(loanId, bookId, memberId);
                Pause("Loan registered successfully!");
            }
            catch (LibraryException ex)
            {
                Pause(ex.Message);
            }
        }

        private void RegisterReturn()
        {
            try
            {
                int loanId = ReadInt("Loan ID: ");
                _repo.RegisterReturn(loanId);
                Pause("Return processed!");
            }
            catch (LibraryException ex)
            {
                Pause(ex.Message);
            }
        }

        private void ViewActiveLoans()
        {
            var loans = _repo.GetActiveLoans();

            Console.WriteLine("\n--- ACTIVE LOANS ---");
            foreach (var l in loans)
            {
                Console.WriteLine(
                    $"Loan {l.LoanID}: {l.FkMember.MemberName} → {l.FkBook.BookTitle}");
            }

            Pause("");
        }

        private void SearchBooks()
        {
            Console.Write("Search: ");
            var q = Console.ReadLine();

            var results = _repo.SearchBooks(q);

            foreach (var b in results)
                Console.WriteLine($"[{b.BookID}] {b.BookTitle} by {b.Author}");

            Pause("");
        }
    }
}
