using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Extentions
{
    public static class DateTimeExtention
    {
        public static string ToString_N(this DateTime? str, string format = "dd/MM/yyyy")
        {
            return (str.HasValue ? str.Value.ToString(format) : "");
        }
        public static string ToString_N(this DateTime  str, string format = "dd/MM/yyyy")
        {
            return (str!=null ? str.ToString(format) : "");
        }
    }
}