using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;

namespace CashCard.Controllers
{
     [Authorize(Roles = "User")]
    public class CashCardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

         public CashCardController()
         {
             ViewBag.Menu = "MnCashflow";
         }
        //
        // GET: /CashCard/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var cashflows =
                db.CashCards.Include(p => p.CutOff)
                    .Where(p => p.UserId == userId && p.CutOff.State == StateCutOff.Start);
            var y = new SelectList(Enum.GetValues(typeof(CostCenter)));
            ViewBag.UserTypes = y;

            var x = new SelectList(Enum.GetValues(typeof(TypeOut)));
            ViewBag.TypeOuts = x;
            return View(cashflows.ToList());
          
        }
        public ActionResult CreateCashOut()
        {
            string strTypeOut = this.Request.QueryString["TypeOut"];
            string strUserType = this.Request.QueryString["UserType"];
            var typeOut = (TypeOut) Enum.Parse(typeof (TypeOut), strTypeOut);
            var userType = (CostCenter) Enum.Parse(typeof (CostCenter), strUserType);

            ViewBag.Quiz = new SelectList(db.Quizs.Where(p=>p.CostCenter==userType), "Id", "Info");
            ViewBag.QuizInfo = from x in db.Quizs select new { Id = x.Id, label1 = x.Note1Label, label2 = x.Note2Label };

            var cash = new CashOut();
            if (typeOut == TypeOut.Regular)
                return View("CreateCashOutRegular", cash);
            return null;
        }
	}
}