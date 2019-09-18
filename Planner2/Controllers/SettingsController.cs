using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class SettingsController : BaseController
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData_Data()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                List<SettingData> data = rrc_db.SettingDatas.ToList();
                 JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }

   

        [HttpPost]
        public JsonResult Update_StaffOn(SettingData obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                SettingData a = rrc_db.SettingDatas.Find(obj.KeyID);
                bool result = false;
                a.Value = obj.Value;
                if (!string.IsNullOrEmpty(obj.Type))
                {
                    a.Type = obj.Type;
                }
                rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                result = true;
                return Json(new { result = result, data = a }, JsonRequestBehavior.AllowGet);
            }
        }
 
    }
}