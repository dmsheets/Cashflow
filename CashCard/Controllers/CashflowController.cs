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

        public ActionResult CashoutIrregular()
        {
            var y = new SelectList(Enum.GetValues(typeof(IrregularType)));
            ViewBag.IrregularTypes = y;
          

            var cash = new CashOutIrregular();

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

        private void Subtitution(CashOutIrregular cashInDb, CashOutIrregular cashInView)
        {
            cashInDb.Date = cashInView.Date;
            cashInDb.Note = cashInView.Note;

            //delete detail
            for (int i = cashInDb.IrregularDetails.Count - 1; i >= 0; i--)
            {
                var idReg = cashInDb.IrregularDetails[i].Id;

                var reg = cashInView.IrregularDetails.FirstOrDefault(p => p.Id == idReg);
                if (reg == null)
                {
                    db.Entry(cashInDb.IrregularDetails[i]).State = EntityState.Deleted;
                    //cashoutDb.RegularDetails.RemoveAt(i);
                }
            }
            //add or update detail
            for (int i = 0; i < cashInView.IrregularDetails.Count; i++)
            {
                if (cashInView.IrregularDetails[i].Id == 0)
                {
                    cashInDb.IrregularDetails.Add(cashInView.IrregularDetails[i]);
                }
                else
                {
                    var regDetail = cashInView.IrregularDetails[i];
                    var detail = cashInDb.IrregularDetails.First(p => p.Id == regDetail.Id);
                    detail.IrregularType = regDetail.IrregularType;
                    detail.FlightDate = regDetail.FlightDate;
                    detail.FlightNo = regDetail.FlightNo;
                    detail.FromTo = regDetail.FromTo;
                    detail.Amount = regDetail.Amount;
                    detail.Qty = regDetail.Qty;
                    detail.Note = regDetail.Note;
                   
                  

                }
            }


        }

        public CutOff SetCutOff(int branchId)
        {
            var xx = db.CutOffs.Where(p => p.BranchId == branchId).OrderByDescending(p => p.DateStart).Take(1).ToList();
            var lastBallance = 0;
            if (xx.Count > 0)
            {
                lastBallance = xx[0].EndBallance;
            }

            var cutOff = new CutOff();
            
            cutOff.BranchId = branchId;
            cutOff.PreviousBallance = lastBallance;
            db.CutOffs.Add(cutOff);
            db.SaveChanges();
            return cutOff;
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

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                             SetCutOff(xx.BranchId.Value);

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
        public JsonResult CreateCashInFinal(CashIn cashflow,string log)
        {
            try
            {
                var cashInActive = cashflow;
                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                            SetCutOff(xx.BranchId.Value);


                if (cashflow.Id != 0)
                {
                    cashInActive = db.CashFlows.OfType<CashIn>().First(p => p.Id == cashflow.Id);

                    Subtitution(cashInActive, cashflow);

                }
                else
                {
                    db.CashFlows.Add(cashInActive);
                    //cashoutRegular.Date = DateTime.Now;
                    cashInActive.CutOffId = cutOff.Id;
                    cashInActive.UserId = usr;

                }


                cashInActive.SetToFinal();
                cashInActive.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " + User.Identity.Name + " | Final | " + log + "<br>" + cashInActive.LogNote;

                cashInActive.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashflow.Id, ex = "" });
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

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                            SetCutOff(xx.BranchId.Value);


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
        public JsonResult CreateCashoutRegularFinal(CashOutRegular cashflow, string log)
        {
            try
            {
                var cashout = cashflow;
                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                            SetCutOff(xx.BranchId.Value);


                if (cashflow.Id != 0)
                {
                   cashout = db.CashFlows.OfType<CashOutRegular>().First(p => p.Id == cashflow.Id);

                    Subtitution(cashout, cashflow);
      
                }
                else
                {
                    db.CashFlows.Add(cashout);
                    //cashoutRegular.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;
                 
                }


                cashout.SetToFinal();
                cashout.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " + User.Identity.Name + " | Final | " + log + "<br>" + cashout.LogNote;

                cashout.RegularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashflow.Id, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }


        }

        #endregion
        #region Cashout IRegular
        [HttpPost]
        public JsonResult CreateCashoutIrregularDraft(CashOutIrregular cashoutIregular)
        {

            try
            {

                var cashout = cashoutIregular;

                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                              SetCutOff(xx.BranchId.Value);


                if (cashoutIregular.Id != 0)
                {
                    cashout = db.CashFlows.OfType<CashOutIrregular>().First(p => p.Id == cashoutIregular.Id);

                    Subtitution(cashout, cashoutIregular);




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
                cashout.IrregularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashoutIregular.Id, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
            }


        }

        [HttpPost]
        public JsonResult CreateCashoutIrregularFinal(CashOutIrregular cashflow, string log)
        {
            try
            {
                var cashout = cashflow;
                var usr = User.Identity.GetUserId();
                var xx = db.Users.Find(usr);

                var cutOff = db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                              SetCutOff(xx.BranchId.Value);


                if (cashflow.Id != 0)
                {
                    cashout = db.CashFlows.OfType<CashOutIrregular>().First(p => p.Id == cashflow.Id);

                    Subtitution(cashout, cashflow);

                }
                else
                {
                    db.CashFlows.Add(cashout);
                    //cashoutRegular.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;

                }


                cashout.SetToFinal();
                cashout.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " + User.Identity.Name + " | Final | " + log + "<br>" + cashout.LogNote;

                cashout.IrregularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new { Success = 1, CashOutId = cashflow.Id, ex = "" });
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

            var cashOutIrregular = cashflow as CashOutIrregular;
            if (cashOutIrregular != null)
            {
                if (cashOutIrregular.State == StateCashFlow.Draft || cashOutIrregular.State == StateCashFlow.Revision)
                {
                    var y = new SelectList(Enum.GetValues(typeof(IrregularType)));
                    ViewBag.IrregularTypes = y;
          
                    return View("CashoutIrregular", cashOutIrregular);
                }
                return View("CashoutIrregularInfo", cashOutIrregular);
            }


            return View("Error");
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
