using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class ChoDuyetController :BaseController
    {
        // GET: ChoDuyet
        public ActionResult Index()
        {
         
                return View();

        }
        [HttpPost]
         public ActionResult GetListAllTask()
        {
             using (var db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(v => v.XetDuyet == 0).ToList();

                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        } 
        [HttpPost]
         public ActionResult OK(int[] data =null)
        {
             using (var db = new Planner2Entities())
            {
                foreach (var item in data)
                {
                    var row = db.MainTasks.Where(v => v.Id == item).FirstOrDefault();
                    row.XetDuyet = 1;
                }
                db.SaveChanges();
                JsonResult jsonResult = Json("", JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }[HttpPost]
         public ActionResult NG(int[] data = null)
        {
             using (var db = new Planner2Entities())
            {
                foreach (var item in data)
                {
                    var row = db.MainTasks.Where(v => v.Id == item).FirstOrDefault();
                    db.MainTasks.Remove(row);
                }
                db.SaveChanges();
                JsonResult jsonResult = Json("", JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
    }
}