using _420_476.Projet.Session.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _420_476.Projet.Session.Controllers
{
    public class GestionController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        public ActionResult Rapport()
        {
            return View();
        }

        public ActionResult GererProfils()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // GET: Gestion
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Membres, "UserID", "FirstName", user.ID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Label", user.RoleID);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleID,Login,Statut_disponible,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GererProfils");
            }
            ViewBag.ID = new SelectList(db.Membres, "UserID", "FirstName", user.ID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Label", user.RoleID);
            return View("GererProfils");
        }
    }
}