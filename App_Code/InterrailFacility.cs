using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailFacility
    {
        public int FacilityId { get; set; }
        public int FacilityNumber { get; set; }
        public string AlphaCode { get; set; }
        public string FacilityName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int DefaultTaskId { get; set; }
        public string DefaultShiftId { get; set; }
        public int RegionId { get; set; }
        public int OvertimeCalcBasis { get; set; }
        public string GlCostCenter { get; set; }
        public int IrgCompanyId { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool Active { get; set; }
        public decimal BudgetedCpu { get; set; }
    }
}