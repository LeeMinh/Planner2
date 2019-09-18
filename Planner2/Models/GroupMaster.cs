using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class GroupMaster
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Master { get; set; }
    }
}