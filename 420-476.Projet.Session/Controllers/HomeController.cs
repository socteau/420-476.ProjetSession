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

        public void saveCookie()
        {
            string type = "";
            foreach (string item in Request.Form)
            {                
                type = Request[item];
            }
            @Response.Cookies["typeOffre"].Value = type;
            @Response.Cookies["typeOffre"].Expires = DateTime.Now.AddDays(7);
        }
    }
}