using Newtonsoft.Json;
using Planner2.Models;
using Planner2.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Planner2.Extentions;
using System.Web.UI;
using Planner2.Common;

namespace Planner2.Controllers
{
    public class TasksController : BaseController
    {

        public string ThongBao = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Content/templatemail/ThongBao.html");
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadKhuVuc_Huyen(int ID)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.ThanhPhoes.Where(z => z.MaThanhPho == ID).GroupBy(z => z.MaQuanHuyen).Select(v => new { Name = v.FirstOrDefault().QuanHuyen, Key = v.FirstOrDefault().MaQuanHuyen }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult LoadKhuVuc_Xa(int ID)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.ThanhPhoes.Where(z => z.MaQuanHuyen == ID).GroupBy(z => z.MaPhuongXa).Select(v => new { Name = v.FirstOrDefault().PhuongXa, Key = v.FirstOrDefault().MaPhuongXa }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public ActionResult ResetPass(int Id)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.Users.Where(z => z.Id == Id).FirstOrDefault();
                string PassMD = "ABC123456";
                data.Password = LoginController.EncryptPassword(PassMD);
                db.SaveChanges();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult XoaBai(int ID)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(z => z.Id == ID).FirstOrDefault();
                db.MainTasks.Remove(data);
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }
        // chi tiết dữ liệu
        public ActionResult Info(int ID = 0)
        {
            ViewBag.ID = ID;
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var data = db.MainTasks.Where(z => z.Id == ID).FirstOrDefault();
                if (data == null)
                {
                    return Content("Không tìm thấy dữ liệu,dữ liệu đã bị xóa hoặc không tồn tại." + MvcApplication.HomePage);
                }


                ViewBag.FileUpload = db.UploadFiles.Where(v => v.TaskID == ID && v.TableName == "MainTask").ToList();
                return View(data);
            }
        }

        public ActionResult Create(int TaskID = 0)
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                // nếu = 0 => tạo mới dữ liệu
                ViewBag.FileUpload = db.UploadFiles.Where(v => v.TaskID == TaskID && v.TableName == "MainTask").ToList();

                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                var listdm = db.User_Category.Where(z => z.UserName == nguoidung.UserName && z.NgayHetHan.Value >= DateTime.Now).Select(z => z.CategoryRowID).ToList();
                var Categories = db.Categories.Where(v => v.Menu == "Top").ToList();
                var CategoriesVIP = db.Categories.Where(v => v.Menu != "Top").Where(v => listdm.Contains(v.CategoryRowID) || nguoidung.SupperAdmin==1).ToList();

                ViewBag.Categories = Categories;
                ViewBag.CategoriesVIP = CategoriesVIP;
                #region create
                if (TaskID == 0)
                {
                    return View(new MainTask());
                }

                #endregion
                // nếu != 0 => Sửa dữ liệu
                #region Edit

                var data = db.MainTasks.Where(z => z.Id == TaskID).FirstOrDefault();

                data = data ?? new MainTask();

                ViewBag.SeoUrl = "http://" + Request.Url.Authority + "/post/info?id=" + data.SeoUrl;
                // hoàn thành hoặc Hủy thì không được sửa nữa


                //if (!(data.AttendPerson+"").ToUpper().Contains(nguoidung.UserName.ToUpper()))
                //{
                //    return Content("Bạn không được quyền chỉnh sửa dữ liệu này" + MvcApplication.HomePage);
                //}

                return View(data);


