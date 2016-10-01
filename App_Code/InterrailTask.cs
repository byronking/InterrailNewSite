using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailTask
    {
        public int TaskId { get; set; }
        public int OtherTaskId { get; set; }
        public string TaskCode { get; set; }
        public string TaskDescription { get; set; }
    }
}