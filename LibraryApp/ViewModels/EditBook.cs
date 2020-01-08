using System.Collections.Generic;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.ViewModels
{
    public class EditBook
    {
        public Book Book { get; set; }
        public IList<SelectListItem> AllAuthors { get; set; }
        public ICollection<string> BookAuthors { get; set; }

    }
}