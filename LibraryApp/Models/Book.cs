using System;
using System.Collections.Generic;

namespace LibraryApp.Models;

public partial class Book
{
    public int BookID { get; set; }

    public string BookTitle { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Publisher { get; set; }

    public int? YearPublished { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
