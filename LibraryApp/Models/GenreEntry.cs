namespace LibraryApp.Models
{
    public class GenreEntry
    {
        public int BookId { get; set; }
        public string GenreName { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }
}