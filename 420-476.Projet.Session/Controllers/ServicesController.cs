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
    public class ServicesController : Controller
    {
        private Pet_CareEntities db = new Pet_CareEntities();

        //Consulter les offres
        // GET: Services
        public ActionResult Index()
        {
            var services = db.Services.Where(x => x.Statut_actif == true).Include(s => s.Offrant).ToList();
            if (Session["filtre"] != null)
            {
                var filtre = Session["filtre"].ToString();
                services = db.Services.Where(x => x.Statut_actif == true).Where(x => x.Type == filtre).Include(s => s.Offrant).ToList();
            }
            services = NavigationController.Prep(services);
            return View(services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region");
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Autre", Value = "Autre" });
            items.Add(new SelectListItem { Text = "Garderie", Value = "Garderie" });
            items.Add(new SelectListItem { Text = "Nourriture", Value = "Nourriture" });
            items.Add(new SelectListItem { Text = "Photo", Value = "Photo" });
            items.Add(new SelectListItem { Text = "Promenage", Value = "Promenage" });
            items.Add(new SelectListItem { Text = "Tonte", Value = "Tonte" });
            items.Add(new SelectListItem { Text = "Tonte et toilettage", Value = "Tonte et toilettage" });
            items.Add(new SelectListItem { Text = "Toilettage", Value = "Toilettage" });
            items.Add(new SelectListItem { Text = "Transport", Value = "Transport" });
            ViewBag.Type = items;
            
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OffrantID,Label,Type,Prix,Statut_actif")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.OffrantID = Int32.Parse(Session["UserID"].ToString());
                service.Statut_actif = true;
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("MesOffres");
            }

            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", service.OffrantID);
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", service.OffrantID);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OffrantID,Label,Type,Prix")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MesOffres");
            }
            ViewBag.OffrantID = new SelectList(db.Offrants, "MembreID", "Region", service.OffrantID);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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

        public ActionResult MesOffres()
        {
            var services = db.Services.Include(s => s.Offrant).ToList();
            if (Session["UserID"].ToString() != null)
            {
                int id = int.Parse(Session["UserID"].ToString());
                services = db.Services.Include(s => s.Offrant).Where(x => x.OffrantID == id).ToList();
            }
            services = NavigationController.Prep(services);
            return View(services.ToList());
        }

        public ActionResult publicite()
        {
            return View();
        }

        public void accepterService()
        {
            int offrantID = -1;
            int userID = -1;
            int serviceID= -1;
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
                    case ("serviceID"):
                        serviceID = Int32.Parse(Request["serviceID"]);
                        break;
                }
            }

            ServicesRating serviceRating = new ServicesRating()
            {
                ServiceID = serviceID,
                MembreID = userID,
                OffrantID = offrantID,
                Note = 0,
                Text = ""               
            };
            db.ServicesRatings.Add(serviceRating);
            db.SaveChanges();
        }

        public ActionResult typesAutoComplete()
        {
            string term = Request.QueryString["term"].ToLower();
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Autre", Value = "Autre" });
            items.Add(new SelectListItem { Text = "Garderie", Value = "Garderie" });
            items.Add(new SelectListItem { Text = "Nourriture", Value = "Nourriture" });
            items.Add(new SelectListItem { Text = "Photo", Value = "Photo" });
            items.Add(new SelectListItem { Text = "Promenage", Value = "Promenage" });
            items.Add(new SelectListItem { Text = "Tonte", Value = "Tonte" });
            items.Add(new SelectListItem { Text = "Tonte et toilettage", Value = "Tonte et toilettage" });
            items.Add(new SelectListItem { Text = "Toilettage", Value = "Toilettage" });
            items.Add(new SelectListItem { Text = "Transport", Value = "Transport" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Research(string search)
        {
            Session["filtre"] = search;
            //var services = db.Services.Where(x => x.Type == search).Where(x => x.Statut_actif == true).Include(s => s.Offrant).ToList();
            //services = NavigationController.Prep(services);
            //return View("Index",services.ToList());
            return View("Index");
        }
    }
}
