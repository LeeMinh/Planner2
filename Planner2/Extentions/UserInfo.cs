using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Extentions
{
    public static class UserInfo
    { 
        public static string GetID(this int ID)
        {
            return "KH"+ ID.ToString("0000000");
        }
    }
}