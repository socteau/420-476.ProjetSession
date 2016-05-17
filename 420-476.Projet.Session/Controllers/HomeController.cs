using _420_476.Projet.Session.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace _420_476.Projet.Session.Controllers
{
    public class HomeController : Controller
    {
        private static string errorMessage = null;
        public ActionResult Login()
        {
            string message = errorMessage;
            errorMessage = null;
            ViewBag.errorMessage = message;
            return View();
        }

        public ActionResult LoginIn()
        {
            String userName = Request.Form["UserName"];
            String password = Request.Form["Password"];

            using (Pet_CareEntities db = new Pet_CareEntities())
            {
                var user = db.Users.Include(u => u.Membre).Include(u => u.Role).Where(x => x.Statut_disponible == true && x.Login.Equals(userName)).FirstOrDefault();
                if (user != null)
                {
                    String dbPassword = user.Password;
                    if (CheckPassword(dbPassword, password))
                    {
                        Session["userRole"] = user.Role.Label;
                        Session["userName"] = user.Login;
                        Session["UserID"] = user.ID;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        errorMessage = "Mot de passe incorrect";
                    }
                }
                else
                {
                    errorMessage = "Nom de compte introuvable";
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Profil()
        {
            return View();
        }

        public ActionResult Theme()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        private bool CheckPassword(string dbHashedPassword, string UserUnhashedPassword)
        {
            return Crypto.VerifyHashedPassword(dbHashedPassword, UserUnhashedPassword);
        }

        public ActionResult LoginOut()
        {
            Session["userRole"] = null;
            Session["userName"] = null;
            Session["UserID"] = null;
            return RedirectToAction("Index");
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
        public void saveCookie()
        {
            string type = "";
            foreach (string item in Request.Form)
            {                
                type = Request[item];
            }
            @Response.Cookies["typeOffre"].Value = type;
            @Response.Cookies["typeOffre"].Expires = DateTime.Now.AddDays(7);
=======
>>>>>>> 2962e45c80512361c97c9a26755705a1eaa4c7c2
        public ActionResult Evolve()
        {
            using(Pet_CareEntities context = new Pet_CareEntities())
            {
                string log = Session["username"].ToString();
                var user = context.Users.Where(x => x.Login == log).FirstOrDefault();
                var role = context.Roles.Where(x => x.ID == 4).FirstOrDefault();
                user.Role = role;
                Session["userRole"] = role.Label;

                context.SaveChanges();
            }
            return RedirectToAction("Index");
<<<<<<< HEAD
=======
        public void saveCookie()
        {
            string type = "";
            foreach (string item in Request.Form)
            {                
                type = Request[item];
            }
            @Response.Cookies["typeOffre"].Value = type;
            @Response.Cookies["typeOffre"].Expires = DateTime.Now.AddDays(7);
>>>>>>> e9ec799b88ba3a0b7402341704c115041f1261bf
=======
>>>>>>> 7617ecf227df5ff5d3f98858caaf7c680747776d
>>>>>>> 2962e45c80512361c97c9a26755705a1eaa4c7c2
        }
    }
}