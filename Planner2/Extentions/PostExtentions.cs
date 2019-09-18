using Planner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Extentions
{
    public static class PostExtentions

    {
        public static IQueryable<MainTask> WhereChuDe(this IQueryable<MainTask> data, List<int?> CategoryRowID, Models.Planner2Entities db)
        {
            data = data.Where(v =>
             db.MainTask_ChuDe.Any(z => z.TaskID == v.Id && CategoryRowID.Contains(z.CategoryRowID))
            );
            data = data.Where(v => v.Page == false && v.Status == Common.ConstTrangThai.CongKhai);

            return data;
        }
        public static IQueryable<MainTask> WhereChuDe(this IQueryable<MainTask> data, int CategoryRowID, Models.Planner2Entities db)
        {
            var cd = new List<int?>()
            {
                CategoryRowID
            };
            data = data.WhereChuDe(cd, db);

            return data;
        }

    }
}