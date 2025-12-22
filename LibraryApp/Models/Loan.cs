using System;
using System.Collections.Generic;

namespace LibraryApp.Models;

public partial class Loan
{
    public int LoanID { get; set; }

    public int FkBookID { get; set; }

    public int FkMemberID { get; set; }

    public DateOnly LoanDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public virtual Book FkBook { get; set; } = null!;

    public virtual Member FkMember { get; set; } = null!;
}
