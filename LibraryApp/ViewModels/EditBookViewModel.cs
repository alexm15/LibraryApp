using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.ViewModels
{
    public class EditBookViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public IEnumerable<SelectListItem> AvailableAuthors { get; set; } = new List<SelectListItem>();

        public IEnumerable<int> SelectedAuthors { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> AvailableGenres { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> SelectedGenres { get; set; } = new List<string>();
    }
}