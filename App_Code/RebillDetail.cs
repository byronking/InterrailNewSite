using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class RebillDetail
    {
        public string Description { get; set; }
        public int TaskId { get; set; }
        public int RebillSubTasksId { get; set; }
    }
}