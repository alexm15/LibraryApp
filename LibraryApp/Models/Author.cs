using System.Collections.Generic;

namespace LibraryApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<BookAuthor> BooksWritten { get; set; }
    }
}