using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ebook_website.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Net;
using System.Data.Entity;

namespace ebook_website.Controllers
{
    
    public class TestController : Controller
    {
        SqlConnection cn = new SqlConnection(@"Data Source=samir\sqlexpress;Initial Catalog=Projet_EBOOK;Integrated Security=True");
        SqlDataAdapter adap = new SqlDataAdapter();
        DataTable dt = new DataTable();

        private ebookEntities db = new ebookEntities();
        // GET: Test
        public ActionResult home()
        {
            return View();
        }
        public ActionResult register()
        {
            if (Session["entree"] != null)
            {
                return View("afterLogin");
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult register(Utilisateur user)
        {
            if (ModelState.IsValid)
            {
                db.Utilisateur.Add(user);
                db.SaveChanges();
                Session["ins"] = "Bienvenue " + user.Nom + " " + user.Prenom + " inscription avec succès ";
                return View("login");
            }
            ModelState.AddModelError("", "Information invalides");
            return View();
            
        }
        public ActionResult login()
        {
                 return View();
        }
        public static string islogin;
        [HttpPost]
        public ActionResult login(Utilisateur user)
        {
            using (var context = new ebookEntities())
            {
                bool isValid = context.Utilisateur.Any(x => x.Email == user.Email && x.Password == user.Password);
                if (isValid)
                {
                    Session["email"] = "login avec succé";
                    islogin = Request["email"];
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("afterLogin", "Test");
                }
                ModelState.AddModelError("", "adresse mail ou mot de passe incorrects");

            }
            return View();
        }
        public ActionResult afterLogin(string SearchName)
        {
            if (Session["email"]!=null)
            {
                return View(db.Categorie.Where(c => c.Nom_Categorie.Contains(SearchName) || c.Id_Image.Contains(SearchName) || SearchName == null).ToList());
            }  
            else
            {
                 return View("login");
            }
          
        }
        public ActionResult profilee()
        {
            if (Session["email"]!=null)
            {
                var Email = User.Identity.Name;
                var user = db.Utilisateur.Where(a => a.Email == Email).SingleOrDefault();
                Utilisateur profil = new Utilisateur();
                profil.Nom = user.Nom;
                profil.Prenom = user.Prenom;
                profil.Email = user.Email;
                return View(profil);
            }
            else
            {
                return View("login");
            }
        }
        public ActionResult EditPassword()
        {
            if (Session["email"] != null)
            { 
                return View();
            }
            else
                return View("login");
        }


        public ActionResult showBooks(int? id, string nom)
        {
            if (Session["email"] != null)
            {
                List<Livre> lv = new List<Livre>();
                lv = db.Livre.Where(u => u.ID_Categorie == id).ToList();
                Session["nomCat"] = nom;
                return View(lv);
            }
            else
            {
                return View("login");
            }
          
        }
        public ActionResult pageLecture(int?id)
        {
            if (Session["email"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Livre livre = db.Livre.Find(id);
                if (livre == null)
                {
                    return HttpNotFound();
                }
                return View(livre);
            }
            else
            {
                return View("login");
            }
        }
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("login", "Test");
        }
        
        public ActionResult contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult contact( conctact ct)
        {
            using (var context = new ebookEntities())
            {
                context.conctact.Add(ct);
                context.SaveChanges();
                Session["ct"] = "Merci (M/Mme) : " + ct.Nom + " de nous contacter !";
            }
            return View();
        }
        public ActionResult showPDF(int?id)
        {
            if (Session["email"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Livre livre = db.Livre.Find(id);
                if (livre == null)
                {
                    return HttpNotFound();
                }
                Session["id"] = id;
                return View(livre);
            }
            else
            {
                return View("login");
            }
        }
        

    }
}