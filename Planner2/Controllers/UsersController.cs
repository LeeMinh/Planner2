using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class UsersController : BaseController
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
                List<User> data = rrc_db.Users.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }




        [HttpPost]
        public JsonResult Update_StaffOn(User obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                User a = rrc_db.Users.Find(obj.Id);
                bool result = false;
                a.Email = obj.Email ?? a.Email;
                a.StaffName = obj.StaffName ?? a.StaffName;
                a.UserName = obj.UserName ?? a.UserName;
                a.Password = obj.Password ?? a.Password;
                a.SoTien = obj.SoTien ?? a.SoTien;
                a.Active = obj.Active ?? a.Active;
                a.SupperAdmin = obj.SupperAdmin ?? a.SupperAdmin;
                  rrc_db.SaveChanges();
                 result = true;
                return Json(new { result = result, data = a }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNew(User obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                bool? result = false;
                
                if (obj.Id == 0)
                {
                    obj.Password = LoginController.EncryptPassword(obj.Password);
                    rrc_db.Users.Add(obj);
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
                User a = rrc_db.Users.Find(id);
                bool result = false;

                if (a != null)
                {
                    rrc_db.Users.Remove(a);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}