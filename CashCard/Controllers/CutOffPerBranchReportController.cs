using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
     [Authorize]
    public class CutOffPerBranchReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /CutOffPerBranchReport/
         public CutOffPerBranchReportController()
        {
            ViewBag.Menu = "MnReportPerBranch";
        }
        public ActionResult Index()
        {
            var dt = db.Branches.Select(oo => new { aaa = oo.Id, bbb = oo.Name}).ToList();

            var x = new SelectList(dt, "aaa", "bbb");
            ViewBag.Branches = x;
            return View();
        }

        public JsonResult FindData(DateTime startDate, DateTime endDate, int branchId, int draw, int start, int length, Order[] order )
        {
            var json = new DataTablesJson();
            json.draw = draw;
            json.recordsTotal = db.CutOffs.Count();

            var tempEndDate = endDate.AddDays(1);
            var dt =
                db.CutOffs.Where(p => p.DateEnd >= startDate && p.DateEnd < tempEndDate && p.BranchId == branchId && p.State == StateCutOff.End).OrderBy(p=>p.Id)
                    .Skip(start).Take(length).ToList();
            json.recordsFiltered =
                db.CutOffs.Count(p => p.DateEnd >= startDate && p.DateEnd < tempEndDate && p.BranchId == branchId && p.State == StateCutOff.End);

            var xxx = dt.Select(co => new[]
            {
                co.Branch.Name, co.State.ToString(), co.PreviousBallance.ToString("N0"), co.EndBallance.ToString("N0"), co.DateEnd.ToString("yyyy/MM/dd"), co.Id.ToString()
            }).ToList();

            json.data = xxx;
            return Json(json, JsonRequestBehavior.AllowGet);




        }
         public ActionResult GetReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CutOff cutoff = db.CutOffs.Find(id);
            if (cutoff == null)
            {
                return HttpNotFound();
            }
            cutoff.CashFlows = cutoff.CashFlows.Where(p => p.State == StateCashFlow.Approve).ToList();
            ViewBag.TotalCashIn = cutoff.CashFlows.OfType<CashIn>().Sum(p => p.Total).ToString("N0");
            ViewBag.TotalCashOutRegular = cutoff.CashFlows.OfType<CashOutRegular>().Sum(p => p.Total).ToString("N0");
            ViewBag.TotalCashOutIrregular = cutoff.CashFlows.OfType<CashOutIrregular>().Sum(p => p.Total).ToString("N0");
            return View(cutoff);
        }
	}
}