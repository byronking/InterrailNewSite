using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class EmployeesForTasks
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TempNumber { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public int FacilityId { get; set; }
    }
}