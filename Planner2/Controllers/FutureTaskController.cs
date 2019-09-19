using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planner2.Models;

namespace Planner2.Controllers
{
    public class FutureTaskController : BaseController
    {
        // GET: FutureTask
        public class Min_MainTask
        {
            public int Id { get; set; }
            public Nullable<int> ParentID { get; set; }
            public string TaskName { get; set; }
            public Nullable<System.DateTime> StartDate { get; set; }
            public Nullable<System.DateTime> DueDate { get; set; }
            public string TaskChecker { get; set; }
            public string Authorized { get; set; }
            public string Master { get; set; }
            public string Status { get; set; }
            public string TaskAssigner { get; set; }
            public Nullable<int> Days { get; set; }
            public string Description { get; set; }
            public string Priority { get; set; }
            public System.DateTime Created { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> DateFinish { get; set; }
            public Nullable<System.DateTime> DateEnd { get; set; }
            public string AttendPerson { get; set; }
            public string Dept { get; set; }

        }


        public ActionResult Index(int TaskID = 0)
        {
            ViewBag.TaskID = TaskID;
            if (TaskID == 0)
            {
                return View();
            }
            else
            {
                return PartialView();
            }
        }
        public ActionResult All()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            if (nguoidung.SupperAdmin!=1)
            {
                return Content("bạn không có quyền");
            }
            return View();
           
        }

        List<Min_MainTask> listMyAllTask = new List<Min_MainTask>();
        public ActionResult GetListAllTask()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (var db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(v=>v.Page==false && v.CreatedBy== nguoidung.UserName).ToList();//.Where(z => !string.IsNullOrEmpty(z.AttendPerson) && z.AttendPerson.ToUpper().Contains(nguoidung.UserName.ToUpper())).OrderByDescending(z=>z.NgayDang).ToList();

                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
        [HttpPost]
        public ActionResult all_data()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (var db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(z=>z.Page==false).ToList();//.Where(z => !string.IsNullOrEmpty(z.AttendPerson) && z.AttendPerson.ToUpper().Contains(nguoidung.UserName.ToUpper())).OrderByDescending(z=>z.NgayDang).ToList();

                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }

     

       

          public void AddHistory(int TaskID, string NoiDung = "")
        {
            if (NoiDung == "")
            {
                return;
            }
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];

            History his = new History();
            his.DATECREATE = DateTime.Now;
            his.StaffID = nguoidung.UserName;
            his.TaskID = TaskID;

            his.NOIDUNG = NoiDung;
            using (Planner2Entities db = new Planner2Entities())
            {
                db.Histories.Add(his);
                db.SaveChanges();
            }
        }
      

     }
}