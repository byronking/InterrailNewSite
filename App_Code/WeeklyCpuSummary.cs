using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class WeeklyCpuSummary
    {
        public string Facility { get; set; }
        public decimal TotalPay { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalCostPerUnit { get; set; }
        public decimal Variance { get; set; }
        public string PossibleFactorsForVariance { get; set; }
    }
}