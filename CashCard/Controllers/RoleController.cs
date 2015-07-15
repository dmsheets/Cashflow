using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CashCard.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Role/
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(string name)
        {
            var context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
           IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
           if (result.Succeeded)
           {
               //await SignInAsync(user, isPersistent: false);


               return RedirectToAction("Index");
           }

            AddErrors(result);
            return View(name);


        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
	}
}