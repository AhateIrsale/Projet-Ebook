using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ebook_website.Models;

namespace ebook_website.Controllers
{
    public class UtilisateursController : Controller
    {
        private ebookEntities db = new ebookEntities();

        // GET: Utilisateurs
        public ActionResult Index(string SearchName)
        {
            if (Session["access"] != null)
            {
                return View(db.Utilisateur.Where(c => c.Email.Contains(SearchName)
                 || c.Nom.Contains(SearchName) || c.Prenom.Contains(SearchName) || SearchName == null).ToList());
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(string id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Utilisateur utilisateur = db.Utilisateur.Find(id);
                if (utilisateur == null)
                {
                    return HttpNotFound();
                }
                return View(utilisateur);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            if (Session["access"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Prenom,Email,Password")] Utilisateur utilisateur)
        {
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Utilisateur.Add(utilisateur);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(utilisateur);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Utilisateur utilisateur = db.Utilisateur.Find(id);
                if (utilisateur == null)
                {
                    return HttpNotFound();
                }
                return View(utilisateur);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nom,Prenom,Email,Password")] Utilisateur utilisateur)
        {
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(utilisateur).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(utilisateur);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(string email)
        {
            if (Session["access"] != null)
            {
                if (email == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Utilisateur utilisateur = db.Utilisateur.Find(email);
                if (utilisateur == null)
                {
                    return HttpNotFound();
                }
                return View(utilisateur);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string email)
        {
            if (Session["access"] != null)
            {
                Utilisateur utilisateur = db.Utilisateur.Find(email);
                db.Utilisateur.Remove(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
