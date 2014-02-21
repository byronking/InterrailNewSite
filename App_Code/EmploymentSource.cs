using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class EmploymentSource
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string SourceCode { get; set; }
        public string SourceName { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}