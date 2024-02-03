using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNet.Identity;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private LibraryManagementSystemContext db = new LibraryManagementSystemContext();

        // GET: Books
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(db.Books.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Borrow/5
        public ActionResult Borrow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            if (book.Copies <= book.BorrowedCopies)
            {
                ViewBag.ErrorMessage = "Sorry, book is unavailable.";
                return View("Message");
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            Borrow borrow = new Borrow
            {
                UserID = userId,
                BookID = book.BookID,
                BorrowDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(14),

            };


            book.BorrowedCopies++;
            book.Availability = (book.Copies > book.BorrowedCopies);

            // Save changes
            db.Borrows.Add(borrow);
            db.SaveChanges();

            return RedirectToAction("Index", "Borrows");
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,Genre,Author,Description,Copies,BorrowedCopies,Availability")] Book book)
        {
            if (ModelState.IsValid)
            {
                if(book.Copies > book.BorrowedCopies)
                {
                    book.Availability = true;
                }
                else
                {
                    book.Availability = false;
                }

                book.BorrowedCopies = 0;

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,Genre,Author,Description,Copies,BorrowedCopies,Availability")] Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.Copies > book.BorrowedCopies)
                {
                    book.Availability = true;
                }
                else
                {
                    book.Availability = false;
                }
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
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
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}
