using System;
using System.Collections.Generic;

namespace LibraryApp.Models;

public partial class Member
{
    public int MemberID { get; set; }

    public string MemberName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateOnly MembershipDate { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
