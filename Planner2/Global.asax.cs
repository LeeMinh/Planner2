using Planner2.Extentions;
using Planner2.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Planner2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string HostUrl =  System.Configuration.ConfigurationManager.AppSettings["HOST"];
        public static string HomePage = "<br><a href='/'>Click để quay về trang chủ</a>"+ "<br><a href='#' onclick='window.history.back()'>Click để quay về trang trước đó.</a>";
        public static string filePath = AppDomain.CurrentDomain.BaseDirectory;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                Application["Users"] = db.Users.Where(z=>z.Email!=null).ToList();
                
            }
            ReloadSetting();
        }
      public static void ReloadSetting()
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
               
                Common.SettingData obj = new Common.SettingData();
                foreach (var item in db.SettingDatas)
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(item.KeyID, BindingFlags.Public | BindingFlags.Static);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(obj, item.Value, null);
                    }
                    var x = obj.GetType();
                }
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {


            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;

            string ip = Request.UserHostAddress;
            string UserName = (HttpContext.Current.Session[Planner2.Controllers.LoginAuth.NameSession] != null ? ((Planner2.Models.User)HttpContext.Current.Session[Planner2.Controllers.LoginAuth.NameSession]).UserName : "");
             var html = ClassToHTML.ClassToTableHTML<Exception>(exception);

            if (HttpContext.Current != null)
            {
                var url = HttpContext.Current.Request.Url;
              
                 html = "url: " + url.AbsoluteUri + "<br>" + html;
             }

            html = "ip: " + ip + "<br>" + html;
            html = "UserName: " + UserName + "<br>" + html;
            SendMail.SendEmail(new List<string> { "huychu.k14@gmail.com" }, "Lỗi "+Common.SettingData.TenCongTy, html);
        }
    }
}
