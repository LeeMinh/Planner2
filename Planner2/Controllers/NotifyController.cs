using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class NotifyController : BaseController
    {
        [HttpPost]
        public ActionResult read(int NotifyUserID)
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var DATA = db.NotifyUsers.Where(z => z.ID == NotifyUserID).FirstOrDefault();
                DATA.ReadNotify = true;
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Unread()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                string sql = @"SELECT  count('')
                                        FROM NotifyUser AS T1
                                        INNER JOIN Notify AS t2 ON  t1.NotifyID = t2.ID               
										INNER JOIN MainTask AS t3 ON  t1.TaskID = t3.ID
                                        where t1.ToUser = '" + nguoidung.UserName + "' AND T1.ReadNotify=0 ";
                var data = db.Database.SqlQuery<int?>(sql).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                string sql = @"SELECT top 50 t2.*,t1.ReadNotify,t3.TaskName,t1.ID AS NotifyUserID
                                        FROM NotifyUser AS T1
                                        INNER JOIN Notify AS t2 ON  t1.NotifyID = t2.ID               
										INNER JOIN MainTask AS t3 ON  t1.TaskID = t3.ID
                                        where t1.ToUser = '" + nguoidung.UserName + "' order by t2.id desc";
                var data = db.Database.SqlQuery<Models.V_Notifies>(sql).ToList();
                foreach (var item in data)
                {
                    item.CREATE_BY = GetNameFromUserName(item.CREATE_BY);
                }
                return PartialView(data);
            }
        }   public ActionResult ReadAll()
        {
            var nguoidung = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                string sql = @"SELECT  t2.*,t1.ReadNotify,t3.TaskName,t1.ID AS NotifyUserID
                                        FROM NotifyUser AS T1
                                        INNER JOIN Notify AS t2 ON  t1.NotifyID = t2.ID               
										INNER JOIN MainTask AS t3 ON  t1.TaskID = t3.ID
                                        where t1.ToUser = '" + nguoidung.UserName + "' order by t2.id desc";
                var data = db.Database.SqlQuery<Models.V_Notifies>(sql).ToList();
                foreach (var item in data)
                {
                    item.CREATE_BY = GetNameFromUserName(item.CREATE_BY);
                }
                return View(data);
            }
        }
        public string GetNameFromUserName(string UserName)
        {
            if (UserName == null)
            {
                return UserName;
            }
            var users = (List<Models.User>)HttpContext.Application["Users"];
            var x = users.Where(z =>

            z.UserName.ToUpper() == UserName.ToUpper() ||
            z.StaffID.ToUpper() == UserName.ToUpper()
            ).FirstOrDefault();
            if (x != null)
            {
                return x.StaffID + "-" + x.StaffName;
            }
            else
            {
                return UserName;
            }
        }

    }
}