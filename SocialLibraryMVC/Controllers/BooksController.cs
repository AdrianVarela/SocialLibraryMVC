using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialLibraryMVC.Data;
using SocialLibraryMVC.Models;

namespace SocialLibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var applicationDbContext = _context.Books.Include(b => b.Authors);
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        applicationDbContext = _context.Books.OrderByDescending(b => b.Title).Where(b => b.Title.Contains(searchString)).Include(b => b.Authors);
                        break;
                    case "Date":
                        applicationDbContext = _context.Books.OrderBy(b => b.Authors).Where(b => b.Title.Contains(searchString)).Include(b => b.Authors);
                        break;
                    case "date_desc":
                        applicationDbContext = _context.Books.OrderByDescending(b => b.Authors).Where(b => b.Title.Contains(searchString)).Include(b => b.Authors);
                        break;
                    default:
                        applicationDbContext = _context.Books.OrderBy(b => b.Title).Where(b => b.Title.Contains(searchString)).Include(b => b.Authors);
                        break;
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    applicationDbContext = _context.Books.OrderByDescending(b => b.Title).Include(b => b.Authors);
                    break;
                case "Date":
                    applicationDbContext = _context.Books.OrderBy(b => b.Authors).Include(b => b.Authors);
                    break;
                case "date_desc":
                    applicationDbContext = _context.Books.OrderByDescending(b => b.Authors).Include(b => b.Authors);
                    break;
                default:
                    applicationDbContext = _context.Books.OrderBy(b => b.Title).Include(b => b.Authors);
                    break;
            }
            foreach (var book in applicationDbContext)
            {
                var reviews = _context.Reviews.Where(r => r.Isbn_13 == book.ISBN_13);
                int sumRating = 0;
                foreach (var review in reviews)
                {
                    sumRating += review.Rating;
                }
                ViewData["AverageRating"+book.ISBN_13] = (reviews.Count()>0)?((double)sumRating / reviews.Count()) : 0;
                ViewData["CountRating"+book.ISBN_13] = reviews.Count();
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.ISBN_13 == id);
            if (books == null)
            {
                return NotFound();
            }

            ViewBag.Cover = null;
            if (books.Cover != null)
                ViewBag.Cover = books.Cover;

            return View(books);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("Title,Description,AuthorId,Genre,PublishYear,ISBN_10,ISBN_13,Cover")] Books books
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,Description,AuthorId,Genre,PublishYear,ISBN_10,ISBN_13,Cover")] Book books)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        books.Cover = dataStream.ToArray();
                    }
                }
                _context.Add(books);
                await _context.SaveChangesAsync();
                TempData["success"] = "Book added successfully!!"; //Toastr
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", books.AuthorId);
            return View(books);

        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", books.AuthorId);
            // This viewbag is for the image display
            ViewBag.Cover = null;
            if(books.Cover != null)
                ViewBag.Cover = books.Cover;
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("Title,Description,AuthorId,Genre,PublishYear,ISBN_10,ISBN_13")] Book books)
        {
            if (id != books.ISBN_13)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // If there was an image uploaded, then put that image onto the books.Cover property
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        books.Cover = dataStream.ToArray();
                    }
                }
                // if there is an edit and no file is uploaded (meaning no new cover), keep the old cover
                else
                {
                    Book? oldBook = FindBook(books.ISBN_13);
                    if(oldBook != null)
                        books.Cover = oldBook.Cover;
                }
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Book edited successfully!!"; //Toastr
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.ISBN_13))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", books.AuthorId);
            return View(books);
        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.ISBN_13 == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Book deleted successfully!!"; //Toastr
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(long id)
        {
          return (_context.Books?.Any(e => e.ISBN_13 == id)).GetValueOrDefault();
        }

        private Book? FindBook(long isbn13)
        {
            Book? book = _context.Books.Find(isbn13);
            _context.ChangeTracker.Clear();
            return book;
        }
    }
}
