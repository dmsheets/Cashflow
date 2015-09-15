using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using WebGrease.Css.Extensions;

namespace CashCard.Controllers

{
    [Authorize(Roles = "Officer")]
    public class CutoffController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CutoffController()
        {
            ViewBag.Menu = "MnCutoff";
        }
        // GET: /Cutoff/
        public ActionResult Index()
        {
            
            var dt = DateTime.Now.AddMonths(-2); 
           
            var cutoffs =
                db.CutOffs.Include(c => c.Branch).Include(p => p.CashCards)
                    .Where(p => dt <= p.DateEnd || p.State == StateCutOff.Open).OrderByDescending(p => p.Id);
            var list = cutoffs.ToList();
            foreach (var p in list)
            {
                if (p.State == StateCutOff.Open)
                {
                    p.SetEndBallance();
                }
            }
        
            return View(list);
        }

        // GET: /Cutoff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutOff cutoff = db.CutOffs.Include(p => p.CashCards).FirstOrDefault(p => p.Id == id);
            if (cutoff == null)
            {
                return HttpNotFound();
            }
            cutoff.SetEndBallance();
            return View(cutoff);
        }

        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cashCard = db.CashCards.Find(id);
            if (cashCard == null)
            {
                return HttpNotFound();
            }

            var cOut = cashCard as CashOut;
            if (cOut != null)
                return View("CashOutInfo",cOut);
            var cIn = cashCard as CashIn;
            if (cIn != null)
                return View("CashInInfo",cIn);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [HttpPost]
        public JsonResult CutOff(int id)
        {

            try
            {

                
                var cutOff = db.CutOffs.Find(id);
               
                var count =
                    cutOff.CashCards.Count(p => !(p.State == StateCashCard.Reject || p.State == StateCashCard.Approve));
                if (count > 0)
                {
                    throw new Exception("Make sure all Cash card have been validated as Approve or Reject");
                }

                if (cutOff.State == StateCutOff.Open)
                {
                    cutOff.SetEndBallance();
                    cutOff.SetEndState();
                }


                db.SaveChanges();
              
              


                return Json(new { Success = 1,  ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message });
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
