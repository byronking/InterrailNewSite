using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class IRGRailCarrier
    {
        public int Id { get; set; }
        public string RailCarrierCode { get; set; }
        public string RailCarrierName { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}