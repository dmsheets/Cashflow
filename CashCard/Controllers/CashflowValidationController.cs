using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;

namespace CashCard.Controllers
{
    public class CashflowValidationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /WorkflowValidation/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var branchId = db.Users.Find(userId).BranchId;

            var cashflows = db.CashFlows.Include(c => c.CutOff).Where(p => p.CutOff.BranchId == branchId);
            return View(cashflows.ToList());
        }

        // GET: /WorkflowValidation/Details/5
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

        // GET: /WorkflowValidation/Create
        public ActionResult Create()
        {
            ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note");
            ViewBag.SuperVisorId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: /WorkflowValidation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Date,Note,State,LogNote,CutOffId,UserId,SuperVisorId,Total")] CashFlow cashflow)
        {
            if (ModelState.IsValid)
            {
                db.CashFlows.Add(cashflow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note", cashflow.CutOffId);
            ViewBag.SuperVisorId = new SelectList(db.Users, "Id", "UserName", cashflow.SuperVisorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);
            return View(cashflow);
        }

        // GET: /WorkflowValidation/Edit/5
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
            ViewBag.SuperVisorId = new SelectList(db.Users, "Id", "UserName", cashflow.SuperVisorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);
            return View(cashflow);
        }

        // POST: /WorkflowValidation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Date,Note,State,LogNote,CutOffId,UserId,SuperVisorId,Total")] CashFlow cashflow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashflow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note", cashflow.CutOffId);
            ViewBag.SuperVisorId = new SelectList(db.Users, "Id", "UserName", cashflow.SuperVisorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);
            return View(cashflow);
        }

        // GET: /WorkflowValidation/Delete/5
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

        // POST: /WorkflowValidation/Delete/5
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
