using ebook_website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ebook_website.Controllers
{
    public class CategoriesController : Controller
    {
       
        private ebookEntities db = new ebookEntities();
        // GET: Admin/Categories
        public ActionResult Index(string SearchName)
        {

            if (Session["access"] != null)
            {
                return View(db.Categorie.Where(c => c.Nom_Categorie.Contains(SearchName) || c.Id_Image.Contains(SearchName) || SearchName == null).ToList());
            }
            else
            return RedirectToAction("Login","Admin");
           

        }


        // GET: Admin/Categories/Details/5

        public ActionResult Details(int? id)
        {
            
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Categorie categorie = db.Categorie.Find(id);
                Session["imgPath"] = categorie.Id_Image;
                if (categorie == null)
                {
                    return HttpNotFound();
                }
                return View(categorie);
            }
            else
                return RedirectToAction("Login", "Admin");


        }

        // GET: Admin/Categories/Create
        [HttpGet]
        public ActionResult Create()
        {

            if (Session["access"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");


        }

        // POST: Admin/Categories/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categorie categorie, HttpPostedFileBase upload)
        {
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    string filename = Path.GetFileName(upload.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(upload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/Categories/"), _filename);
                    categorie.Id_Image = "~/Uploads/Categories/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (upload.ContentLength <= 1000000)
                        {
                            db.Categorie.Add(categorie);
                            if (db.SaveChanges() > 0)
                            {
                                upload.SaveAs(path);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }

                return View(categorie);
            }
            else
                return RedirectToAction("Login", "Admin");

        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Categorie categorie = db.Categorie.Find(id);

                Session["imgPath"] = categorie.Id_Image;

                if (categorie == null)
                {
                    return HttpNotFound();
                }
                return View(categorie);
            }
            else
                return RedirectToAction("Login", "Admin");


        }

        // POST: Admin/Categories/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Categorie,Nom_Categorie,Id_Image")] Categorie categorie, HttpPostedFileBase upload)
        {
           
            if (Session["access"] != null)
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(categorie).State = EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                    if (upload != null)
                    {
                        string filename = Path.GetFileName(upload.FileName);
                        string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                        string extension = Path.GetExtension(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads/Categories/"), _filename);
                        categorie.Id_Image = "~/Uploads/Categories/" + _filename;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (upload.ContentLength <= 1000000)
                            {
                                db.Entry(categorie).State = EntityState.Modified;
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
                        categorie.Id_Image = Session["imgPath"].ToString();
                        db.Entry(categorie).State = EntityState.Modified;
                        return RedirectToAction("Index");
                    }
                }
                return View(categorie);
            }
            else
                return RedirectToAction("Login", "Admin");


        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["access"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Categorie categorie = db.Categorie.Find(id);
                Session["imgPath"] = categorie.Id_Image;
                if (categorie == null)
                {
                    return HttpNotFound();
                }
                return View(categorie);
            }
            else
                return RedirectToAction("Login", "Admin");


        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["access"] != null)
            {
                Categorie categorie = db.Categorie.Find(id);
                db.Categorie.Remove(categorie);
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