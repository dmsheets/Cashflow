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
                db.CashFlows.Include(p=>p.CutOff)
                    .Where(p => p.UserId == userId && p.CutOff.State == StateCutOff.Start);
            return View(cashflows.ToList());
        }

   

        // GET: /Cashflow/Create
        public ActionResult CashoutRegular()
        {
         
            ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
            ViewBag.RegularQuizInfo = from x in db.RegularQuizs select new {Id = x.Id, Info = x.Info};

            var cash = new CashOutRegular();

            return View( cash);
        }

        public ActionResult CashIn()
        {


            var cash = new CashIn();

            return View(cash);
        }

      

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

        private void Subtitution(CashIn cashInDb, CashIn cashInView)
        {
            cashInDb.Date = cashInView.Date;
            cashInDb.Note = cashInView.Note;

            //delete detail
            for (int i = cashInDb.CashInDetails.Count - 1; i >= 0; i--)
            {
                var idReg = cashInDb.CashInDetails[i].Id;

                var reg = cashInView.CashInDetails.FirstOrDefault(p => p.Id == idReg);
                if (reg == null)
                {
                    db.Entry(cashInDb.CashInDetails[i]).State = EntityState.Deleted;
                    //cashoutDb.RegularDetails.RemoveAt(i);
                }
            }
            //add or update detail
            for (int i = 0; i < cashInView.CashInDetails.Count; i++)
            {
                if (cashInView.CashInDetails[i].Id == 0)
                {
                    cashInDb.CashInDetails.Add(cashInView.CashInDetails[i]);
                }
                else
                {
                    var regDetail = cashInView.CashInDetails[i];
                    var detail = cashInDb.CashInDetails.First(p => p.Id == regDetail.Id);
                    detail.Note = regDetail.Note;
                    detail.Amount = regDetail.Amount;

                }
            }


        }

        #region CashIn
        [HttpPost]
        public JsonResult CreateCashInDraft(CashIn cashIn)
        {
            try
            {
                var cashInActive = cashIn;

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

                if (cashIn.Id != 0)
                {
                    cashInActive = db.CashFlows.OfType<CashIn>().First(p => p.Id == cashIn.Id);

                    Subtitution(cashInActive, cashIn);




                }
                else
                {
                    db.CashFlows.Add(cashInActive);
                    //cashoutRegular.Date = DateTime.Now;
                    cashInActive.CutOffId = cutOff.Id;
                    cashInActive.UserId = usr;



                }
                //set subtotal and total

                cashInActive.SetToDraft();
               
                cashInActive.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashIn.Id, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }


        }

        [HttpPost]
        public JsonResult CreateCashInFinal(CashIn cashIn)
        {
            try
            {
                var cashInActive = cashIn;
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

                if (cashIn.Id != 0)
                {
                    cashInActive = db.CashFlows.OfType<CashIn>().First(p => p.Id == cashIn.Id);

                    Subtitution(cashInActive, cashIn);

                }
                else
                {
                    db.CashFlows.Add(cashInActive);
                    //cashoutRegular.Date = DateTime.Now;
                    cashInActive.CutOffId = cutOff.Id;
                    cashInActive.UserId = usr;

                }


                cashInActive.SetToFinal();
                cashInActive.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashIn.Id, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }


        }

        #endregion
        #region Cashout Regular
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

        #endregion

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

            var cashOutRegular = cashflow as CashOutRegular;
            if (cashOutRegular != null)
            {
                if (cashOutRegular.State == StateCashFlow.Draft || cashOutRegular.State == StateCashFlow.Revision)
                {
                    ViewBag.RegularQuiz = new SelectList(db.RegularQuizs, "Id", "Quiz");
                    ViewBag.RegularQuizInfo = from x in db.RegularQuizs select new {Id = x.Id, Info = x.Info};
                    return View("CashoutRegular", cashOutRegular);
                }
                return View("CashoutRegularInfo", cashOutRegular);
            }
            var cashIn = cashflow as CashIn;
            if (cashIn != null)
            {
                if (cashIn.State == StateCashFlow.Draft || cashIn.State == StateCashFlow.Revision)
                {
                    return View("CashIn", cashIn);
                }
                return View("CashInInfo", cashIn);
            }

            return View("CashIn", cashIn);

            //return View( cashflow);
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
