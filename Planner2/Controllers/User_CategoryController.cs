using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class User_CategoryController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetData_Data()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                List<User_Category> data = rrc_db.User_Category.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
        
        [HttpPost]
        public JsonResult GetUserName()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
              var data = rrc_db.Users.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
         [HttpPost]
        public JsonResult GeCategory()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
              var data = rrc_db.Categories.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }




        [HttpPost]
        public JsonResult Update_StaffOn(User_Category obj,DateTime? NgayHetHan= null)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                User_Category a = rrc_db.User_Category.Find(obj.ID);
                bool result = false;
                a.NgayHetHan = obj.NgayHetHan ?? a.NgayHetHan;
             
                rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                result = true;
                return Json(new { result = result, data = a }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNew(User_Category obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                bool? result = false;
                
                if (obj.ID == 0)
                {
                     rrc_db.User_Category.Add(obj);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(new { result = result, obj = obj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                User_Category a = rrc_db.User_Category.Find(id);
                bool result = false;

                if (a != null)
                {
                    rrc_db.User_Category.Remove(a);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


    }
}