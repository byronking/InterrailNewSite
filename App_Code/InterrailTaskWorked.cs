using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailTaskWorked
    {
        public int TaskWorkedId { get; set; }
        public int TaskId { get; set; }
        public int OtherTaskId { get; set; }
        public int FacilityId { get; set; }
        public int EmployeeId { get; set; }
        public int RebillDetailId { get; set; }
        public string WorkDate { get; set; }
        public int ShiftId { get; set; }
        public double Upm { get; set; }
        public double HoursWorked { get; set; }
        public string PayrollStatus { get; set; }
        public string OutOfTownType { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedOn { get; set; }
        public string Notes { get; set; }
    }
}