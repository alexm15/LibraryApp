using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.ViewModels;
using Microsoft.EntityFrameworkCore.Query;

namespace LibraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryAppContext _context;

        public BooksController(LibraryAppContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await GetAllBooksWithDetails()
                .AsNoTracking()
                .ToListAsync();


            return View(books);
        }

        private IQueryable<Book> GetAllBooksWithDetails()
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Genres);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await GetAllBooksWithDetails()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            var createModel = await NewEditBookViewModel();
            return View(createModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditBookViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var book = new Book
            {
                Title = model.Title,
                BookAuthors =
                    new List<BookAuthor>(model.SelectedAuthors.Select(a => new BookAuthor
                        {AuthorId = a, BookId = model.Id})),
                Genres = new List<GenreEntry>(model.SelectedGenres.Select(g => new GenreEntry
                    {GenreName = g, BookId = model.Id}))
            };
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await GetAllBooksWithDetails()
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
            return View(await PopulateEditBookViewModel(book));
        }

        private async Task<EditBookViewModel> PopulateEditBookViewModel(Book book)
        {
            var authors = await _context.Authors
                .Select(a => new SelectListItem($"{a.FirstName} {a.LastName}", a.Id.ToString()))
                .ToListAsync();
            var model = new EditBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AvailableAuthors = authors,
                SelectedAuthors = book.BookAuthors.Select(b => b.AuthorId).ToList(),
                AvailableGenres = await _context.Genres.Select(g => new SelectListItem(g.Name, g.Name)).ToListAsync(),
                SelectedGenres = book.Genres.Select(b => b.GenreName).ToList()
            };
            return model;
        }

        private async Task<EditBookViewModel> NewEditBookViewModel()
        {
            var authors = await _context.Authors
                .Select(a => new SelectListItem($"{a.FirstName} {a.LastName}", a.Id.ToString()))
                .ToListAsync();
            var model = new EditBookViewModel
            {
                AvailableAuthors = authors,
                AvailableGenres = await _context.Genres.Select(g => new SelectListItem(g.Name, g.Name)).ToListAsync(),
            };
            return model;
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            var book = await GetAllBooksWithDetails().FirstOrDefaultAsync(b => b.Id == model.Id);
            if (book == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    book.Title = model.Title;
                    UpdateGenres(model.SelectedGenres, book);
                    UpdateAuthors(model.SelectedAuthors, book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (DbUpdateException e)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists, " +
                                             "see your system administrator."); 
                
            }
            return View(await PopulateEditBookViewModel(book));
        }

        private void UpdateAuthors(IEnumerable<int> authorIds, Book book)
        {
            var selectedAuthors = new HashSet<int>(authorIds);
            var bookAuthors = new HashSet<int>(book.BookAuthors.Select(b => b.AuthorId));
            foreach (var author in _context.Authors)
            {
                if (selectedAuthors.Contains(author.Id))
                {
                    if (!bookAuthors.Contains(author.Id))
                    {
                        book.BookAuthors.Add(new BookAuthor {AuthorId = author.Id, BookId = book.Id});
                    }
                }
                else
                {
                    if (!bookAuthors.Contains(author.Id)) continue;
                    var bookAuthor = book.BookAuthors.FirstOrDefault(b => b.AuthorId.Equals(author.Id));
                    book.BookAuthors.Remove(bookAuthor);
                }
            }
        }

        private void UpdateGenres(IEnumerable<string> genreNames, Book book)
        {
            var selectedGenres = new HashSet<string>(genreNames);
            var bookGenres = new HashSet<string>(book.Genres.Select(b => b.GenreName));
            foreach (var genre in _context.Genres)
            {
                if (selectedGenres.Contains(genre.Name))
                {
                    if (!bookGenres.Contains(genre.Name))
                    {
                        book.Genres.Add(new GenreEntry {GenreName = genre.Name, BookId = book.Id});
                    }
                }
                else
                {
                    if (!bookGenres.Contains(genre.Name)) continue;
                    var bookGenre = book.Genres.FirstOrDefault(b => b.GenreName.Equals(genre.Name));
                    book.Genres.Remove(bookGenre);
                }
            }
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await GetAllBooksWithDetails()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}