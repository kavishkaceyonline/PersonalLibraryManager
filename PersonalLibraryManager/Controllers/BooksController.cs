using Microsoft.AspNet.Identity;
using PersonalLibraryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Books/Create
        public ActionResult Create()
        {
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