using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class Custom_Group_Members
    {
        public int? GroupId { get; set; }

        public string GroupName { get; set; }
        public string StaffID { get; set; }

        public string MemberEmail { get; set; }

        public string MemberName { get; set; }

    }
}