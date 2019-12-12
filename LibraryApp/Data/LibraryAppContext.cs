using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data
{
    public class LibraryAppContext : DbContext
    {
        public LibraryAppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasKey(g => g.Name);
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Genres);
            modelBuilder.Entity<GenreEntry>().HasKey(g => new {g.GenreName, g.BookId});
            modelBuilder.Entity<BookAuthor>().HasKey(b => new {b.BookId, b.AuthorId});
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<LibraryLoan> LibraryLoans { get; set; }
        public DbSet<GenreEntry> GenreEntries { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
    }
}