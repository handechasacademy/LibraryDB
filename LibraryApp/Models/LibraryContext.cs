using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LibraryApp.Models;

namespace LibraryApp.Models;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Loan> Loans { get; set; }
    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("LibraryDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.Property(e => e.BookID).ValueGeneratedNever();
            entity.Property(e => e.Author).HasMaxLength(200);
            entity.Property(e => e.BookTitle).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Publisher).HasMaxLength(200);
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("Loan");

            entity.Property(e => e.LoanID).ValueGeneratedNever();

            entity.HasOne(d => d.FkBook).WithMany(p => p.Loans)
                .HasForeignKey(d => d.FkBookID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loan_Book");

            entity.HasOne(d => d.FkMember).WithMany(p => p.Loans)
                .HasForeignKey(d => d.FkMemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loan_Member");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberID).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.MemberName).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}