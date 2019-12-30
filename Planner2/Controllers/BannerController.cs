using Planner2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class BannerController : BaseController
    {
        // GET: Banner
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData_Data()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                List<Models.SettingData> data = rrc_db.SettingDatas.Where(z => z.Type == "BANNER" || z.Type == "LOGO").ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }  [HttpPost]
        public JsonResult XoaAnh(string ID)
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                var data = rrc_db.SettingDatas.Where(v => v.KeyID == ID).FirstOrDefault();
                data.Value = "";
                rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                return Json("",JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UploadFILEBanner(string BannerID, HttpPostedFileBase File_Banner=null)
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                var file = SubmitFile(new List<HttpPostedFileBase> { File_Banner });
                var data = rrc_db.SettingDatas.Where(v => v.KeyID == BannerID).FirstOrDefault();
                data.Value = file.FirstOrDefault();
                rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                return RedirectToAction("index");
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