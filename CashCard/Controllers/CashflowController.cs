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
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace CashCard.Controllers
{
    [Authorize(Roles = "User")]
    public class CashflowController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CashflowController()
        {
            ViewBag.Menu = "MnCashflow";
        }
        // GET: /Cashflow/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var cashflows =
                db.CashFlows
                    .Where(p => p.UserId == userId);
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
        public ActionResult CashoutRegular()
        {
         
            ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
            ViewBag.RegularQuizInfo = from x in db.RegularQuizs select new {Id = x.Id, Info = x.Info};

            var cash = new CashOutRegular();

            return View( cash);
        }

        // POST: /Cashflow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      /*  [HttpPost]
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

        */

        private void Subtitution(CashOutRegular cashoutDb, CashOutRegular cashoutView)
        {
            cashoutDb.Date = cashoutView.Date;
            cashoutDb.Note = cashoutView.Note;

            //delete detail
            for (int i = cashoutDb.RegularDetails.Count - 1; i >= 0; i--)
            {
                var idReg = cashoutDb.RegularDetails[i].Id;

                var reg = cashoutView.RegularDetails.FirstOrDefault(p => p.Id == idReg);
                if (reg == null)
                {
                    db.Entry(cashoutDb.RegularDetails[i]).State = EntityState.Deleted;
                    //cashoutDb.RegularDetails.RemoveAt(i);
                }
            }
            //add or update detail
            for (int i = 0; i < cashoutView.RegularDetails.Count; i++)
            {
                if (cashoutView.RegularDetails[i].Id == 0)
                {
                    cashoutDb.RegularDetails.Add(cashoutView.RegularDetails[i]);
                }
                else
                {
                    var regDetail = cashoutView.RegularDetails[i];
                    var detail = cashoutDb.RegularDetails.First(p => p.Id == regDetail.Id);
                    detail.RegularQuizId = regDetail.RegularQuizId;
                    detail.RegularDetailQuiz = null;
                    detail.Note1 = regDetail.Note1;
                    detail.Note2 = regDetail.Note2;
                    detail.Qty = regDetail.Qty;
                    detail.Amount = regDetail.Amount;
                  
                }
            }


        }
        [HttpPost]
        public JsonResult CreateCashoutRegularDraft(CashOutRegular cashoutRegular)
        {
            try
            {
                var cashout = cashoutRegular;

                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start);
                if (cutOff == null)
                {
                    cutOff = new CutOff();
                    cutOff.State = StateCutOff.Start;
                    cutOff.DateStart = DateTime.Now;
                    cutOff.DateEnd = DateTime.Now;
                    cutOff.BranchId = xx.BranchId.Value;
                    db.CutOffs.Add(cutOff);
                    db.SaveChanges();

                }

                if (cashoutRegular.Id != 0)
                {
                    cashout = db.CashFlows.OfType<CashOutRegular>().First(p => p.Id == cashoutRegular.Id);

                    Subtitution(cashout, cashoutRegular);




                }
                else
                {
                    db.CashFlows.Add(cashout);
                    //cashoutRegular.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;



                }
                //set subtotal and total

                cashout.SetToDraft();
                cashout.RegularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new {Success = 1, CashOutId = cashoutRegular.Id, ex = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Success = 0, ex = ex.Message});
            }


        }

        [HttpPost]
        public JsonResult CreateCashoutRegularFinal(CashOutRegular cashoutRegular)
        {
            try
            {
                var cashout = cashoutRegular;
                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start);
                if (cutOff == null)
                {
                    cutOff = new CutOff();
                    cutOff.State = StateCutOff.Start;
                    cutOff.DateStart = DateTime.Now;
                    cutOff.DateEnd = DateTime.Now;
                    cutOff.BranchId = xx.BranchId.Value;
                    db.CutOffs.Add(cutOff);
                    db.SaveChanges();

                }

                if (cashoutRegular.Id != 0)
                {
                   cashout = db.CashFlows.OfType<CashOutRegular>().First(p => p.Id == cashoutRegular.Id);

                    Subtitution(cashout, cashoutRegular);
      
                }
                else
                {
                    db.CashFlows.Add(cashout);
                    //cashoutRegular.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;
                 
                }


                cashout.SetToFinal();

                cashout.RegularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashoutRegular.Id, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }


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
            //ViewBag.CutOffId = new SelectList(db.CutOffs, "Id", "Note", cashflow.CutOffId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", cashflow.UserId);

            ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
            ViewBag.RegularQuizInfo = from x in db.RegularQuizs select new { Id = x.Id, Info = x.Info };

            return View("CashoutRegular",(CashOutRegular) cashflow);
            //return View( cashflow);
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
