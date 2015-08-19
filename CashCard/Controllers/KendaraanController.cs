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
    public class KendaraanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Kendaraan/
        public ActionResult Index()
        {
            return View(db.Kendaraan.ToList());
        }

        // GET: /Kendaraan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kendaraan kendaraan = db.Kendaraan.Find(id);
            if (kendaraan == null)
            {
                return HttpNotFound();
            }
            return View(kendaraan);
        }

        // GET: /Kendaraan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Kendaraan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,NoKendaraan,JenisKendaraan,EffStartDate,EffEndDate,Cc,MerkKendaraan,ThnPembuatan")] Kendaraan kendaraan)
        {
            if (ModelState.IsValid)
            {
                db.Kendaraan.Add(kendaraan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kendaraan);
        }

        // GET: /Kendaraan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kendaraan kendaraan = db.Kendaraan.Find(id);
            if (kendaraan == null)
            {
                return HttpNotFound();
            }
            return View(kendaraan);
        }

        // POST: /Kendaraan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,NoKendaraan,JenisKendaraan,EffStartDate,EffEndDate,Cc,MerkKendaraan,ThnPembuatan")] Kendaraan kendaraan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kendaraan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kendaraan);
        }

        // GET: /Kendaraan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kendaraan kendaraan = db.Kendaraan.Find(id);
            if (kendaraan == null)
            {
                return HttpNotFound();
            }
            return View(kendaraan);
        }

        // POST: /Kendaraan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kendaraan kendaraan = db.Kendaraan.Find(id);
            db.Kendaraan.Remove(kendaraan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult CheckNo(string noKendaraan)
        {
            var count =
                db.Kendaraan.Count(p => p.NoKendaraan.Equals(noKendaraan, StringComparison.InvariantCultureIgnoreCase));

            if (count == 0)
                return Json(true, JsonRequestBehavior.AllowGet);


            return Json(string.Format("{0} is already exist", noKendaraan), JsonRequestBehavior.AllowGet);
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
