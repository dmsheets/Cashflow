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
            var now = DateTime.Now.Date;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = new DateTime(now.Year, now.Month + 1, 1);
            var cutoffs =
                db.CutOffs.Include(c => c.Branch)
                    .Where(p => p.State == StateCutOff.Start || (startDate <= p.DateStart && p.DateStart <= endDate));
            return View(cutoffs.ToList());
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
                    throw new Exception("Make sure all Cash flow have been validated as Approve or Reject");
                }

                if (cutOff.State == StateCutOff.End)
                {
                    
                }
                cutOff.SetEndBallance();
               cutOff.SetEndState();
                
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
