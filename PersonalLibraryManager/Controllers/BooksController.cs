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
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var books = _context.Books
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.DateAdded)
                .ToList();

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