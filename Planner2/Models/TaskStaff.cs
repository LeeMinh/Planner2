using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class TaskStaff
    {
        public string DeptName { get; set; } 
        public string StaffName { get; set; }
        public string Staff { get; set; }
        public string Responsibility { get; set; }
        public int SumTask { get; set; }
        public int SumTaskLate { get; set; }
        public int SumLateDay { get; set; }
        public int SumTimes { get; set; }
    }
}