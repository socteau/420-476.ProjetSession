using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476.Projet.Session.Models;
using System.Web.Helpers;
using System.Net.Mail;

namespace _420_476.Projet.Session.Controllers
{
    public class UsersController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Membre).Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
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
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Membres, "UserID", "FirstName");
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Label");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleID,Login,Statut_disponible,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = db.Roles.Where(x =>x.ID==3).FirstOrDefault();
                user.Statut_disponible = true;
                user.Password = Crypto.HashPassword(user.Password);                                
                db.Users.Add(user);                
                //db.SaveChanges();                
                int MAXID = db.Users.Max(x => x.ID);                
                User lastAdded = db.Users.Where(x => x.ID == MAXID).FirstOrDefault();
                Membre membre = new Membre() { UserID=lastAdded.ID,
                Address = "",
                FirstName = "",
                LastName= ""              
                };
                db.Membres.Add(membre);
                //db.SaveChanges();
                sendEmail(user);
                Session["userRole"] = user.Role.Label;
                Session["userName"] = user.Login;
                Session["UserID"] = lastAdded.ID;
                return RedirectToAction("Create", "Membres");
                //return RedirectToAction("Edit", "Membres", new { id = lastAdded.ID });
            }

            ViewBag.ID = new SelectList(db.Membres, "UserID", "FirstName", user.ID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Label", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleID,Login,Statut_disponible,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //int id = Int32.Parse(Session["UserID"].ToString());
                var dbUser = db.Users.Where(x => x.ID == 1013).FirstOrDefault();
                user.ID = dbUser.ID;
                user.RoleID = dbUser.RoleID;
                user.Statut_disponible = dbUser.Statut_disponible;
                if (!SamePassword(user.ID, user.Password))
                {
                    user.Password = Crypto.HashPassword(user.Password);
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Membres, "UserID", "FirstName", user.ID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Label", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        private void sendEmail(User user)
        {
            string to = user.Email;
            string from = "samuel.octeau@hotmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Welcome to Pet-Care.";
            message.Body = @"Welcome "+user.Login+"!\nWelcome to Pet-Care";
            SmtpClient client = new SmtpClient("smtp.sendgrid.net");
            client.Port = 2525;
            client.Credentials = new System.Net.NetworkCredential("azure_5689b4033a9d224a512b2c18edc6fff2@azure.com", "vachier#123");
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
            //client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());
            }
        }

        public bool SamePassword(int id, string password)
        {
            using(Pet_CareEntities db = new Pet_CareEntities())
            {
                var hashPwd = db.Users.Where(x => x.ID == id).FirstOrDefault().Password;
                return hashPwd.Equals(password);
            }
        }
    }
}
