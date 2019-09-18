using PagedList;
using Planner2.Extentions;
using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(int page = 1, string sort = "df", string ChuDe = "")
        {
            ViewBag.ChuDe = ChuDe;
            ViewBag.page = page;
            ViewBag.sort = sort;
            return View();
        }
        public ActionResult Timkiem()
        {
            return PartialView();
        }
        public ActionResult HomePage()
        {
            return View("HomePage");
        }
        public ActionResult HomePage_Slide(string SeoUrl)
        {
            if (string.IsNullOrEmpty(SeoUrl))
            {
                return Content("");
            }
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var cd = db.Categories.Where(v => v.SeoUrl == SeoUrl).FirstOrDefault();
                if (cd == null)
                {
                    return Content("");

                }
                ViewBag.TitleChuDe = cd.CategoryName;
                ViewBag.SeoUrl = cd.SeoUrl;
                string sql = $"select TOP 10 t1.* from MainTask as t1 inner join MainTask_ChuDe as t2 on t1.Id=t2.TaskID and t2.CategoryRowID={cd.CategoryRowID} ORDER BY T1.NgayDang DESC";
                var data = db.Database.SqlQuery<MainTask>(sql).ToList();
                if (data.Count == 0)
                {
                    return Content("");
                }
                return PartialView(data);
            }

        }
        public ActionResult HomePage_ChuDe(string SeoUrl)
        {
            if (string.IsNullOrEmpty(SeoUrl))
            {
                return Content("");
            }
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var cd = db.Categories.Where(v => v.SeoUrl == SeoUrl).FirstOrDefault();
                if (cd == null)
                {
                    return Content("");

                }
                ViewBag.TitleChuDe = cd.CategoryName;
                ViewBag.SeoUrl = cd.SeoUrl;
                string sql = $"select TOP 10 t1.* from MainTask as t1 inner join MainTask_ChuDe as t2 on t1.Id=t2.TaskID and t2.CategoryRowID={cd.CategoryRowID} ORDER BY T1.NgayDang DESC";
                var data = db.Database.SqlQuery<MainTask>(sql).ToList();
                if (data.Count == 0)
                {
                    return Content("");
                }
                return PartialView(data);
            }

        }
       public ActionResult HomePage_Table(string SeoUrl)
        {
            if (string.IsNullOrEmpty(SeoUrl))
            {
                return Content("");
            }
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var cd = db.Categories.Where(v => v.SeoUrl == SeoUrl).FirstOrDefault();
                if (cd == null)
                {
                    return Content("");

                }
                ViewBag.TitleChuDe = cd.CategoryName;
                ViewBag.SeoUrl = cd.SeoUrl;
                string sql = $"select TOP 10 t1.* from MainTask as t1 inner join MainTask_ChuDe as t2 on t1.Id=t2.TaskID and t2.CategoryRowID={cd.CategoryRowID} ORDER BY T1.NgayDang DESC";
                var data = db.Database.SqlQuery<MainTask>(sql).ToList();
                if (data.Count == 0)
                {
                    return Content("");
                }
                return PartialView(data);
            }

        }
        public ActionResult Contact()
        {

            return View();
        }

        public partial class _breadcrums
        {
            public int CategoryRowID { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
            public Nullable<int> ParentID { get; set; }
            public string SeoUrl { get; set; }
            public string Menu { get; set; }
            public Nullable<int> STT { get; set; }

        }
        public ActionResult breadcrums(string ChuDe)
        {
            if (string.IsNullOrEmpty(ChuDe))
            {
                return Content("");
            }

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.Database.SqlQuery<_breadcrums>(@"WITH ABC AS( SELECT   [CategoryRowID]
      ,[CategoryName]
      ,[Description]
       ,[SeoUrl],1 as [ParentID] ,[ParentID]  as [Parent]
  FROM  [Categories] where  '" + ChuDe + @"' in (SeoUrl,CONVERT(nvarchar,[CategoryRowID]))
  UNION  ALL
  SELECT T2.[CategoryRowID]
      ,T2.[CategoryName]
      ,T2.[Description]
 	  ,T2.[SeoUrl] 
	  ,(T1.[ParentID]+1) AS [ParentID] ,T2.[ParentID]  as [Parent]
     FROM ABC AS T1 INNER JOIN [Categories] AS T2 ON T2.[CategoryRowID]=T1.[Parent]
  )
  SELECT *,'' SeoUrl,''  Menu,0 as STT from abc").ToList();
                return PartialView(data);
            }
        }
        public ActionResult Info(string ID = "")
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var data = db.MainTasks.Where(v => v.SeoUrl == ID).FirstOrDefault();
                var cd = db.MainTask_ChuDe.Where(v => v.TaskID == data.Id).Select(z => z.CategoryRowID).ToList();
                ViewBag.TinLienQuan = db.MainTasks.WhereChuDe(cd, db).OrderByDescending(z => z.NgayDang).Skip(0).Take(10).ToList();
                ViewBag.TitleChuDe = db.Categories.Where(c => c.CategoryRowID == cd.FirstOrDefault()).Select(z => z.CategoryName).FirstOrDefault();
                ViewBag.ChuDe = cd.FirstOrDefault();
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                nguoidung = nguoidung ?? new User();
                if (data == null || (data != null && data.Status != Common.ConstTrangThai.CongKhai && data.CreatedBy != nguoidung.UserName))
                {
                    if (nguoidung.SupperAdmin != 1)
                    {
                        return Content("<h1>Không tìm thấy trang......</h1>");
                    }
                }
                data.Viewer = data.Viewer ?? 0;
                if (data.Status == Common.ConstTrangThai.CongKhai)
                {
                    data.Viewer++;
                }
                db.SaveChanges();
                if (data.Page == true)
                {
                    ViewBag.Page = 1;
                    return View("page", data);
                }

                return View(data);
            }
        }
        public ActionResult Search(MainTask item, string key, string DienTich, int? page, string sort)
        {

            IQueryable<MainTask> data;
            List<MainTask> Localdata;
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                int pageNumber = (page ?? 1);
                int pageSize = 10;
                if (pageNumber != 1)
                {
                    Localdata = (List<MainTask>)Session["dataSearch"];
                    goto End;
                }

                data = db.MainTasks.Where(v => v.Page == false && v.Status == Common.ConstTrangThai.CongKhai);
                if (!string.IsNullOrEmpty(key))
                    data = data.Where(z => z.TaskName != null && z.TaskName.ToUpper().Contains(key.ToUpper()));

                if (!string.IsNullOrEmpty(item.Map_LoaiBatDongSan))
                    data = data.Where(z => z.Map_LoaiBatDongSan == item.Map_LoaiBatDongSan);

                if (item.KhuVuc_TP.HasValue)
                    data = data.Where(z => z.KhuVuc_TP == item.KhuVuc_TP);


                if (item.KhuVuc_Huyen.HasValue)
                    data = data.Where(z => z.KhuVuc_Huyen == item.KhuVuc_Huyen);

                if (item.KhuVuc_Xa.HasValue)
                    data = data.Where(z => z.KhuVuc_Xa == item.KhuVuc_Xa);

                // dien tich
                if (!string.IsNullOrEmpty(DienTich))
                {
                    int start = 0;
                    int end = 0;
                    int.TryParse(DienTich.Split('-').FirstOrDefault(), out start);
                    int.TryParse(DienTich.Split('-').LastOrDefault(), out end);
                    if (start != 0)
                        data = data.Where(z => z.DienTich >= start);

                    if (end != 0)
                        data = data.Where(z => z.DienTich <= end);
                }

                // muc gia
                if (item.Gia.HasValue)
                {

                    switch (item.Gia)
                    {
                        case 1: data = data.Where(z => z.Gia == null || z.Gia == 0); break;
                        case 2: data = data.Where(z => z.Gia < 500 && z.TyGia == Planner2.Common.ConstVal.TrieuDong); break;
                        case 3: data = data.Where(z => z.Gia >= 500 && z.Gia < 1000 && z.TyGia == Planner2.Common.ConstVal.TrieuDong); break;
                        case 4: data = data.Where(z => z.Gia >= 1 && z.Gia < 3 && z.TyGia == Planner2.Common.ConstVal.TyDong); break;
                        case 5: data = data.Where(z => z.Gia >= 3 && z.Gia < 5 && z.TyGia == Planner2.Common.ConstVal.TyDong); break;
                        case 6: data = data.Where(z => z.Gia >= 5); break;
                    }
                }


                if (!string.IsNullOrEmpty(item.Map_Huong))
                    data = data.Where(z => z.Map_Huong == item.Map_Huong);

                if (item.Map_SoPhongNgu.HasValue)
                    data = data.Where(z => z.Map_SoPhongNgu >= item.Map_SoPhongNgu);

                if (!string.IsNullOrEmpty(item.Map_DuAn))
                    data = data.Where(z => z.Map_DuAn == item.Map_DuAn);


                Localdata = data.ToList();
                Session["dataSearch"] = Localdata;


            End: ViewBag.sort = sort;
                ViewBag.page = page;
                ViewBag.key = key;
                ViewBag.DienTich = DienTich;
                ViewBag.itemSearch = item;
                ViewBag.action = "Search";
                ViewBag.KhuVuc_TP = db.ThanhPhoes.Where(v => v.MaThanhPho == item.KhuVuc_TP).Select(z => z.TenThanhPho).FirstOrDefault();
                ViewBag.KhuVuc_Huyen = db.ThanhPhoes.Where(v => v.MaThanhPho == item.KhuVuc_Huyen).Select(z => z.QuanHuyen).FirstOrDefault();
                ViewBag.KhuVuc_Xa = db.ThanhPhoes.Where(v => v.MaThanhPho == item.KhuVuc_Xa).Select(z => z.PhuongXa).FirstOrDefault();

                ViewBag.TitleChuDe = "Kết quả tìm kiếm";
                Localdata = SortGrid(Localdata, sort);
                return View(Localdata.ToPagedList(pageNumber, pageSize));
            }
        }

        public List<MainTask> SortGrid(List<MainTask> data, string sort = "")
        {
            switch (sort)
            {
                case "dd": data = data.OrderByDescending(z => z.NgayDang).ToList(); break;
                case "du": data = data.OrderBy(z => z.NgayDang).ToList(); break;
                case "pd": data = data.OrderByDescending(z => z.Gia).ToList(); break;
                case "pu": data = data.OrderBy(z => z.Gia).ToList(); break;
                default:
                    data = data.OrderBy(z => z.NgayDang).ToList(); break;
            }

            return data;
        }
        public IQueryable<MainTask> SortGrid(IQueryable<MainTask> data, string sort = "")
        {
            switch (sort)
            {
                case "dd": data = data.OrderByDescending(z => z.NgayDang); break;
                case "du": data = data.OrderBy(z => z.NgayDang); break;
                case "pd": data = data.OrderByDescending(z => z.Gia); break;
                case "pu": data = data.OrderBy(z => z.Gia); break;
                default:
                    data = data.OrderBy(z => z.NgayDang); break;
            }

            return data;
        }
        public ActionResult Grid(String ChuDe, int page = 1, string sort = "", string acton = "")
        {

            ViewBag.TitleChuDe = "Bất động sản";
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                IQueryable<MainTask> data = db.MainTasks.Where(v => v.Page == false && v.Status == Common.ConstTrangThai.CongKhai);
                if (!string.IsNullOrEmpty(ChuDe))
                {
                    var Categories = db.Categories.Where(v => v.SeoUrl == ChuDe).FirstOrDefault();
                    if (Categories != null)
                    {
                        data = data.WhereChuDe(Categories.CategoryRowID, db);
                        ViewBag.TitleChuDe = Categories.CategoryName;
                    }


                }


                int pageSize = 10;
                int pageNumber = page;
                ViewBag.ChuDe = ChuDe;
                ViewBag.sort = sort;
                ViewBag.page = page;
                ViewBag.acton = acton;
                data = SortGrid(data, sort);
                var dl = data.ToPagedList(pageNumber, pageSize);
                if (dl.Count == 1)
                {
                    return PartialView("page", dl.FirstOrDefault());

                }
                return PartialView(dl);
            }
        }

        [HttpPost]
        public ActionResult GuiPhanHoi(CommentTask item)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                item.DateCreate = DateTime.Now;
                db.CommentTasks.Add(item);
                db.SaveChanges();

                var task = db.MainTasks.Where(v => v.Id == item.TaskID).FirstOrDefault();
                if (task != null)
                {
                    var seourl = db.MainTasks.Where(v => v.Id == item.TaskID).Select(v => v.SeoUrl).FirstOrDefault();
                    var HOST = "http://" + Request.Url.Authority + "/post/info?id=" + seourl;


                    var html = @"<b>Xin chào " + task.Map_TenLienLac + @"</b>, <br>

                            Bài đăng của bạn trên website " + Common.SettingData.TenCongTy + @" có người phản hồi: 
                            <br>";
                    html += " <b>Tiêu đề</b>: " + item.TieuDe + @"<br>";
                    html += " <b>Họ tên</b>: " + item.HoTen + @"<br>";
                    html += " <b>Địa chỉ</b>: " + item.DiaChi + @"<br>";
                    html += " <b>Email</b>: " + item.Email + @"<br>";
                    html += " <b>Điện thoại: </b>:" + item.DienThoai + @"<br>";
                    html += " <b>Mục đích: </b>:" + item.MucDich + @"<br>";
                    html += " <b>Nội dung: </b>:" + item.NOIDUNG + @"<br>";
                    html += "<a href='" + HOST + "'><H3>Click vào đây để xem chi tiết</H3></a>";

                    html += "Cám ơn bạn và chúc bạn một ngày tốt lành." + Common.SettingData.TenCongTy + @"!";

                    List<string> nguoinhan = new List<string>();
                    nguoinhan.Add(task.Map_Email);

                    Module.SendMail.SendEmail(nguoinhan, Common.SettingData.TenCongTy + ": Phản hồi : " + task.TaskName, html, "", Common.SettingData.TenCongTy);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult TinChuDe(String ChuDe)
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var cd = db.Categories.Where(v => v.SeoUrl == ChuDe).FirstOrDefault();
                if (cd == null)
                {
                    return Content("");
                }

                ViewBag.SeoUrl = cd.SeoUrl;
                ViewBag.ChuDe = cd.CategoryName;
                var data = db.MainTasks.WhereChuDe(cd.CategoryRowID, db).OrderBy(z => z.NgayDang).Skip(0).Take(5).ToList();
                return PartialView(data);
            }
        }

    }
}