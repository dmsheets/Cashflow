using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace CashCard.Controllers
{
    [Authorize(Roles = "User")]
    public class CashCardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public CashCardController()
        {
            ViewBag.Menu = "MnCashflow";
        }

        //
        // GET: /CashCard/
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            IQueryable<Models.CashCard> cashflows =
                db.CashCards.Include(p => p.CutOff)
                    .Where(p => p.UserId == userId && p.CutOff.State == StateCutOff.Start);
            var y = new SelectList(Enum.GetValues(typeof (CostCenter)));
            ViewBag.UserTypes = y;

            var x = new SelectList(Enum.GetValues(typeof (TypeOut)));
            ViewBag.TypeOuts = x;
            return View(cashflows.ToList());
        }

        public ActionResult CreateCashOut(TypeOut? typeOut, CostCenter? userType)
        {
            var cash = new CashOut();
            cash.TypeOut = typeOut == null ? TypeOut.Regular : typeOut.Value;
            cash.CostCenter = userType == null ? CostCenter.Other : userType.Value;
           


            if (cash.TypeOut == TypeOut.Irregular)
            {
                var data = db.Quizs.Where(p => p.QuizGroup.GroupType == GroupType.Irregularaties);
                ViewBag.Quiz =new SelectList(data, "Id", "Info");
                   
                ViewBag.QuizInfo = from dt in data select new { dt.Id, label1 = dt.Note1Label, label2 = dt.Note2Label, label3 = dt.Note3Label, requiredAll = dt.RequiredAll };
                  
            }
            else
            {
                var data = db.Quizs.Where(
                            p => p.CostCenter == cash.CostCenter && p.QuizGroup.GroupType != GroupType.Irregularaties).ToList();

                ViewBag.Quiz = new SelectList(data, "Id", "Info");
                 

                ViewBag.QuizInfo = from dt in data select new { dt.Id, label1 = dt.Note1Label, label2 = dt.Note2Label, label3 = dt.Note3Label, requiredAll = dt.RequiredAll };
                    //from quiz in db.Quizs  join groups in db.RegularGroup on quiz.QuizGroupId equals groups.Id 
                    //where quiz.CostCenter == cash.CostCenter && groups.GroupType!=GroupType.Irregularaties
                    //               select new { quiz.Id, label1 = quiz.Note1Label, label2 = quiz.Note2Label, label3 = quiz.Note3Label, requiredAll = quiz.RequiredAll };
            }

            return View("CashOut", cash);
        }

        public ActionResult CashIn()
        {
            var cash = new CashIn();

            return View(cash);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CashCard cash = db.CashCards.Include(p => p.ImageDatas).FirstOrDefault(p => p.Id == id);
            
            if (cash == null)
            {
                return HttpNotFound();
            }


            var cashOutRegular = cash as CashOut;
            if (cashOutRegular != null)
            {
                if (cashOutRegular.State == StateCashCard.Draft || cashOutRegular.State == StateCashCard.Revision)
                {
                    if (cashOutRegular.TypeOut == TypeOut.Irregular)
                    {
                        var data = db.Quizs.Where(p => p.QuizGroup.GroupType == GroupType.Irregularaties);
                        ViewBag.Quiz = new SelectList(data, "Id", "Info");
                     
                        ViewBag.QuizInfo = from dt in data select new { dt.Id, label1 = dt.Note1Label, label2 = dt.Note2Label, label3 = dt.Note3Label, requiredAll = dt.RequiredAll };
                       
                    }
                    else
                    {
                        ViewBag.Quiz = new SelectList(db.Quizs.Where(p => p.CostCenter == cashOutRegular.CostCenter),
                            "Id",
                            "Info");
                        ViewBag.QuizInfo = from x in db.Quizs
                            where x.CostCenter == cashOutRegular.CostCenter
                            select new {x.Id, label1 = x.Note1Label, label2 = x.Note2Label};
                    }
                    return View("CashOut", cashOutRegular);
                }
                return View("CashOutInfo", cashOutRegular);
            }
            var cashIn = cash as CashIn;
            if (cashIn != null)
            {
                if (cashIn.State == StateCashCard.Draft || cashIn.State == StateCashCard.Revision)
                {
                    return View("CashIn", cashIn);
                }
                return View("CashInInfo", cashIn);
            }


            return View("Error");
        }

        [HttpPost]
        public JsonResult CreateCashoutRegularDraft(CashOut cashout)
        {
            try
            {
                CashOut cashout1 = cashout;

                string usr = User.Identity.GetUserId();
                ApplicationUser xx = db.Users.Find(usr);

                CutOff cutOff =
                    db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                    SetCutOff(xx.BranchId.Value);


                if (cashout.Id != 0)
                {
                    cashout = db.CashCards.OfType<CashOut>().First(p => p.Id == cashout.Id);

                    Subtitution(cashout, cashout1);
                }
                else
                {
                    db.CashCards.Add(cashout);
                    //CashOut.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;
                }
                //set subtotal and total

                cashout.SetToDraft();
                cashout.RegularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new {Success = 1, CashOutId = cashout.Id, ex = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Success = 0, ex = ex.Message});
            }
        }

        [HttpPost]
        public JsonResult CreateCashoutRegularFinal(CashOut cashout, string log)
        {
            try
            {
                CashOut cashout1 = cashout;
                string usr = User.Identity.GetUserId();
                ApplicationUser xx = db.Users.Find(usr);

                CutOff cutOff =
                    db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                    SetCutOff(xx.BranchId.Value);


                if (cashout.Id != 0)
                {
                    cashout = db.CashCards.OfType<CashOut>().First(p => p.Id == cashout.Id);

                    Subtitution(cashout, cashout1);
                }
                else
                {
                    db.CashCards.Add(cashout);
                    //CashOut.Date = DateTime.Now;
                    cashout.CutOffId = cutOff.Id;
                    cashout.UserId = usr;
                }


                cashout.SetToFinal();
                cashout.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " + User.Identity.Name + " | Final | " +
                                  log + "<br>" + cashout.LogNote;

                cashout.RegularDetails.ForEach(p => p.SetSubTotal());
                cashout.SetTotal();
                db.SaveChanges();


                return Json(new {Success = 1, CashOutId = cashout.Id, ex = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Success = 0, ex = ex.Message});
            }
        }

        private void Subtitution(CashOut cashoutDb, CashOut cashoutView)
        {
            cashoutDb.Date = cashoutView.Date;
            cashoutDb.Note = cashoutView.Note;

            //delete detail
            for (int i = cashoutDb.RegularDetails.Count - 1; i >= 0; i--)
            {
                int idReg = cashoutDb.RegularDetails[i].Id;

                CashOutDetail reg = cashoutView.RegularDetails.FirstOrDefault(p => p.Id == idReg);
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
                    CashOutDetail regDetail = cashoutView.RegularDetails[i];
                    CashOutDetail detail = cashoutDb.RegularDetails.First(p => p.Id == regDetail.Id);
                    detail.QuizId = regDetail.QuizId;
                    detail.Quiz = null;
                    detail.Note1 = regDetail.Note1;
                    detail.Note2 = regDetail.Note2;
                    detail.Qty = regDetail.Qty;
                    detail.Amount = regDetail.Amount;
                    detail.DateInfo = regDetail.DateInfo;
                }
            }

            //image list
            //delete detail
            for (int i = cashoutDb.ImageDatas.Count - 1; i >= 0; i--)
            {
                int idImg = cashoutDb.ImageDatas[i].Id;

                var img = cashoutView.ImageDatas.FirstOrDefault(p => p.Id == idImg);
                if (img == null)
                {
                    db.Entry(cashoutDb.ImageDatas[i]).State = EntityState.Deleted;
                   
                }
            }
            //add or update detail
            for (int i = 0; i < cashoutView.ImageDatas.Count; i++)
            {
                if (cashoutView.ImageDatas[i].Id == 0)
                {
                    cashoutDb.ImageDatas.Add(cashoutView.ImageDatas[i]);
                }
               
            }


        }

        public CutOff SetCutOff(int branchId)
        {
            List<CutOff> xx =
                db.CutOffs.Where(p => p.BranchId == branchId).OrderByDescending(p => p.DateStart).Take(1).ToList();
            int lastBallance = 0;
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

        [HttpPost]
        public JsonResult CreateCashInFinal(CashIn cashflow, string log)
        {
            try
            {
                CashIn cashInActive = cashflow;
                string usr = User.Identity.GetUserId();
                ApplicationUser xx = db.Users.Find(usr);

                CutOff cutOff =
                    db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                    SetCutOff(xx.BranchId.Value);


                if (cashflow.Id != 0)
                {
                    cashInActive = db.CashCards.OfType<CashIn>().First(p => p.Id == cashflow.Id);

                    Subtitution(cashInActive, cashflow);
                }
                else
                {
                    db.CashCards.Add(cashInActive);
                    //CashOut.Date = DateTime.Now;
                    cashInActive.CutOffId = cutOff.Id;
                    cashInActive.UserId = usr;
                }


                cashInActive.SetToFinal();
                cashInActive.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " + User.Identity.Name +
                                       " | Final | " + log + "<br>" + cashInActive.LogNote;

                cashInActive.SetTotal();
                db.SaveChanges();


                return Json(new {Success = 1, CashOutId = cashflow.Id, ex = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Success = 0, ex = ex.Message});
            }
        }

        private void Subtitution(CashIn cashInDb, CashIn cashInView)
        {
            cashInDb.Date = cashInView.Date;
            cashInDb.Note = cashInView.Note;

            //delete detail
            for (int i = cashInDb.CashInDetails.Count - 1; i >= 0; i--)
            {
                int idReg = cashInDb.CashInDetails[i].Id;

                CashInDetail reg = cashInView.CashInDetails.FirstOrDefault(p => p.Id == idReg);
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
                    CashInDetail regDetail = cashInView.CashInDetails[i];
                    CashInDetail detail = cashInDb.CashInDetails.First(p => p.Id == regDetail.Id);
                    detail.Note = regDetail.Note;
                    detail.Amount = regDetail.Amount;
                }
            }

            //image list
            //delete detail
            for (int i = cashInDb.ImageDatas.Count - 1; i >= 0; i--)
            {
                int idImg = cashInDb.ImageDatas[i].Id;

                var img = cashInView.ImageDatas.FirstOrDefault(p => p.Id == idImg);
                if (img == null)
                {
                    db.Entry(cashInDb.ImageDatas[i]).State = EntityState.Deleted;

                }
            }
            //add or update detail
            for (int i = 0; i < cashInView.ImageDatas.Count; i++)
            {
                if (cashInView.ImageDatas[i].Id == 0)
                {
                    cashInDb.ImageDatas.Add(cashInView.ImageDatas[i]);
                }

            }
        }

        [HttpPost]
        public JsonResult CreateCashInDraft(CashIn cashIn)
        {
            try
            {
                CashIn cashInActive = cashIn;

                string usr = User.Identity.GetUserId();
                ApplicationUser xx = db.Users.Find(usr);

                CutOff cutOff =
                    db.CutOffs.FirstOrDefault(p => p.BranchId == xx.BranchId && p.State == StateCutOff.Start) ??
                    SetCutOff(xx.BranchId.Value);

                if (cashIn.Id != 0)
                {
                    cashInActive = db.CashCards.OfType<CashIn>().First(p => p.Id == cashIn.Id);

                    Subtitution(cashInActive, cashIn);
                }
                else
                {
                    db.CashCards.Add(cashInActive);
                    //CashOut.Date = DateTime.Now;
                    cashInActive.CutOffId = cutOff.Id;
                    cashInActive.UserId = usr;
                }
                //set subtotal and total

                cashInActive.SetToDraft();

                cashInActive.SetTotal();
                db.SaveChanges();


                return Json(new {Success = 1, CashOutId = cashIn.Id, ex = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Success = 0, ex = ex.Message});
            }
        }
    }
}