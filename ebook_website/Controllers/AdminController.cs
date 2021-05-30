using ebook_website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ebook_website.Controllers
{
    public class AdminController : Controller
    {
        private ebookEntities db = new ebookEntities();
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            bool isValid = db.Admin.Any(a => a.Email == admin.Email && a.Password == admin.Password);
            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(admin.Email, false);
                Session["access"] = "Login with success";
                return RedirectToAction("Index", "Categories");
            }
            ModelState.AddModelError("", "E-mail ou Mot de passe non valide");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return View("Login");
        }
    }
}