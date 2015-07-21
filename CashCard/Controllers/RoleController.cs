using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CashCard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public RoleController()
        {
            ViewBag.Menu = "MnRole";
        }
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
            //var context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(db);
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

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {

                var role = db.Roles.Find(id);
                db.Entry(role).State = EntityState.Deleted;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View("Error");
            }



        }
    }
}