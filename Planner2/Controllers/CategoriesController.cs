using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class CategoriesController : BaseController
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
                List<Category> data = rrc_db.Categories.ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }




        [HttpPost]
        public JsonResult Update_StaffOn(Category obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                Category a = rrc_db.Categories.Find(obj.CategoryRowID);
                bool result = false;
                a.Description = obj.Description ?? a.Description;
                a.ParentID = obj.ParentID ?? a.ParentID;
                a.Menu = obj.Menu ?? a.Menu;
                a.CategoryName = obj.CategoryName ?? a.CategoryName;
                a.STT = obj.STT ?? a.STT;
                a.onePrice = obj.onePrice ?? a.onePrice;
                a.Price7Day = obj.Price7Day ?? a.Price7Day;
                a.Price15Day = obj.Price15Day ?? a.Price15Day;
                a.Price30Day = obj.Price30Day ?? a.Price30Day;
                a.Price2Month = obj.Price2Month ?? a.Price2Month;
                a.Price3Month = obj.Price3Month ?? a.Price3Month;
                a.Price6Month = obj.Price6Month ?? a.Price6Month;
                a.Price1Year = obj.Price1Year ?? a.Price1Year;
                a.Price2Year = obj.Price2Year ?? a.Price2Year;
                a.Price5Year = obj.Price5Year ?? a.Price5Year;

                a.SeoUrl = Extentions.SeoUrl.SeoURL(a.CategoryName);
                rrc_db.SaveChanges();
                MvcApplication.ReloadSetting();
                result = true;
                return Json(new { result = result, data = a }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNew(Category obj)
        {
            using (Planner2Entities rrc_db = new Planner2Entities())
            {
                bool? result = false;
                if (obj.STT == null)
                {
                    obj.STT = 9999;
                }
                if (obj.CategoryRowID == 0)
                {
                    obj.SeoUrl = Extentions.SeoUrl.SeoURL(obj.CategoryName);
                    rrc_db.Categories.Add(obj);
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
                Category a = rrc_db.Categories.Find(id);
                bool result = false;

                if (a != null)
                {
                    rrc_db.Categories.Remove(a);
                    rrc_db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


    }
}