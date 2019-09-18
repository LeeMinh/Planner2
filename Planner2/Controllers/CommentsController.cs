using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class CommentsController : BaseController
    {
        

        public class DsComment
        {
            public int ID { get; set; }
            public int TaskID { get; set; }
            public string NOIDUNG { get; set; }
            public string HoTen { get; set; }
            public string DienThoai { get; set; }
            public string DiaChi { get; set; }
            public string TieuDe { get; set; }
            public string MucDich { get; set; }
            public DateTime DateCreate { get; set; }
              public string Email { get; set; }

            public string TaskName { get; set; }
            public string CreatedBy { get; set; }
            public string SeoUrl { get; set; }
        }

       

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                var a = rrc_db.CommentTasks.Find(ID);
                bool result = false;

                if (a != null)
                {
                    rrc_db.CommentTasks.Remove(a);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetData_Data()
        {
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                string sql = @" SELECT  T1.[ID]
      ,[TaskID]
      ,[NOIDUNG]
      ,[HoTen]
      ,[DienThoai]
      ,[DiaChi]
      ,[TieuDe]
      ,[MucDich]
      ,[DateCreate]
  
      ,[Email],t2.TaskName,t2.CreatedBy,t2.SeoUrl
         FROM  [CommentTask] as t1 inner join MainTask as t2 on t1.TaskID=t2.Id 
  where t2.CreatedBy=''  or  1 = "+ nguoidung .SupperAdmin+ @"
 order by t1.DateCreate desc
 ";
                var data = rrc_db.Database.SqlQuery<DsComment>(sql).ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
    }
}