using System.Collections.Generic;

namespace LibraryApp.Models
{
    public class Genre
    {
        public string Name { get; set; }
        public ICollection<GenreEntry> GenreEntries { get; set; }
    }
}