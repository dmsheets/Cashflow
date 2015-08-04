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
    [Authorize(Roles = "Admin")]
    public class RegularQuizController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public RegularQuizController()
        {
            ViewBag.Menu = "MnRegularQuiz";
        }
        // GET: /Quiz/
        public ActionResult Index()
        {
            return View(db.Quizs.ToList());
        }

        // GET: /Quiz/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: /Quiz/Create
        public ActionResult Create()
        {
            var dt = db.RegularGroup.Select(oo => new {aaa = oo.Id, bbb = oo.AccountNo + "-" + oo.AccountDesription}).ToList();

            var x = new SelectList(dt,"aaa", "bbb");
            ViewBag.RegularGroups = x;
            var y = new SelectList(Enum.GetValues(typeof (CostCenter)));
            ViewBag.RegularType = y;
            return View();
        }

        // POST: /Quiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizs.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: /Quiz/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            var dt = db.RegularGroup.Select(oo => new { aaa = oo.Id, bbb = oo.AccountNo + "-" + oo.AccountDesription }).ToList();

            var x = new SelectList(dt, "aaa", "bbb");
            ViewBag.RegularGroups = x;
            var y = new SelectList(Enum.GetValues(typeof(CostCenter)));
            ViewBag.RegularType = y;

            return View(quiz);
        }

        // POST: /Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: /Quiz/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: /Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Quiz quiz = db.Quizs.Find(id);
                db.Quizs.Remove(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
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
    }
}
