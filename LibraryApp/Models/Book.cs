using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<GenreEntry> Genres { get; set; } = new List<GenreEntry>();

        public override string ToString()
        {
            var output = $"{Id} {Title} ";
            output = BookAuthors.Aggregate(output, (current, author) => current + $"{author.Author.FirstName} {author.Author.LastName} ");
            return Genres.Aggregate(output, (current, genre) => current + $"{genre.Genre.Name} ");
        }
    }
}