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
    public class MessagesController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        // GET: Messages
        public ActionResult Index()
        {
            var messages = db.Messages.Include(m => m.User).Include(m => m.User1).ToList();
            messages = NavigationController.Prep(messages);
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.FromUserID = new SelectList(db.Users, "ID", "Login");
            ViewBag.ToUserID = new SelectList(db.Users, "ID", "Login");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FromUserID,ToUserID,Sujet,Texte,DateEnvoie,Statut_lu")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromUserID = new SelectList(db.Users, "ID", "Login", message.FromUserID);
            ViewBag.ToUserID = new SelectList(db.Users, "ID", "Login", message.ToUserID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromUserID = new SelectList(db.Users, "ID", "Login", message.FromUserID);
            ViewBag.ToUserID = new SelectList(db.Users, "ID", "Login", message.ToUserID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FromUserID,ToUserID,Sujet,Texte,DateEnvoie,Statut_lu")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromUserID = new SelectList(db.Users, "ID", "Login", message.FromUserID);
            ViewBag.ToUserID = new SelectList(db.Users, "ID", "Login", message.ToUserID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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

        public ActionResult Plainte()
        {
            return View();
        }

        public ActionResult GererPlaintes()
        {
            return View();
        }

        public void ajaxCreateMessage()
        {
            int offrantID = -1;
            int userID = -1;
            string msg = "";
            string sujet = "";
            foreach (string item in Request.Form)
            {
                switch (item)
                {
                    case ("offrant"):
                        offrantID = Int32.Parse(Request["offrant"]);
                        break;
                    case ("user"):
                        userID = Int32.Parse(Request["user"]);
                        break;
                    case ("message"):
                        msg = Request["message"].ToString();
                        break;
                    case ("sujet"):
                        sujet = Request["sujet"].ToString();
                        break;
                }
            }

            Message message = new Message()
            {
                DateEnvoie = DateTime.Now,
                FromUserID = userID,
                ToUserID = offrantID,
                Sujet = sujet,
                Statut_lu = false,
                Texte = msg
            };
//            Console.Write(message.ToString());
            db.Messages.Add(message);
            db.SaveChanges();            
        }
    }
}