                #endregion


            }
        }
        public ActionResult Page()
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(z => z.Page==true).FirstOrDefault();
                return View(data);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Page(MainTask Item)
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                ViewBag.TitleChuDe = "Giới thiệu";
                var data = db.MainTasks.Where(z => z.Id == Item.Id).FirstOrDefault();
                data.Description = Item.Description;
                db.SaveChanges();
                return RedirectToAction("INFO", "POST", new { id = data.SeoUrl });
            }
        }


        public List<UploadFile> SubmitFile(HttpFileCollectionBase files, string TableName, string ColumnName, int TaskID)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                List<UploadFile> FileUp = new List<UploadFile>();
                for (int i = 0; i < files.Count; i++)
                {
                    try
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        }
                        string newnane = "/FileUpload/" + fname;
                        FileUp.Add(new Models.UploadFile
                        {
                            TaskID = TaskID,
                            UrlFile = newnane,
                            FileName = file.FileName,
                            DateCreated = DateTime.Now,
                            TableName = TableName,
                            ColumnName = ColumnName
                        });
                        fname = Path.Combine(Server.MapPath("~/FileUpload/"), fname);
                        file.SaveAs(fname);
                    }
                    catch (Exception)
                    {

                    }

                }

                db.UploadFiles.AddRange(FileUp);
                db.SaveChanges();
                return FileUp;
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

        // submit tạo mới dữ liệu
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(MainTask item, int[] ChuDe, int[] ChuDeVIP, string Type, HttpPostedFileBase Picture = null)
        {
            try
            {
                ChuDe = ChuDe ?? new int[] { };
                ChuDeVIP = ChuDeVIP ?? new int[] { };
                item.NgayDang = DateTime.Now;
                var file = SubmitFile(new List<HttpPostedFileBase> { Picture });
                if (file.Count > 0)
                {
                    item.Picture = file.FirstOrDefault();
                }

                if (Type == Common.ConstTrangThai.CongKhai)
                {
                    item.Status = Type;
                }
                else
                {
                    item.Status = Common.ConstTrangThai.RiengTu;

                }



                item.Page = false;
                item.SeoUrl = Extentions.SeoUrl.SeoURL(item.TaskName);

                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];

                string noidungmail = "";
                using (Models.Planner2Entities db = new Models.Planner2Entities())
                {


                    foreach (var x in ChuDeVIP)
                    {
                        var listdm = db.User_Category.Where(z => z.UserName == nguoidung.UserName && z.NgayHetHan.Value >= DateTime.Now && x == z.CategoryRowID).Select(z => z.CategoryRowID).Count();
                        if (listdm == 0 && nguoidung.SupperAdmin!=1)
                        {
                            var dtr = "Bạn không có quyền đăng bài vào chuyên mục :" + db.Categories.Where(v => v.CategoryRowID == x).Select(z => z.CategoryName).FirstOrDefault();
                            return Json(new { TT = 1, Value = dtr }, JsonRequestBehavior.AllowGet);
                        }
                    }


                    if (db.MainTasks.Where(v => v.SeoUrl == item.SeoUrl && v.Id != item.Id).Count() > 0)
                    {
                        return Json(new { TT = 1, Value = "Tên tiêu đề đã tồn tại, không thể thêm được tiêu đề giống nhau" }, JsonRequestBehavior.AllowGet);
                    }


                    if (item.Id != 0)
                    {
                        var task = db.MainTasks.Where(z => z.Id == item.Id).FirstOrDefault();

                        // clone task để phục vụ so sánh thay đổi
                        var TaskBK = Models.CompareClass.Clone<MainTask>(task);


                        task.TaskName = item.TaskName;
                        task.Description = item.Description;
                        task.KhuVuc_TP = item.KhuVuc_TP;
                        task.KhuVuc_Xa = item.KhuVuc_Xa;
                        task.KhuVuc_Huyen = item.KhuVuc_Huyen;
                        task.NgayDang = item.NgayDang;
                        task.DienTich = item.DienTich;
                        task.Gia = item.Gia;
                        task.SeoUrl = item.SeoUrl;
                        task.TyGia = item.TyGia;
                        task.Map_LoaiBatDongSan = item.Map_LoaiBatDongSan;
                        task.Map_Huong = item.Map_Huong;
                        task.Map_SoPhongNgu = item.Map_SoPhongNgu;
                        task.Map_DuAn = item.Map_DuAn;
                        task.Map_TenLienLac = item.Map_DuAn;
                        task.DienTich = item.DienTich;
                        task.Map_Zalo = item.Map_Zalo;
                        task.Map_Skyper = item.Map_Skyper;
                        task.Page = item.Page;
                        task.Status = item.Status;
                        task.Map_Email = item.Map_Email;
                        task.NgayDang = item.NgayDang;
                        task.Youtube = item.Youtube;
                        if (!string.IsNullOrEmpty(item.Picture))
                        {
                            task.Picture = item.Picture;
                        }

                        // kiểm tra sự thay đổi
                        var Change = Models.CompareClass.ClassWithClassToTableHTML<MainTask>(TaskBK, task);
                        if (Change != null)
                        {
                            db.SaveChanges();



                            string process = nguoidung.StaffID + " - " + nguoidung.StaffName + " đã sửa đổi, bổ sung dữ liệu dữ liệu.";
                            noidungmail = process + Change;


                            // Thêm lịch sử    
                            AddHistory(item.Id, noidungmail, process);
                            // Gửi mail
                            SendMainTask(item.Id, noidungmail);


                        }
                    }
                    if (item.Id == 0)
                    {

                        // thêm phòng ban vào đầu mã dữ liệu
                        if (string.IsNullOrEmpty(item.Picture))
                        {
                            item.Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6c/No_image_3x4.svg/1280px-No_image_3x4.svg.png";
                        }


                        item.Created = DateTime.Now;
                        item.CreatedBy = nguoidung.UserName;
                        item.AttendPerson = nguoidung.UserName;


                        db.MainTasks.Add(item);
                        db.SaveChanges();


                        noidungmail = nguoidung.StaffID + "-" + nguoidung.StaffName + " đã tạo mới dữ liệu." + TrangThaiTask(item);
                        // Thêm lịch sử    
                        AddHistory(item.Id, noidungmail);
                        // Gửi mail
                        SendMainTask(item.Id, noidungmail);


                    }
                    db.MainTask_ChuDe.RemoveRange(db.MainTask_ChuDe.Where(v => v.TaskID == item.Id));
                    foreach (var it in ChuDe)
                    {
                        MainTask_ChuDe mcd = new MainTask_ChuDe();
                        mcd.TaskID = item.Id;
                        mcd.CategoryRowID = it;
                        db.MainTask_ChuDe.Add(mcd);
                    }
                    foreach (var it in ChuDeVIP)
                    {
                        MainTask_ChuDe mcd = new MainTask_ChuDe();
                        mcd.TaskID = item.Id;
                        mcd.CategoryRowID = it;
                        db.MainTask_ChuDe.Add(mcd);
                    }
                    item.UuTien = ChuDeVIP.Count();
                    db.SaveChanges();

                    SubmitFile(Request.Files, "MainTask", "FileDinhKem", item.Id);


                    return Json(new { TT = 0, Value = item.Id }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { TT = 1, Value = "Lỗi, vui lòng kiểm tra lại \n" + ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RemoveFile(int TaskID = 0, int ID = 0)
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.UploadFiles.Where(v => v.ID == ID).FirstOrDefault();
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                string noidungmail = nguoidung.StaffID + "-" + nguoidung.StaffName.Trim() + " đã Xoá file : <a target='_blank' href='" + data.UrlFile + "'>" + data.FileName + "</a>";

                AddHistory(TaskID, noidungmail);
                db.UploadFiles.Remove(data);
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RemoveFilePicture(int ID = 0)
        {

            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(v => v.Id == ID).FirstOrDefault();
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                string noidungmail = nguoidung.StaffID + "-" + nguoidung.StaffName.Trim() + " đã Xoá file : <a target='_blank' href='" + data.Picture + "'>" + data.Picture + "</a>";

                AddHistory(ID, noidungmail);
                data.Picture = "";
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        // kiểm tra file có phải file ảnh hay không
        public static bool GetImageFormat(string fileName)
        {
            try
            {
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                    throw new ArgumentException(
                        string.Format("Unable to determine file extension for fileName: {0}", fileName));

                switch (extension.ToLower())
                {
                    case @".bmp":
                        return true;

                    case @".gif":
                        return true;

                    case @".ico":
                        return true;

                    case @".jpg":
                    case @".jpeg":
                        return true;

                    case @".png":
                        return true;

                    case @".tif":
                    case @".tiff":
                        return true;

                    case @".wmf":
                        return true;

                    default:
                        return false;
                }
            }
            catch
            {

                return false;

            }
        }

        #region Comment phản hồi
        public ActionResult Comment(int TaskID = 0)
        {

            ViewBag.TaskID = TaskID;
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.CommentTasks.Where(z => z.TaskID == TaskID && z.NOIDUNG != null && z.NOIDUNG.Trim() != "").ToList();

                ViewBag.FileUpload = db.UploadFiles.Where(v => v.TaskID.HasValue && db.CommentTasks.Where(x => x.TaskID == TaskID).Select(z => z.ID).Contains(v.TaskID.Value) && v.TableName == "CommentTask").ToList();

                return PartialView(data);
            }
        }

        [HttpPost]  // submit phản hồi về dữ liệu
        [ValidateInput(false)]
        public ActionResult UploadFile()
        {
            try
            {
                List<string> FileUp = new List<string>();
                var TaskID = int.Parse(Request.Params["TaskID"].ToString());
                var GuiMail = bool.Parse(Request.Params["GuiMail"].ToString());
                var NOIDUNG = Request.Params["NOIDUNG"].ToString();
                var ParentID = int.Parse(Request.Params["ParentID"].ToString());


                NOIDUNG = NOIDUNG.Replace("\r\n", "<br>");
                using (Models.Planner2Entities db = new Planner2Entities())
                {
                    Models.CommentTask tim = new CommentTask();
                    tim.DateCreate = DateTime.Now;
                    tim.TaskID = TaskID;
                    tim.ParentID = ParentID;
                    var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];

                    tim.NOIDUNG = (GuiMail == true ? "<B>[Mail]</B>:<br> " : "") + NOIDUNG;
                    db.CommentTasks.Add(tim);
                    db.SaveChanges();

                    SubmitFile(Request.Files, "CommentTask", "FileDinhKem", tim.ID);

                    var data = db.MainTasks.Where(z => z.Id == TaskID).FirstOrDefault();
                    AddNotify(data, "Đã phản hồi dữ liệu :<br>" + tim.NOIDUNG, nguoidung.UserName);

                    if (GuiMail == true)
                    {



                        SendMainTask(TaskID, nguoidung.StaffID + "-" + nguoidung.StaffName + " đã gửi phản hồi :<br> <br> <br> " + NOIDUNG);



                    }
                    //    Models.BackUpDB.BackUpNow();

                    return Json("1", JsonRequestBehavior.AllowGet);


                }
            }
            catch (Exception e)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
        public List<string> GetPeopleOfTask(MainTask task)
        {
            List<string> People = (task.AttendPerson + "").Split(',').Where(v => !string.IsNullOrEmpty(v)).ToList();
            return People;
        }
        public void AddNotify(MainTask Task, string NOIDUNG, string UseSend, string URL = "/Tasks/Info?ID=")
        {
            var ToUse = GetPeopleOfTask(Task).Where(z => z != UseSend);
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                if (NOIDUNG.Length >= 2000)
                {
                    NOIDUNG = NOIDUNG.Substring(0, 2000);


                }
                Models.Notify tim = new Notify();
                tim.NOIDUNG = NOIDUNG;
                tim.URL = URL + Task.Id.ToString();
                tim.TaskID = Task.Id;
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                tim.CREATE_BY = nguoidung.UserName;
                tim.CREATED = DateTime.Now;
                db.Notifies.Add(tim);
                db.SaveChanges();

                foreach (var item in ToUse)
                {
                    var u = new NotifyUser();
                    u.NotifyID = tim.ID;
                    u.TaskID = tim.TaskID;
                    u.ToUser = item;
                    u.ReadNotify = false;
                    db.NotifyUsers.Add(u);
                }
                db.SaveChanges();
            }

        }

        #endregion


        public int GetTT(List<_TRANGTHAI> data, string TT = "")
        {
            var dl = data.Where(z => z.status == TT).FirstOrDefault();
            if (dl != null)
            {
                return dl.Qty;
            }
            return 0;
        }
        public void AddHistory(int TaskID, string NoiDung = "", string Notify = "")
        {
            if (NoiDung == "")
            {
                return;
            }
            if (Notify == "")
            {
                Notify = NoiDung;
            }
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];

            History his = new History();
            his.DATECREATE = DateTime.Now;
            his.StaffID = nguoidung.UserName;
            his.TaskID = TaskID;

            his.NOIDUNG = NoiDung;
            using (Planner2Entities db = new Planner2Entities())
            {
                db.Histories.Add(his);
                db.SaveChanges();
                var data = db.MainTasks.Where(z => z.Id == TaskID).FirstOrDefault();
                AddNotify(data, Notify, nguoidung.UserName);
            }
            //    Models.BackUpDB.BackUpNow();

        }

        public void SendMainTask(int TaskID, string NOIDUNG = "", string[] nguoinhan = null)
        {
            NOIDUNG = NOIDUNG.Replace("\n", "<br>").Trim();
            if (NOIDUNG == "")
            {
                return;
            }
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(z => z.Id == TaskID).FirstOrDefault();

                var ND = ThongBao;
                NOIDUNG += "<br>";
                NOIDUNG += "<br>";
                NOIDUNG += "---------------------------END---------------------------";
                NOIDUNG += "<br>";
                NOIDUNG += "<br>";
                ND = ND.Replace("{{NoiDungLITE}}", NOIDUNG.Substring(0, (NOIDUNG.Length > 50 ? 50 : NOIDUNG.Length)));
                ND = ND.Replace("{{NoiDungLITE}}", NOIDUNG.Substring(0, (NOIDUNG.Length > 50 ? 50 : NOIDUNG.Length)));
                ND = ND.Replace("{{NoiDung}}", NOIDUNG);
                ND = ND.Replace("{{TaskName}}", data.TaskName);
                ND = ND.Replace("{{CVCHINH}}", data.TaskName);
                ND = ND.Replace("{{Content}}", data.Description);


                ND = ND.Replace("{{Link}}", MvcApplication.HostUrl + "/Tasks/Info?ID=" + data.Id);
                List<string> mail = new List<string>();
                if (nguoinhan == null)
                {

                    mail.AddRange(GetPeopleOfTask(data));

                }
                else
                {

                    mail.AddRange(nguoinhan);
                }
                //Task.Run(() =>
                // SendMail.SendEmail(data.MessageId, data.Id, mail, "[CV-" + "" + data.Id + "]: " + data.TaskName + (CVCHINH != "" && CVCHINH != null ? "(" + CVCHINH + ")" : ""), ND)
                //);


            }
        }
        public virtual string RenderPartialViewToString(string viewName, object viewmodel)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = viewmodel;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
        public ActionResult History(int TaskID = 0)
        {

            ViewBag.TaskID = TaskID;
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.Histories.Where(z => z.TaskID == TaskID).ToList();
                return PartialView(data);
            }
        }
        public string CheckTrangThai(MainTask task)
        {
            return task.Status;
        }


        public void UpdateHuy(int ID, string Noidung = "", Planner2.Models.User nguoidung = null)
        {
            using (Models.Planner2Entities db = new Planner2Entities())
            {
                var data = db.MainTasks.Where(c => c.Id == ID).FirstOrDefault();
                data.Status = ConstTrangThai.Huy;
                db.SaveChanges();
            }
        }

        public string TrangThaiTask(MainTask task)
        {


            return task.Status;
        }



    }
}