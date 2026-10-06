using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using home_work_54_lab_work.Models;
using Microsoft.AspNetCore.Identity;

namespace home_work_54_lab_work.Controllers
{
    public class BookController : Controller
    {
        private readonly TaskContext _context;
        private readonly UserManager<User> _userManager;

        public BookController(TaskContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b=>b.Comments).ThenInclude(c=>c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            
            ViewBag.UserId= _userManager.GetUserId(User);
            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        public IActionResult SearchAjax()
        {
            return View();
        }

        public async Task<IActionResult> SearchAjaxResult(string? keyWord)
        {
            List<Book> books = null;
            if(!string.IsNullOrEmpty(keyWord))
            {
                books = await _context.Books.Where(b => b.Name.Contains(keyWord)).ToListAsync();
            }
            return PartialView("_BookSearchResultPartial", books);
        }
        
        public async Task<IActionResult> TestSearch(string? keyWord)
        {
            List<Book> books = null;
            if(!string.IsNullOrEmpty(keyWord))
            {
                books = await _context.Books.Where(b => b.Name.Contains(keyWord)).ToListAsync();
            }
            return Json(books);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(int? bookId, int? userId, string? content)
        {
            Models.Comment comment = new Comment();
            if (!string.IsNullOrEmpty(content) && bookId.HasValue && userId.HasValue)
            {
                comment=new Comment()
                {
                    BookId=bookId.Value,
                    UserId=userId.Value,
                    Body=content
                };
                var result=await _context.AddAsync(comment);
                if (result.State == EntityState.Added)
                {
                    await _context.SaveChangesAsync();
                }
            }
            return Json(comment);
        }
        
        public async Task<IActionResult> GetComments(int bookId)
        {
            List<Comment> comments =
                await _context.Comments.Include(u => u.User).Where(c => c.BookId == bookId).ToListAsync();
            return Json(comments);
        }
        
    }
}
