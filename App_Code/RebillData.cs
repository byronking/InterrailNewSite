using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class RebillData
    {
        public int RebillDetailId { get; set; }
        public DateTime WorkDate { get; set; }
        public string RebillStatus { get; set; }
        public string Description { get; set; }
        public string TaskCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal TotalHours { get; set; }
        public int TotalUnits { get; set; }
        public string HoursOrUnits { get; set; }
    }
}