using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
    public class CashflowController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Cashflow/
        public ActionResult Index()
        {
            var cashflows = db.CashFlows.Include(c => c.CutOff).Include(c => c.User);
            return View(cashflows.ToList());
        }

        // GET: /Cashflow/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashFlow cashflow = db.CashFlows.Find(id);
            if (cashflow == null)
            {
                return HttpNotFound();
            }
            return View(cashflow);
        }

        // GET: /Cashflow/Create
        public ActionResult CreateCashoutRegular()
        {
         
            ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
            ViewBag.RegularQuizInfo = from x in db.RegularQuizs select new {Id = x.Id, Info = x.Info};


            return View();
        }

        // POST: /Cashflow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCashoutRegular([Bind(Include="Id,Date,Note,State,LogNote,CutOffId,UserId")] CashOutRegular cashflow)
        {
            if (ModelState.IsValid)
            {
                db.CashFlows.Add(cashflow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
            return View(cashflow);
        }

        // GET: /Cashflow/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashFlow cashflow = db.CashFlows.Find(id);
            if (cashflow == null)
            {
                return HttpNotFound();
            }
            ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note", cashflow.CutOffId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);
            return View(cashflow);
        }

        // POST: /Cashflow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Date,Note,State,LogNote,CutOffId,UserId")] CashOutRegular cashflow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashflow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note", cashflow.CutOffId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);
            return View(cashflow);
        }

        // GET: /Cashflow/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashFlow cashflow = db.CashFlows.Find(id);
            if (cashflow == null)
            {
                return HttpNotFound();
            }
            return View(cashflow);
        }

        // POST: /Cashflow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashFlow cashflow = db.CashFlows.Find(id);
            db.CashFlows.Remove(cashflow);
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
