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
    public class RegularQuizController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /RegularQuiz/
        public ActionResult Index()
        {
            return View(db.RegularDetailQuizs.ToList());
        }

        // GET: /RegularQuiz/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularDetailQuiz regulardetailquiz = db.RegularDetailQuizs.Find(id);
            if (regulardetailquiz == null)
            {
                return HttpNotFound();
            }
            return View(regulardetailquiz);
        }

        // GET: /RegularQuiz/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /RegularQuiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,SapCode,ConterName,Quiz,Active")] RegularDetailQuiz regulardetailquiz)
        {
            if (ModelState.IsValid)
            {
                db.RegularDetailQuizs.Add(regulardetailquiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(regulardetailquiz);
        }

        // GET: /RegularQuiz/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularDetailQuiz regulardetailquiz = db.RegularDetailQuizs.Find(id);
            if (regulardetailquiz == null)
            {
                return HttpNotFound();
            }
            return View(regulardetailquiz);
        }

        // POST: /RegularQuiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,SapCode,ConterName,Quiz,Active")] RegularDetailQuiz regulardetailquiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regulardetailquiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(regulardetailquiz);
        }

        // GET: /RegularQuiz/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegularDetailQuiz regulardetailquiz = db.RegularDetailQuizs.Find(id);
            if (regulardetailquiz == null)
            {
                return HttpNotFound();
            }
            return View(regulardetailquiz);
        }

        // POST: /RegularQuiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegularDetailQuiz regulardetailquiz = db.RegularDetailQuizs.Find(id);
            db.RegularDetailQuizs.Remove(regulardetailquiz);
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
