using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailCompany
    {
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LogoPath { get; set; }
        public int PayPeriodId { get; set; }
        public string PayrollCompanyCode { get; set; }
        public decimal OutOfTownRate { get; set; }
        public decimal OutOfTownHoursPerDay { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}