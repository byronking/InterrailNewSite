using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class TempPayrollReport
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal Upm { get; set; }
        public string TaskCode { get; set; }
        public string Name { get; set; }
        public string TaskType { get; set; }
        public string TempNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public int OvertimeCalcBasis { get; set; }
        public string SourceName { get; set; }
        public int SourceId { get; set; }
        public decimal RegPayRate { get; set; }
        public decimal RegHoursPaid { get; set; }
        public decimal RegPayAmount { get; set; }
        public decimal OtPayRate { get; set; }
        public decimal OtHoursPaid { get; set; }
        public decimal OtPayAmount { get; set; }
    }
}