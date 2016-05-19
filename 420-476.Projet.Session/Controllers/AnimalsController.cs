using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476.Projet.Session.Models;
using System.IO;

namespace _420_476.Projet.Session.Controllers
{
    public class AnimalsController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        // GET: Animals
        public ActionResult Index()
        {
            var animals = db.Animals.Include(a => a.Membre);
            if (Session["UserID"].ToString() != null)
            {
                int id = int.Parse(Session["UserID"].ToString());
                animals = animals.Where(x => x.MembreID == id);
            }
                
            
            return View(animals.ToList());
        }

        // GET: Animals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: Animals/Create
        public ActionResult Create()
        {
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MembreID,Nom,Race,Age,Besoins,Etat,Photo")] Animal animal)
        {
            animal.ID = db.Animals.Count() + 1;
            animal.MembreID = int.Parse(Session["UserID"].ToString());
            //Upload
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var userPath = "~/Content/Images/" + Session["UserID"].ToString();
                    if (!Directory.Exists(Server.MapPath(userPath)))
                        Directory.CreateDirectory(Server.MapPath(userPath));
                    var path = Path.Combine(Server.MapPath(userPath), fileName);
                    if (!System.IO.File.Exists(path))
                        file.SaveAs(path);
                    animal.Photo = fileName;
                }
            }
            //END
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", animal.MembreID);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", animal.MembreID);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MembreID,Nom,Race,Age,Besoins,Etat,Photo")] Animal animal)
        {
            animal.MembreID = int.Parse(Session["UserID"].ToString());
            if (animal.Photo == null)
                animal.Photo = db.Animals.AsNoTracking().Where(x => x.ID == animal.ID).FirstOrDefault().Photo;
            int test = 42;
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MembreID = new SelectList(db.Membres, "UserID", "FirstName", animal.MembreID);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = db.Animals.Find(id);
            db.Animals.Remove(animal);
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
