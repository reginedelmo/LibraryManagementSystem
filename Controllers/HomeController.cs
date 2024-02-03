using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryManagementSystemContext _dbContext = new LibraryManagementSystemContext();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                //get hash to check
                var source = user.Password;
                using (var md5Hash = MD5.Create())
                {
                    var sourceBytes = Encoding.UTF8.GetBytes(source);
                    var hashBytes = md5Hash.ComputeHash(sourceBytes);
                    var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                    user.Password = hash;
                }

                var data = _dbContext.Users.Where(s => s.Username.ToLower().Equals(user.Username.ToLower()) && s.Password.Equals(user.Password)).ToList();

                foreach (var List in data)
                {
                    var userUsername = user.Username;
                    var userPassword = user.Password;

                    Session["UserID"] = List.UserID.ToString();
                    Session["UserType"] = List.UserType.ToString();

                }

                bool IsValidUser = _dbContext.Users
                    .Any(u => u.Username.ToLower() == user
                    .Username.ToLower() && u.Password == user.Password);

                if (IsValidUser || data != null && data.Count == 1)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User registerUser)
        {
            if (ModelState.IsValid)
            {
                //hash the passord before add to DB
                var source = registerUser.Password;

                // Creates an instance of the default implementation of the MD5 hash algorithm.
                using (var md5Hash = MD5.Create())
                {
                    // Byte array representation of source string
                    var sourceBytes = Encoding.UTF8.GetBytes(source);

                    // Generate hash value(Byte Array) for input data
                    var hashBytes = md5Hash.ComputeHash(sourceBytes);

                    // Convert hash byte array to string
                    var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                    // Output the MD5 hash
                    registerUser.Password = hash;
                }
                registerUser.UserType = "Member";

                _dbContext.Users.Add(registerUser);
                _dbContext.SaveChanges();
                return RedirectToAction("Login");

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}