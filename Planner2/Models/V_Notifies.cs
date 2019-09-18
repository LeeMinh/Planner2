using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class V_Notifies : Notify
    {
        public bool ReadNotify { get; set; }
        public string TaskName { get; set; }
        public int NotifyUserID { get; set; }
    }
}