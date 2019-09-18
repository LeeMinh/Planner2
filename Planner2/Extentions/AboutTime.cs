using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Module
{
    public static class AboutTime
    {
        public static string GetAboutTime(this DateTime date)
        {
            string time = "";
            double soGiayChenhLenh = (DateTime.Now - date).TotalSeconds;
            if (soGiayChenhLenh < 60)
            {
                time = (int)soGiayChenhLenh + " giây trước";
            }
            else
            {
                if (soGiayChenhLenh / 60 < 60)
                {
                    time = (int)(soGiayChenhLenh / 60) + " phút trước";

                }
                else
                {
                    if (soGiayChenhLenh / 60 / 60 < 24)
                    {
                        time = (int)(soGiayChenhLenh / 60 / 60) + " giờ trước";

                    }
                    else
                    {
                        time = date.ToString("yyyy-MM-dd HH:mm");

                    }
                }
            }
            return time;
        }
        public static string GetAboutTime(this DateTime? date)
        {
            if (date.HasValue)
            {
                return GetAboutTime(date.Value);
            }
            return "";
        }
    }
}