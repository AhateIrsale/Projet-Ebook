using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Globalization;
using System.Security.Claims;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using ebook_website.Models;
using System.Data.Entity;

namespace ebook_website.Controllers
{
    public class ProfileController : Controller
    {
        private ebookEntities db = new ebookEntities();
        // GET: Profile

        public ActionResult EditProfile()
        {
            var Email = User.Identity.Name;
            var admin = db.Admin.Where(a => a.Email == Email).SingleOrDefault();
            Admin profil = new Admin();
            profil.Nom = admin.Nom;
            profil.Prenom = admin.Prenom;
            profil.Email = admin.Email;
            return View(profil);
        }


        [HttpPost]
        public ActionResult EditProfile(Admin admin)
        {
            var Email = User.Identity.Name;
            var currentUser = db.Admin.Where(a => a.Email == Email).SingleOrDefault();
            currentUser.Nom = admin.Nom;
            currentUser.Prenom = admin.Prenom;
            currentUser.Email = admin.Email;
            db.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View(admin);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "Nom,Prenom,Email,Password")] Admin admin)
        {
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(admin);
            }
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult EditPassword()
        {
            if (Session["access"] != null)
            {
                 return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }
    }
}