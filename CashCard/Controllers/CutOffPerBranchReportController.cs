using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
    public class CutOffPerBranchReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /CutOffPerBranchReport/
        public ActionResult Index()
        {
            var dt = db.Branches.Select(oo => new { aaa = oo.Id, bbb = oo.Name}).ToList();

            var x = new SelectList(dt, "aaa", "bbb");
            ViewBag.Branches = x;
            return View();
        }
	}
}