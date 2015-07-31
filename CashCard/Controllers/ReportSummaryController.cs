using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;

namespace CashCard.Controllers
{
    public class ReportSummaryController : Controller
    {
        //
        // GET: /ReportSummary/
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var dt = db.Branches.Select(p => p.Name).ToList();
            return View(dt);
        }

        public JsonResult FindData(DateTime startDate, DateTime endDate,int draw)
        {
            var json = new DataTablesJson();
            var tempEndDate = endDate.AddDays(1);

            var xx = db.Branches.Select(p => p.Name).ToList();

            string listBranch = "";
            foreach (var b in xx)
            {
                listBranch += b + ", ";
            }
            if (listBranch.Length > 2)
                listBranch = listBranch.Substring(0, listBranch.Length - 2);


            string sql = string.Format(@"select * from (
                        SELECT     rq.Quiz, b.Name, rd.SubTotal
                        FROM         RegularDetails AS rd INNER JOIN
                                              RegularQuizs AS rq ON rd.RegularQuizId = rq.Id INNER JOIN
                                              CutOffs AS c INNER JOIN
                                              CashFlows AS cf ON c.Id = cf.CutOffId ON rd.CashFlowId = cf.Id INNER JOIN
                                              Branches AS b ON c.BranchId = b.Id
                                              where cf.state = 4 and c.state=1 and c.[DateEnd] between '{0}' and '{1}'
                                              ) as tbl
                         pivot(sum(subtotal) for Name in ({2})) as abc", 
                         startDate.ToString("yyyy-MM-dd"),
                         tempEndDate.ToString("yyyy-MM-dd"),listBranch);


            json.draw = draw;
            json.recordsTotal = 0;
            IList<string[]> list = new List<string[]>();

            var con = db.Database.Connection;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var com = con.CreateCommand();
            com.CommandText = sql;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var oo = new string[] {reader.GetString(0),  reader.GetInt32(1).ToString()};
                list.Add(oo);

            }
            reader.Close();
            con.Close();


          
            json.data = list;
            return Json(json, JsonRequestBehavior.AllowGet);




        }
	}
}