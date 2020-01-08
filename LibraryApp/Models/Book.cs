using System.Collections.Generic;
using System.Net;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<GenreEntry> Genres { get; set; } = new List<GenreEntry>();
    }
}