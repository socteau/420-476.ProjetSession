using _420_476.Projet.Session.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        public ActionResult RapportOffrants()
        {
            var offrants = db.Offrants.Where(x => x.Membre.User.Statut_disponible == true).OrderByDescending(x => x.AverageRating).Take(25).ToList();
            string text = "<table><tr><th>Offrant</th><th>Note moyenne</th></tr>";
            foreach (Offrant offrant in offrants)
            {
                text += ("<tr><td>"+offrant.Membre.User.Login + "</td><td>" + offrant.AverageRating+"</td></tr>");
            }
            text += "</table>";
            System.IO.File.WriteAllText(Server.MapPath("..\\Content\\Rapports\\top25_offrants.html"), text);

            FileInfo file = new FileInfo(Server.MapPath("..\\Content\\Rapports\\top25_offrants.html"));
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
            return RedirectToAction("Rapport");
        }

        public ActionResult RapportMembres()
        {
            var messages = db.Messages.Include(m => m.User).Where(x => x.User.Statut_disponible == true).Take(25).ToList();
            string text = "<table><tr><th>Membres</th><th>Messages Envoyés</th></tr>";
            foreach (Message message in messages)
            {
                int nb = db.Messages.Where(x => x.FromUserID == message.FromUserID).Count();
                text += ("<tr><td>" + message.User.Login + "</td><td>" + nb + "</td></tr>");
            }
            text += "</table>";
            System.IO.File.WriteAllText(Server.MapPath("..\\Content\\Rapports\\top25_membres.html"), text);

            FileInfo file = new FileInfo(Server.MapPath("..\\Content\\Rapports\\top25_membres.html"));
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
            return RedirectToAction("Rapport");
        }
    }
}