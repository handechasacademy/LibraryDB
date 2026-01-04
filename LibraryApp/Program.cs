using LibraryApp.Models;
using LibraryApp.Repositories;
using LibraryApp.UI;

class Program
{
    static void Main(string[] args)
    {
        using var context = new LibraryContext();
        var repository = new LibraryRepository(context);
        var menu = new MenuSystem(repository);

        try
        {
            menu.Run();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nAn error occurred: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}