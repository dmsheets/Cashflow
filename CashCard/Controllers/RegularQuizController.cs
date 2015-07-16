using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
    public class RegularQuizController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /RegularQuiz/
        public ActionResult Index()
        {
            return View(db.RegularQuizs.ToList());
        }

        // GET: /RegularQuiz/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularQuiz RegularQuiz = db.RegularQuizs.Find(id);
            if (RegularQuiz == null)
            {
                return HttpNotFound();
            }
            return View(RegularQuiz);
        }

        // GET: /RegularQuiz/Create
        public ActionResult Create()
        {
            var dt = db.RegularGroup.Select(oo => new {aaa = oo.Id, bbb = oo.AccountNo + "-" + oo.AccountDesription}).ToList();

            var x = new SelectList(dt,"aaa", "bbb");
            ViewBag.RegularGroups = x;
            var y = new SelectList(Enum.GetValues(typeof (RegularType)));
            ViewBag.RegularType = y;
            return View();
        }

        // POST: /RegularQuiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegularQuiz RegularQuiz)
        {
            if (ModelState.IsValid)
            {
                db.RegularQuizs.Add(RegularQuiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(RegularQuiz);
        }

        // GET: /RegularQuiz/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularQuiz RegularQuiz = db.RegularQuizs.Find(id);
            if (RegularQuiz == null)
            {
                return HttpNotFound();
            }

            var dt = db.RegularGroup.Select(oo => new { aaa = oo.Id, bbb = oo.AccountNo + "-" + oo.AccountDesription }).ToList();

            var x = new SelectList(dt, "aaa", "bbb");
            ViewBag.RegularGroups = x;
            var y = new SelectList(Enum.GetValues(typeof(RegularType)));
            ViewBag.RegularType = y;

            return View(RegularQuiz);
        }

        // POST: /RegularQuiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegularQuiz RegularQuiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(RegularQuiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(RegularQuiz);
        }

        // GET: /RegularQuiz/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularQuiz RegularQuiz = db.RegularQuizs.Find(id);
            if (RegularQuiz == null)
            {
                return HttpNotFound();
            }
            return View(RegularQuiz);
        }

        // POST: /RegularQuiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegularQuiz RegularQuiz = db.RegularQuizs.Find(id);
            db.RegularQuizs.Remove(RegularQuiz);
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
    }
}
