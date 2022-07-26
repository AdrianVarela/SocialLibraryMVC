using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialLibraryMVC.Data;
using SocialLibraryMVC.Models;

namespace SocialLibraryMVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context; 
        }


        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reviews.OrderByDescending(r => r.Id).Where(r => r.Id > _context.Reviews.OrderBy(r => r.Id).Last().Id - 10).Include(r => r.Books).Include(r => r.User);
            var users = _context.Users;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: Reviews/Create
        [Authorize]
        public async Task<IActionResult> Create(long? isbn_13)
        {
            var alreadyMadeReview = await _context.Reviews.Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.User_id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(alreadyMadeReview != null && alreadyMadeReview.Isbn_13 == isbn_13)
            {
                string redirect = nameof(Edit) + "?Id=" + alreadyMadeReview.Id;
                return RedirectToAction(nameof(Edit), alreadyMadeReview);
            }
            //ViewData["User_id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            if (isbn_13 == null || _context.Books == null)
            {
                return NotFound();
            }
            var reviews = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.ISBN_13 == isbn_13);
            if(reviews == null)
            {
                return NotFound();
            }
            ViewData["ISBN_13"] = isbn_13;
            ViewData["User_id"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Text_review,Rating,Isbn_13,User_id")] Review reviews)
        {
            //reviews.User_id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["User_id"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", reviews.User_id);
            return View(reviews);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }
            ViewData["User_id"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["ISBN_13"] = reviews.Isbn_13;
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User_id,Text_review,Rating,Isbn_13")] Review reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
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
            ViewData["User_id"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", reviews.User_id);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews != null)
            {
                _context.Reviews.Remove(reviews);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Reviews/_BookReviews
        // Partial View that should appear below the Details page of a particular book. Reviews shown should only be the reviews of the chosen book
        public async Task<IActionResult> _BookReviews(long isbn_13)
        {
            var applicationDbContext = _context.Reviews.Include(r => r.Books).Include(r => r.User).Where(b => b.Isbn_13 == isbn_13);
            return PartialView(await applicationDbContext.ToListAsync());
        }

        private bool ReviewsExists(int id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
