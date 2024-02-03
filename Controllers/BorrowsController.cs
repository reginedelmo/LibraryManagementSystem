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

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BorrowsController : Controller
    {
        private LibraryManagementSystemContext db = new LibraryManagementSystemContext();

        // GET: Borrows
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var borrows = db.Borrows.Include(b => b.Book).Include(b => b.User);
                return View(borrows.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Borrows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }


        public ActionResult Return(int? id)
        {
            var borrow = db.Borrows.Find(id);

            if (borrow != null)
            {
                // Check if return date has passed
                if (DateTime.Now > borrow.ReturnDate)
                {
                    ViewBag.WarningMessage = "You have returned the book after the Return Date. Please return it on time.";
                }

                // Update book availability
                var book = db.Books.Find(borrow.BookID);
                if (book != null)
                {
                    book.BorrowedCopies--;
                    book.Availability = (book.Copies > book.BorrowedCopies);
                }

                db.Borrows.Remove(borrow);
                db.SaveChanges();

                return RedirectToAction("Index", "Borrows");
            }
            else
            {
                ViewBag.ErrorMessage = "Borrow is not existing.";
            }

            // Return the view with the updated ViewBag messages
            return View("Message");
        }

        // GET: Borrows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", borrow.BookID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", borrow.UserID);
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BorrowID,UserID,BookID,BorrowDate,ReturnDate")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", borrow.BookID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", borrow.UserID);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrow borrow = db.Borrows.Find(id);
            db.Borrows.Remove(borrow);
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
