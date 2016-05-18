using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476.Projet.Session.Models;

namespace _420_476.Projet.Session.Controllers
{
    public class OffrantsController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        // GET: Offrants
        public ActionResult Index()
        {
            var offrants = db.Offrants.Include(o => o.Membre);
            return View(offrants.ToList());
        }

        // GET: Offrants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offrant offrant = db.Offrants.Find(id);
            if (offrant == null)
            {
                return HttpNotFound();
            }
            return View(offrant);
        }

        // GET: Offrants/Create
        public ActionResult Create()
        {
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName");
            return View();
        }

        // POST: Offrants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MembreID,Region,AverageRating")] Offrant offrant)
        {
            if (ModelState.IsValid)
            {
                db.Offrants.Add(offrant);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", offrant.MembreID);
            return View(offrant);
        }

        // GET: Offrants/Edit/5
        public ActionResult Edit(int? id)
        {
            if(Session["UserID"].ToString() != null && id == null)
                id = int.Parse(Session["UserID"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offrant offrant = db.Offrants.Find(id);
            if (offrant == null)
            {
                return HttpNotFound();
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", offrant.MembreID);
            return View(offrant);
        }

        // POST: Offrants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MembreID,Region,AverageRating")] Offrant offrant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offrant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", offrant.MembreID);
            return View(offrant);
        }

        // GET: Offrants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offrant offrant = db.Offrants.Find(id);
            if (offrant == null)
            {
                return HttpNotFound();
            }
            return View(offrant);
        }

        // POST: Offrants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offrant offrant = db.Offrants.Find(id);
            db.Offrants.Remove(offrant);
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
    }
}
