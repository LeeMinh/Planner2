using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class DetailLateTask
    { 
        public string DeptName { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }

        public int TotalAssignerTask { get; set; }
        public int AssignerTaskLate { get; set; }
        public int AssignerLateDay { get; set; }
        //public int AssignerSumTimes { get; set; }

        public int TotalMasterTask { get; set; }
        public int MasterTaskLate { get; set; }
        public int MasterLateDay { get; set; }
        public int MasterSumTimes { get; set; }

        public int TotalCheckerTask { get; set; }
        public int CheckerTaskLate { get; set; }
        public int CheckerLateDay { get; set; }
        //public int CheckerSumTimes { get; set; }

        public int TotalAuthorizeTask { get; set; }
        public int AuthorizeTaskLate { get; set; }
        public int AuthorizeLateDay { get; set; }
        //public int AuthorizeSumTimes { get; set; }

        public int TotalTask { get; set; }
        public int TotalTaskLate { get; set; } 
        public int TotalDayLate { get; set; }
        //public int SumTimes { get; set; }
    }
}