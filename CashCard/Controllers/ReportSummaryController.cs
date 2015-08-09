using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using CashCard.Models;

namespace CashCard.Controllers
{
     [Authorize]
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

        [HttpGet]
        public JsonResult FindData1(DateTime startDate, DateTime endDate, int draw)
        {
            var json = new DataTablesJson();
            var tempEndDate = endDate.AddDays(1);

            var xx = db.Branches.Select(p => p.Name).ToList();

            string listBranch = "";
            foreach (var b in xx)
            {
                listBranch += "[" + b + "], ";
            }
            if (listBranch.Length > 2)
                listBranch = listBranch.Substring(0, listBranch.Length - 2);



            var sql1 = string.Format(@"select * from(
SELECT 
  case QuizGroups.GroupType
			when 0 then 'Promosi'
			when 1 then 'Keperluan Kantor'
			when 2 then 'Keperluan Operasional'
			when 3 then 'Entertainment'
			when 4 then 'Komunikasi'
			when 5 then 'Hotel'
			when 6 then 'Irregularity'
			when 7 then 'Transportasi'
			when 8 then 'Sob'
			else '-n/a-'  end   as GroupType
, 
Branches.Name as Branch, CashOutDetails.SubTotal
FROM         CutOffs INNER JOIN
                      CashCards ON CutOffs.Id = CashCards.CutOffId INNER JOIN
                      Branches ON CutOffs.BranchId = Branches.Id INNER JOIN
                      CashOutDetails ON CashCards.Id = CashOutDetails.CashOutId INNER JOIN
                      Quizs ON CashOutDetails.QuizId = Quizs.Id INNER JOIN
                      QuizGroups ON Quizs.QuizGroupId = QuizGroups.Id
where CashCards.state = 4 and CutOffs.state=1 and CutOffs.[DateEnd] between '{0}' and '{1}'
) as tbl
pivot(sum(SubTotal) for Branch in 
({2} ) ) as sumofbranch", startDate.ToString("yyyy-MM-dd"),
                tempEndDate.ToString("yyyy-MM-dd"), listBranch);





            json.draw = draw;
            json.recordsTotal = 0;
            IList<string[]> list = new List<string[]>();

            var con = db.Database.Connection;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var com = con.CreateCommand();
            com.CommandText = sql1;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var oo = new string[xx.Count + 1];
                oo[0] = reader.GetString(0);
                for (int i = 1; i < xx.Count + 1; i++)
                {
                    var temp = reader.GetValue(i) is DBNull ? "0" : reader.GetInt32(i).ToString();

                    oo[i] = temp;
                }

                list.Add(oo);

            }
            reader.Close();
            con.Close();



            json.data = list;
            return Json(json, JsonRequestBehavior.AllowGet);




        }

        [HttpGet]
        public JsonResult FindData2(DateTime startDate, DateTime endDate, int draw)
        {
            var json = new DataTablesJson();
            var tempEndDate = endDate.AddDays(1);

            //var xx = db.Branches.Select(p => p.Name).ToList();

          
          


          

            var sql2 = string.Format(@"
select * from(
SELECT 
  case QuizGroups.GroupType
			when 0 then 'Promosi'
			when 1 then 'Keperluan Kantor'
			when 2 then 'Keperluan Operasional'
			when 3 then 'Entertainment'
			when 4 then 'Komunikasi'
			when 5 then 'Hotel'
			when 6 then 'Irregularity'
			when 7 then 'Transportasi'
			when 8 then 'Sob'
			else '-n/a-'  end   as GroupType
, 
Branches.Name as Branch, CashOutDetails.SubTotal
FROM         CutOffs INNER JOIN
                      CashCards ON CutOffs.Id = CashCards.CutOffId INNER JOIN
                      Branches ON CutOffs.BranchId = Branches.Id INNER JOIN
                      CashOutDetails ON CashCards.Id = CashOutDetails.CashOutId INNER JOIN
                      Quizs ON CashOutDetails.QuizId = Quizs.Id INNER JOIN
                      QuizGroups ON Quizs.QuizGroupId = QuizGroups.Id
where CashCards.state = 4 and CutOffs.state=1 and CutOffs.[DateEnd] between '{0}' and '{1}'
) as tbl
pivot(sum(SubTotal) for GroupType in 
([Promosi],[Keperluan Kantor],[Keperluan Operasional],
[Entertainment], [Komunikasi], [Hotel], [Irregularity],
[Transportasi],  [Sob] ) ) as sumofbranch", startDate.ToString("yyyy-MM-dd"),
                tempEndDate.ToString("yyyy-MM-dd"));




            json.draw = draw;
            json.recordsTotal = 0;
            IList<string[]> list = new List<string[]>();

            var con = db.Database.Connection;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var com = con.CreateCommand();
            com.CommandText = sql2;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var oo = new string[10];
                oo[0] = reader.GetString(0);

                for (int i = 1; i < 10; i++)
                {
                    var temp = reader.GetValue(i) is DBNull ? "0" : reader.GetInt32(i).ToString();
                    oo[i] = temp;
                }
                    

                   
                

                list.Add(oo);

            }
            reader.Close();
            con.Close();



            json.data = list;
            return Json(json, JsonRequestBehavior.AllowGet);




        }
        [HttpGet]
        public JsonResult FindData3(DateTime startDate, DateTime endDate, int draw)
        {
            var json = new DataTablesJson();
            var tempEndDate = endDate.AddDays(1);

            //var xx = db.Branches.Select(p => p.Name).ToList();







            var sql2 = string.Format(@"
select QuizGroups.AccountNo, QuizGroups.AccountDesription, sum(CashOutDetails.Subtotal ) as summary
FROM         CutOffs INNER JOIN
                      CashCards ON CutOffs.Id = CashCards.CutOffId INNER JOIN
                      Branches ON CutOffs.BranchId = Branches.Id INNER JOIN
                      CashOutDetails ON CashCards.Id = CashOutDetails.CashOutId INNER JOIN
                      Quizs ON CashOutDetails.QuizId = Quizs.Id INNER JOIN
                      QuizGroups ON Quizs.QuizGroupId = QuizGroups.Id
where CashCards.state = 4 and CutOffs.state=1 and CutOffs.[DateEnd] between '{0}' and '{1}'
group by QuizGroups.AccountNo, QuizGroups.AccountDesription", startDate.ToString("yyyy-MM-dd"),
                tempEndDate.ToString("yyyy-MM-dd"));




            json.draw = draw;
            json.recordsTotal = 0;
            IList<string[]> list = new List<string[]>();

            var con = db.Database.Connection;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var com = con.CreateCommand();
            com.CommandText = sql2;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var oo = new string[3];
                oo[0] = reader.GetString(0);

                oo[1] = reader.GetString(1);
                oo[2] = reader.GetValue(2).ToString();





                list.Add(oo);

            }
            reader.Close();
            con.Close();



            json.data = list;
            return Json(json, JsonRequestBehavior.AllowGet);




        }
	}
}