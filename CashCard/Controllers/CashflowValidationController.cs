﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CashCard.Controllers
{
    public class CashflowValidationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CashflowValidationController()
        {
            ViewBag.Menu = "MnCashflowValidation";
        }
        // GET: /WorkflowValidation/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var branchId = db.Users.Find(userId).BranchId;

            var cashflows = db.CashFlows.Include(c => c.CutOff).Where(p => p.CutOff.BranchId == branchId && p.CutOff.State == StateCutOff.Start);
            return View(cashflows.ToList());
        }

        // GET: /WorkflowValidation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cashflow = db.CashFlows.FirstOrDefault(p => p.Id == id);
            if (cashflow == null)
            {
                return HttpNotFound();
            }
            var cashoutRegular = cashflow as CashOutRegular;
            if (cashoutRegular != null)
            {
                 return View("CashoutRegular",cashoutRegular);
            }
            var cashoutIrregular = cashflow as CashOutIrregular;
            if (cashoutIrregular != null)
            {
                return View("CashoutIrregular", cashoutIrregular);
            }
            var cashin = cashflow as CashIn;
            if (cashin != null)
            {
                return View("Cashin", cashin);
            }
            return View("Error");

        }

        [HttpPost]
        public JsonResult SetState(int id, StateCashFlow state, string log)
        {
            try
            {
                var ch = db.CashFlows.Find(id);
                switch (state)
                {
                    case StateCashFlow.Revision:
                        ch.SetToRevision();
                        break;
                    case StateCashFlow.Reject:
                        ch.SetToReject();
                        break;
                    case StateCashFlow.Approve:
                        ch.SetToApprove();
                        break;
                    default:
                        throw new Exception("State not valid");
                }
                ch.SuperVisorId = User.Identity.GetUserId();
                ch.LogNote = DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " | " +  User.Identity.Name + " | " + state + " | " + log + "<br>" + ch.LogNote;
                db.SaveChanges();
                return Json(new { Success = 1, CashOutId = ch.Id, ex = "" });

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
