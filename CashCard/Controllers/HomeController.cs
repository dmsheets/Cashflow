using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;

namespace CashCard.Controllers
{
     [Authorize]
    public class HomeController : Controller
    {
         public HomeController()
         {
             ViewBag.Menu = "MnHome";
         }
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var db = new ApplicationDbContext();
            var user = db.Users.Find(id);

            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}