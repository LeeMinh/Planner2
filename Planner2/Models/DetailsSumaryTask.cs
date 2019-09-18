using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Models
{
    public class DetailsSumaryTask
    {
        public string Link { get; set; }
        public string Staff { get; set; }
        public string Responsibility { get; set; }
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? DateFinish { get; set; }
        public int LateDay { get; set; }
    }
}