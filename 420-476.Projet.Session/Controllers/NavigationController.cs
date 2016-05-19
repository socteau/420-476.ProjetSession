using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _420_476.Projet.Session.Controllers
{
    public class NavigationController : Controller
    {
        private static string actual = "Index";
        //private static string master = "";
        private static int start = 0;
        private static int itemsPerPage = 15;
        private static int end;

        public static List<T> Prep<T>(IEnumerable<T> list)
        {
            end = list.Count();
            list = list.Skip(start);
            int test = list.Count();
            list = list.Take(itemsPerPage);
            int test2 = list.Count();
            return list.ToList();
            
        }

        

        public ActionResult PreviousServices()
        {
            if (start > 0)
            {
                start -= itemsPerPage;
                return RedirectToAction(actual, "Services");
            }
            return RedirectToAction(actual, "Services");
        }
        
        public ActionResult NextServices()
        {
            if (start <= (end - itemsPerPage))
            {
                start += itemsPerPage;
                return RedirectToAction(actual, "Services");
            }
            else
                return RedirectToAction(actual, "Services");
        }

        public ActionResult PreviousMessages()
        {
            if (start > 0)
            {
                start -= itemsPerPage;
                return RedirectToAction(actual, "Messages");
            }
            return RedirectToAction(actual, "Messages");
        }

        public ActionResult NextMessages()
        {
            if (start <= (end - itemsPerPage))
            {
                start += itemsPerPage;
                return RedirectToAction(actual, "Messages");
            }
            else
                return RedirectToAction(actual, "Messages");
        }
        //internal static void GetNames()
        //{

        //    string path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        //    string[] name = path.Split('/');
        //    string tempMaster = master;
        //    if(name.Count() >= 2)
        //    {
        //        tempMaster = name.ElementAt(name.Count()-2);
        //    }

        //    master = tempMaster;
        //}
    }
}