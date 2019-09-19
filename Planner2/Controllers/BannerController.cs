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
                return RedirectToAction("index");
            }
        }

        public List<string> SubmitFile(List<HttpPostedFileBase> files)
        {

            List<string> FileUp = new List<string>();
            if (files == null)
            {
                return FileUp;
            }
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                if (file == null)
                {
                    continue;
                }
                string fname;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = DateTime.Now.ToString("ddhhmmss") + "_" + file.FileName;
                }
                string ext = Path.GetExtension(fname);
                Guid g = Guid.NewGuid();
                string newnane = "/FileUpload/" + fname;
                FileUp.Add(newnane);

                fname = Path.Combine(Server.MapPath("~/FileUpload/"), fname);
                file.SaveAs(fname);
            }

            return FileUp;
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