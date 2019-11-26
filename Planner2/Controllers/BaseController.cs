using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Planner2.Controllers
{
    #region SSO


    public class LoginAuth : AuthorizeAttribute
    {
        public static string NameSession = "NewNguoiDung";
        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            Contract.Assert(filterContext != null);
            return filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                   || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (HttpContext.Current.Session[LoginAuth.NameSession] == null)
            {

                if (HttpContext.Current.Request.Cookies[LoginAuth.NameSession] == null)
                {
                    if (SkipAuthorization(filterContext)) return;
                    var lastUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                    HttpContext.Current.Session["lastUrl"] = lastUrl;
                    filterContext.Result = new RedirectResult("/login/index");

                }
                else
                {
                    var nd = HttpContext.Current.Request.Cookies[LoginAuth.NameSession];
                    if (!string.IsNullOrEmpty(nd["_Key"]))
                    {
                        string ID = nd["_Key"];
                        ID = LoginController.Decrypt(ID);
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        try
                        {
                            //   Models.User user = JsonConvert.DeserializeObject<Models.User>(ID);
                            using (Models.Planner2Entities db = new Models.Planner2Entities())
                            {
                                var user = db.Users.Where(z => z.UserName == ID).FirstOrDefault();
                                if (user==null)
                                {
                                    var lastUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                                    HttpContext.Current.Session["lastUrl"] = lastUrl;
                                    filterContext.Result = new RedirectResult("/login/index");
                                }
                                HttpContext.Current.Session[LoginAuth.NameSession] = user;
                            }

                        }
                        catch (Exception)
                        {
                            if (SkipAuthorization(filterContext)) return;
                            var lastUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                            HttpContext.Current.Session["lastUrl"] = lastUrl;
                            filterContext.Result = new RedirectResult("/login/index");

                        }
                    }
                    else
                    {
                        if (SkipAuthorization(filterContext)) return;
                        var lastUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                        HttpContext.Current.Session["lastUrl"] = lastUrl;
                        filterContext.Result = new RedirectResult("/login/index");

                    }

                }

            }



        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result =
           new RedirectToRouteResult(
               new RouteValueDictionary{
                   { "area", "Default" },
                   { "controller", "Login" },
                   { "action", "Index" }

            });

        }

    }

    #endregion
    [LoginAuth]
    public class BaseController : Controller
    {
        public JsonResult JsonMax(object data)
        {
            JsonResult jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = 2147483647;
            return jsonResult;
        }
    }
}