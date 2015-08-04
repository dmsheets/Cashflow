using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegularGroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public RegularGroupController()
        {
            ViewBag.Menu = "MnRegularGroup";
        }
        // GET: /QuizGroup/
        public ActionResult Index()
        {
            return View(db.RegularGroup.ToList());
        }

        // GET: /QuizGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizGroup regulargroup = db.RegularGroup.Find(id);
            if (regulargroup == null)
            {
                return HttpNotFound();
            }
            return View(regulargroup);
        }

        // GET: /QuizGroup/Create
        public ActionResult Create()
        {
            var x = new SelectList(Enum.GetValues(typeof(GroupType)));
            ViewBag.GroupTypes = x;

       
            return View();
        }

        // POST: /QuizGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,AccountNo,AccountDesription,GroupType,Info")] QuizGroup regulargroup)
        {
            if (ModelState.IsValid)
            {
                db.RegularGroup.Add(regulargroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(regulargroup);
        }

        // GET: /QuizGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizGroup regulargroup = db.RegularGroup.Find(id);
            if (regulargroup == null)
            {
                return HttpNotFound();
            }
            var x = new SelectList(Enum.GetValues(typeof(GroupType)));
            ViewBag.GroupTypes = x;
            return View(regulargroup);
        }

        // POST: /QuizGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,AccountNo,AccountDesription,GroupType,Info")] QuizGroup regulargroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regulargroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(regulargroup);
        }

        // GET: /QuizGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizGroup regulargroup = db.RegularGroup.Find(id);
            if (regulargroup == null)
            {
                return HttpNotFound();
            }
            return View(regulargroup);
        }

        // POST: /QuizGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
          
            try
            {
                QuizGroup regulargroup = db.RegularGroup.Find(id);
                db.RegularGroup.Remove(regulargroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult CheckName(string accountNo)
        {
            var count =
                db.RegularGroup.Count(p => p.AccountNo.Equals(accountNo, StringComparison.InvariantCultureIgnoreCase));

            if (count == 0)
                return Json(true, JsonRequestBehavior.AllowGet);


            return Json(string.Format("{0} is already exist", accountNo), JsonRequestBehavior.AllowGet);
        }
    }
}
