using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class CustomPagesController : BaseController
    {
        // GET: CustomPages
        public ActionResult Index(string NamePage,string TT="")
        {
            ViewBag.TT = TT;
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.CustomPages.Where(z => z.NamePage == NamePage).FirstOrDefault();
                return View(data);
            }
        }   public ActionResult IndexText(string NamePage,string TT="")
        {
            ViewBag.TT = TT;
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.CustomPages.Where(z => z.NamePage == NamePage).FirstOrDefault();
                return View(data);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(CustomPage item)
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.CustomPages.Where(z => z.NamePage == item.NamePage).FirstOrDefault();
                data.ContentPage = item.ContentPage;
                if (item.NamePage== "HEADER")
                {
                    Common.SettingData.HeaderHTML = data.ContentPage;
                    db.SaveChanges();
                    return RedirectToAction("IndexText", new { NamePage = item.NamePage, TT = "Đã lưu" });
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { NamePage = item.NamePage,TT="Đã lưu" });
            }
        }
    }
}