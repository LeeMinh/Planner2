using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Common
{
    public class ConstVal
    {
        public const string TrieuDong = "Triệu";
        public const string TyDong = "Tỷ";
        public const string TrieuM2 = "Triệu/m2";
        public const string ThoaThuan = "Thỏa thuận";
        public static List<string> data = new List<string>() {
      ThoaThuan,  TrieuDong,TrieuM2,TyDong
        };

    }
}