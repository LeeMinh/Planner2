using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class ProfileController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult addDanhMuc()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult InfoDanhMuc(int ID)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.Categories.Where(v => v.CategoryRowID == ID).FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult addDanhMuc(int DM, string TYPECHOOSE)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                var nd = db.Users.Where(c => c.Id == nguoidung.Id).FirstOrDefault();
                nd.SoTien = nd.SoTien ?? 0;
                var data = db.Categories.Where(v => v.CategoryRowID == DM).FirstOrDefault();

                var dmlast = db.User_Category.Where(v => v.CategoryRowID == DM && v.UserName.ToUpper() == nguoidung.UserName.ToUpper()).FirstOrDefault();

                if (dmlast == null)
                {
                    dmlast = new User_Category();
                    dmlast.CategoryRowID = DM;
                    dmlast.UserName = nguoidung.UserName;
                    dmlast.UserID = nguoidung.Id;
                    dmlast.NgayHetHan = DateTime.Now;
                }
                double? Tien = 0;
                switch (TYPECHOOSE)
                {
                    case "Price7Day": Tien = data.Price7Day; dmlast.NgayHetHan =dmlast.NgayHetHan.Value.AddDays(7); break;
                    case "Price15Day": Tien = data.Price15Day; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddDays(15);  break;
                    case "Price30Day": Tien = data.Price30Day; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddDays(30);  break;
                    case "Price2Month": Tien = data.Price2Month; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddMonths(2);  break;
                    case "Price3Month": Tien = data.Price3Month; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddMonths(3);  break;
                    case "Price6Month": Tien = data.Price6Month; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddMonths(6);  break;
                    case "Price1Year": Tien = data.Price1Year; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddYears(1);  break;
                    case "Price2Year": Tien = data.Price2Year; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddYears(2);  break;
                    case "Price5Year": Tien = data.Price5Year; dmlast.NgayHetHan = dmlast.NgayHetHan.Value.AddYears(5);  break;

                    default:
                        break;
                }

                if (Tien> nd.SoTien)
                {
                    return Json("Số tiền bạn đang có không đủ để mua gói này, vui lòng nạp thêm", JsonRequestBehavior.AllowGet);
                }

                nd.SoTien = nd.SoTien.Value -(int)Tien.Value;
                if (dmlast.ID==0)
                {
                    db.User_Category.Add(dmlast);
                }
                db.SaveChanges();
                Session[Planner2.Controllers.LoginAuth.NameSession] = nd;
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult GetData_Data()
        {
            using (Planner2Entities rrc_db = new Planner2.Models.Planner2Entities())
            {
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                var data = rrc_db.User_Category.Where(v => (v.UserName + "").ToUpper() == nguoidung.UserName.ToUpper()).ToList();
                JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
        }
        public ActionResult DanhMuc()
        {

            return PartialView();

        }
    }
}