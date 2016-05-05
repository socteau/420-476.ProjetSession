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
    public class ServicesRatingsController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        // GET: ServicesRatings
        public ActionResult Index()
        {
            var servicesRatings = db.ServicesRatings.Include(s => s.Membre).Include(s => s.Offrant).Include(s => s.Service);
            return View(servicesRatings.ToList());
        }

        // GET: ServicesRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesRating servicesRating = db.ServicesRatings.Find(id);
            if (servicesRating == null)
            {
                return HttpNotFound();
            }
            return View(servicesRating);
        }

        // GET: ServicesRatings/Create
        public ActionResult Create()
        {
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName");
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region");
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Label");
            return View();
        }

        // POST: ServicesRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ServiceID,MembreID,OffrantID,Note,Text")] ServicesRating servicesRating)
        {
            if (ModelState.IsValid)
            {
                db.ServicesRatings.Add(servicesRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", servicesRating.MembreID);
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", servicesRating.OffrantID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Label", servicesRating.ServiceID);
            return View(servicesRating);
        }

        // GET: ServicesRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesRating servicesRating = db.ServicesRatings.Find(id);
            if (servicesRating == null)
            {
                return HttpNotFound();
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", servicesRating.MembreID);
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", servicesRating.OffrantID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Label", servicesRating.ServiceID);
            return View(servicesRating);
        }

        // POST: ServicesRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ServiceID,MembreID,OffrantID,Note,Text")] ServicesRating servicesRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicesRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", servicesRating.MembreID);
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", servicesRating.OffrantID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Label", servicesRating.ServiceID);
            return View(servicesRating);
        }

        // GET: ServicesRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicesRating servicesRating = db.ServicesRatings.Find(id);
            if (servicesRating == null)
            {
                return HttpNotFound();
            }
            return View(servicesRating);
        }

        // POST: ServicesRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicesRating servicesRating = db.ServicesRatings.Find(id);
            db.ServicesRatings.Remove(servicesRating);
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
