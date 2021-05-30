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
{[Authorize]
    public class conctactsController : Controller
    {
        private ebookEntities db = new ebookEntities();

        // GET: conctacts
        public ActionResult Index()
        {
            return View(db.conctact.ToList());
        }

        // GET: conctacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            conctact conctact = db.conctact.Find(id);
            if (conctact == null)
            {
                return HttpNotFound();
            }
            return View(conctact);
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
