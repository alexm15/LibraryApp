using System.Collections.Generic;
using System.Linq;
using LibraryApp.Models;

namespace LibraryApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(LibraryAppContext context)
        {
            context.Database.EnsureCreated();

            if (context.Books.Any()) return;

            var authors = new[]
            {
                new Author {FirstName = "J.K", LastName = "Rowling"},
                new Author {FirstName = "Bob", LastName = "Martin"},
                new Author {FirstName = "Martin", LastName = "Fowler"},
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();

            var genres = new[]
            {
                new Genre {Name = "Fiction"},
                new Genre {Name = "Non-Fiction"},
                new Genre {Name = "Development & Tech"},
                new Genre {Name = "Fantasy"}
            };
            context.Genres.AddRange(genres);
            context.SaveChanges();

            var books = new[]
            {
                new Book {Title = "Harry Potter and the Chamber of secrets"},
                new Book {Title = "Clean Code"},
                new Book {Title = "Refactorings"},
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            var genreEntries = new[]
            {
                CreateGenreEntry("Harry Potter and the Chamber of secrets", "Fiction", books, genres),
                CreateGenreEntry("Harry Potter and the Chamber of secrets", "Fantasy", books, genres),
                CreateGenreEntry("Clean Code", "Non-Fiction", books, genres),
                CreateGenreEntry("Clean Code", "Development & Tech", books, genres),
                CreateGenreEntry("Refactorings", "Non-Fiction", books, genres),
                CreateGenreEntry("Refactorings", "Development & Tech", books, genres),

            };
            context.GenreEntries.AddRange(genreEntries);
            context.SaveChanges();

            var bookAuthors = new[]
            {
                CreateBookAuthor("Clean Code", "Martin", authors, books),
                CreateBookAuthor("Harry Potter and the Chamber of secrets", "Rowling", authors, books),
                CreateBookAuthor("Refactorings", "Fowler", authors, books),
            };
            context.BookAuthors.AddRange(bookAuthors);
            context.SaveChanges();
        }

        private static BookAuthor CreateBookAuthor(string bookTitle, string authorLastName, IEnumerable<Author> authors,
            IEnumerable<Book> books)
        {
            return new BookAuthor
            {
                AuthorId = authors.Single(a => a.LastName == authorLastName).Id,
                BookId = books.Single(b => b.Title == bookTitle).Id
            };
        }

        private static GenreEntry CreateGenreEntry(string bookTitle, string genre, IEnumerable<Book> books, IEnumerable<Genre> genres)
        {
            return new GenreEntry
            {
                BookId = books.Single(b => b.Title == bookTitle).Id,
                GenreName = genres.Single(g => g.Name == genre).Name
            };
        }
    }

    
}