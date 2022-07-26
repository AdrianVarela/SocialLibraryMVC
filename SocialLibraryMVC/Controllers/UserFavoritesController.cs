﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialLibraryMVC.Data;
using SocialLibraryMVC.Models;

namespace SocialLibraryMVC.Controllers
{
    public class UserFavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserFavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserFavorites
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserFavorites.Include(u => u.Books).Where(u => u.User_id == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserFavorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserFavorites == null)
            {
                return NotFound();
            }

            var userFavorites = await _context.UserFavorites
                .Include(u => u.Books)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavorites == null)
            {
                return NotFound();
            }

            return View(userFavorites);
        }

        // GET: UserFavorites/Create
        [Authorize]
        public async Task<IActionResult> Create([Bind("ISBN_13")] long ISBN_13)
        {
            var userFavoritesExists = await _context.UserFavorites.Where(f => f.User_id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync(f => f.ISBN_13 == ISBN_13);
            if(userFavoritesExists != null)
            {
                return RedirectToAction(nameof(Delete), ISBN_13);
            }
            UserFavorites userFavorites = new UserFavorites();
            userFavorites.User_id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            userFavorites.ISBN_13 = ISBN_13;

            if (ModelState.IsValid)
            {
                _context.Add(userFavorites);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NoContent();
            //return RedirectToAction(nameof(Index), nameof(BooksController));
            //return RedirectToAction(nameof(Create), route);
        }

        // GET: UserFavorites/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? ISBN_13, [Bind("inIndex")]bool inIndex = false)
        {
            if (ISBN_13 == null || _context.UserFavorites == null)
            {
                return NoContent();
            }

            var userFavorites = await _context.UserFavorites
                .Include(u => u.Books)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.ISBN_13 == ISBN_13);
            if (userFavorites == null)
            {
                return NotFound();
            }
            if (_context.UserFavorites == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserFavorites'  is null.");
            }
            //var userFavorites = await _context.UserFavorites.Where(f => f.User_id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync(f => f.ISBN_13 == ISBN_13);
            if (userFavorites != null)
            {
                _context.UserFavorites.Remove(userFavorites);
            }

            await _context.SaveChangesAsync();
            if (inIndex)
                return RedirectToAction(nameof(Index));
            return NoContent();

/*            if (id == null || _context.UserFavorites == null)
            {
                return NotFound();
            }

            var userFavorites = await _context.UserFavorites
                .Include(u => u.Books)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavorites == null)
            {
                return NotFound();
            }

            return View(userFavorites);*/
        }

/*        // POST: UserFavorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserFavorites == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserFavorites'  is null.");
            }
            var userFavorites = await _context.UserFavorites.FindAsync(id);
            if (userFavorites != null)
            {
                _context.UserFavorites.Remove(userFavorites);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool UserFavoritesExists(int id)
        {
          return (_context.UserFavorites?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> _Favorite([Bind("ISBN_13")] long ISBN_13)
        {
            string? user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
                return NoContent();
            ViewData["ISBN_13"] = ISBN_13;
            var userFavoritesExists = await _context.UserFavorites.Where(f => f.User_id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync(f => f.ISBN_13 == ISBN_13);

            if (userFavoritesExists != null)
            {
                return PartialView("_Unfavorite");
            }
            return PartialView("_Favorite");
        }
    }
}
