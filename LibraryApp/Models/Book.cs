using System.Collections.Generic;
using System.Net;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public ICollection<GenreEntry> Genres { get; set; }
    }
}