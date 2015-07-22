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
            var cutoffs = db.CutOffs.Include(c => c.Branch);
            return View(cutoffs.ToList());
        }

        // GET: /Cutoff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutOff cutoff = db.CutOffs.Find(id);
            if (cutoff == null)
            {
                return HttpNotFound();
            }
            return View(cutoff);
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
