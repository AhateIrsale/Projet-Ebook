using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ebook_website.Models;

namespace ebook_website.Controllers
{
    public class LivresController : Controller
    {
        private ebookEntities db = new ebookEntities();

        // GET: Admin/Livres
        public ActionResult Index(string SearchName)
        {
            return View(db.Livre.Where(c => c.Titre.Contains(SearchName)
          || c.Auteur.Contains(SearchName) || c.Categorie.Nom_Categorie.Contains(SearchName)
            || c.Description.Contains(SearchName) || SearchName == null).ToList());
        }
        //public ActionResult Index(DateTime Searchdate)
        //{
        //    return View(db.Livre.Where(c =>(c.Date_Edition.Equals(Searchdate))|| c.Date_Publication.Equals(Searchdate) 
        //    || Searchdate == null).ToList());
        //}

        // GET: Admin/Livres/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Livre livre = db.Livre.Find(id);
                Session["imgPath"] = livre.Image;
                Session["pdfPath"] = livre.PDF;
                if (livre == null)
                {
                    return HttpNotFound();
                }
                return View(livre);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Admin/Livres/Create
        public ActionResult Create()
        {
            if (Session["access"] != null)
            {
                ViewBag.ID_Categorie = new SelectList(db.Categorie, "ID_Categorie", "Nom_Categorie");
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Admin/Livres/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Livre livre, HttpPostedFileBase upload, HttpPostedFileBase file)
        {
            if (Session["access"] != null)
            {
                if (file == null)
                {
                    ModelState.AddModelError("Erreur", "S'il vous plait selectionnez un fichier Doc/Pdf seulement");
                    return View(livre);
                }
                if (!(file.ContentType == "application/msword" || file.ContentType == "application/pdf"))
                {
                    ModelState.AddModelError("Erreur", "S'il vous plait selectionnez seulement fichier Doc ou Pdf !");
                    return View(livre);
                }
                if (ModelState.IsValid)
                {
                    string pat = Path.Combine(Server.MapPath("~/Uploads/Livres/PDF/"), file.FileName);
                    file.SaveAs(pat);
                    livre.PDF = file.FileName;


                    //Upload image 
                    string filename = Path.GetFileName(upload.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(upload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/Livres/"), _filename);
                    livre.Image = "~/Uploads/Livres/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (upload.ContentLength <= 1000000)
                        {
                            db.Livre.Add(livre);
                            if (db.SaveChanges() > 0)
                            {
                                upload.SaveAs(path);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.ID_Categorie = new SelectList(db.Categorie, "ID_Categorie", "Nom_Categorie", livre.ID_Categorie);
                return View(livre);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Admin/Livres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Livre livre = db.Livre.Find(id);
                Session["imgPath"] = livre.Image;
                if (livre == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID_Categorie = new SelectList(db.Categorie, "ID_Categorie", "Nom_Categorie", livre.ID_Categorie);
                return View(livre);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Admin/Livres/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Livre,Titre,Auteur,Description,Date_Publication,Date_Edition,ID_Categorie,Image,PDF")] Livre livre, HttpPostedFileBase upload)
        {
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(livre).State = EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                    if (upload != null)
                    {
                        string filename = Path.GetFileName(upload.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                        string extension = Path.GetExtension(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads/Livres"), _filename);
                        livre.Image = "~/Uploads/Livres/" + _filename;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (upload.ContentLength <= 1000000)
                            {
                                db.Entry(livre).State = EntityState.Modified;
                                string oldImagePath = Request.MapPath(Session["imgPath"].ToString());
                                //db.Categorie.Add(categorie);
                                if (db.SaveChanges() > 0)
                                {
                                    upload.SaveAs(path);
                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                }
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        livre.Image = Session["imgPath"].ToString();
                        db.Entry(livre).State = EntityState.Modified;
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.ID_Categorie = new SelectList(db.Categorie, "ID_Categorie", "Nom_Categorie", livre.ID_Categorie);
                return View(livre);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // GET: Admin/Livres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Livre livre = db.Livre.Find(id);
                Session["imgPath"] = livre.Image;
                if (livre == null)
                {
                    return HttpNotFound();
                }
                return View(livre);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        // POST: Admin/Livres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["access"] != null)
            {
                Livre livre = db.Livre.Find(id);

                db.Livre.Remove(livre);
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
