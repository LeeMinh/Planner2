using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class BanksController : BaseController
    {
        // GET: Banks
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData_Data()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                List<Bank> data = rrc_db.Banks.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }




        [HttpPost]
        public JsonResult Update_StaffOn(Bank obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                Bank a = rrc_db.Banks.Find(obj.ID);
                bool result = false;
                a.BankName = obj.BankName ?? a.BankName;
                a.ChuTaiKhoan = obj.ChuTaiKhoan ?? a.ChuTaiKhoan;
                a.ChiNhanh = obj.ChiNhanh ?? a.ChiNhanh;
                a.SoTaiKhoan = obj.SoTaiKhoan ?? a.SoTaiKhoan;
                

                 rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                result = true;
                return Json(new { result = result, data = a }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNew(Bank obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                bool? result = false;
                
                if (obj.ID == 0)
                {
                     rrc_db.Banks.Add(obj);
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
                Bank a = rrc_db.Banks.Find(id);
                bool result = false;

                if (a != null)
                {
                    rrc_db.Banks.Remove(a);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}