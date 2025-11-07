using Microsoft.AspNet.Identity;
using PersonalLibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PersonalLibraryManager.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;

        public BooksController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: Books
        public ActionResult Index(string status = "", string search = "")
        {
            var userId = User.Identity.GetUserId();

            // Get all books for user
            var query = _context.Books.Where(b => b.UserId == userId);

            // Filter by status if provided
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<ReadingStatus>(status, out var statusEnum))
                {
                    query = query.Where(b => b.Status == statusEnum);
                }
            }

            // Search by title or author
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            var books = query.OrderByDescending(b => b.DateAdded).ToList();

            // Calculate statistics
            ViewBag.TotalBooks = _context.Books.Count(b => b.UserId == userId);
            ViewBag.ToReadCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.ToRead);
            ViewBag.ReadingCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.Reading);
            ViewBag.CompletedCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.Completed);
            ViewBag.CurrentFilter = status;
            ViewBag.CurrentSearch = search;

            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            var viewModel = new BookFormViewModel
            {
                Heading = "Add New Book"
            };

            return View("BookForm", viewModel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Heading = "Add New Book";
                return View("BookForm", viewModel);
            }

            var book = new Book
            {
                Title = viewModel.Title,
                Author = viewModel.Author,
                ISBN = viewModel.ISBN,
                Rating = viewModel.Rating,
                Review = viewModel.Review,
                Status = viewModel.Status,
                DateAdded = DateTime.Now,
                DateCompleted = viewModel.DateCompleted,
                UserId = User.Identity.GetUserId()
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            var viewModel = new BookFormViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Rating = book.Rating,
                Review = book.Review,
                Status = book.Status,
                DateCompleted = book.DateCompleted,
                Heading = "Edit Book"
            };

            return View("BookForm", viewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Heading = "Edit Book";
                return View("BookForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var bookInDb = _context.Books
                .SingleOrDefault(b => b.Id == viewModel.Id && b.UserId == userId);

            if (bookInDb == null)
            {
                return HttpNotFound();
            }

            bookInDb.Title = viewModel.Title;
            bookInDb.Author = viewModel.Author;
            bookInDb.ISBN = viewModel.ISBN;
            bookInDb.Rating = viewModel.Rating;
            bookInDb.Review = viewModel.Review;
            bookInDb.Status = viewModel.Status;
            bookInDb.DateCompleted = viewModel.DateCompleted;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Books/MarkAsCompleted/5
        [HttpPost]
        public ActionResult MarkAsCompleted(int id)
        {
            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            book.Status = ReadingStatus.Completed;
            book.DateCompleted = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Books/UpdateStatus/5
        [HttpPost]
        public ActionResult UpdateStatus(int id, ReadingStatus status)
        {
            var userId = User.Identity.GetUserId();
            var book = _context.Books
                .SingleOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return HttpNotFound();
            }

            book.Status = status;

            if (status == ReadingStatus.Completed && !book.DateCompleted.HasValue)
            {
                book.DateCompleted = DateTime.Now;
            }
            else if (status != ReadingStatus.Completed)
            {
                book.DateCompleted = null;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Books/Dashboard
        public ActionResult Dashboard()
        {
            var userId = User.Identity.GetUserId();

            // Calculate statistics
            ViewBag.TotalBooks = _context.Books.Count(b => b.UserId == userId);
            ViewBag.ToReadCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.ToRead);
            ViewBag.ReadingCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.Reading);
            ViewBag.CompletedCount = _context.Books.Count(b => b.UserId == userId && b.Status == ReadingStatus.Completed);

            // Get recent books (last 5)
            ViewBag.RecentBooks = _context.Books
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.DateAdded)
                .Take(5)
                .ToList();

            // Get top rated books
            ViewBag.TopRatedBooks = _context.Books
                .Where(b => b.UserId == userId && b.Rating.HasValue)
                .OrderByDescending(b => b.Rating)
                .ThenBy(b => b.Title)
                .Take(5)
                .ToList();

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}