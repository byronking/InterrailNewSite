using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS.Payroll
{
    public partial class ApprovePayrollDataEdit : PageBase
    {
        
        public int rs_numRows;
        public string sStatus;
        public string sWDate;
        public DataReader rs;
        public string sReturnTo;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User"); 

            string sApprovalStatus;
            //string RecID;
            string sWhere;
            string strSQL;


            sReturnTo = "&ReturnTo=%2FPayroll%2FApprovePayrollDataEdit%2Easp%3FID%3D";

            if (System.Convert.ToString(Request["Approval"]) != "")
            {
                sApprovalStatus = UCase(Trim(Request["Approval"]));

                if ((Request["Which"] != ""))
                {
                    if ((Request["cbxApprove"] != ""))
                    {
                        if ((System.Convert.ToString(Request["Which"]) == "CHECKED"))
                        {
                            sWhere = " Where ID IN (0, " + System.Convert.ToString(Request["cbxApprove"]) + ")";
                        }
                        else
                        {
                            sWhere = " Where ID IN (0)";
                        }
                    }
                    else
                    {
                        sWhere = " Where ID IN (0)";
                    }

                    if (System.Convert.ToString(Request["Which"]) == "OPEN")
                    {
                        sWhere = " Where PayrollStatus='OPEN' AND FacilityID = " + cStr(Session["FacilityID"]);
                        if ((Request["From"] != ""))
                        {
                            sWhere = sWhere + " AND WorkDate Between '" + cStr(Request["From"]) + "' AND '" + cStr(System.Convert.ToString(Request["To"])) + "'";
                        }
                    }
                }
                else
                {
                    sWhere = "Where ID = " + Request["ID"];
                }

                strSQL = " Update EmployeeTaskWorked Set ";
                strSQL +=  "   PayrollStatus = '" + sApprovalStatus + "',";
                strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now) + "',";
                strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
                strSQL = strSQL + sWhere;

                Execute(strSQL);

                Response.Redirect("ApprovePayrollData.aspx");
            }

            string rs__MMColParam;
            rs__MMColParam = "1";
            if (Request.QueryString["ID"] != "")
            {

                rs__MMColParam = Request.QueryString["ID"];

                strSQL = "";

                strSQL = " SELECT dbo.EmployeeTaskWorked.Id, dbo.EmployeeTaskWorked.WorkDate, isNull(dbo.Tasks.TaskDescription, '') + isNull(dbo.OtherTasks.TaskDescription, '') as TaskDescription, UPM, HoursWorked,  ";
                strSQL +=  "PayrollStatus, dbo.EmployeeTaskWorked.ShiftID,  isNull(dbo.Tasks.TaskCode, '') + isNull(dbo.OtherTasks.TaskCode, '') as TaskCode, dbo.EmployeeTaskWorked.Notes,  dbo.EmployeeTaskWorked.LastModifiedOn,  dbo.EmployeeTaskWorked.LastModifiedBy, ";
                strSQL +=  "EmpName = Case dbo.Employee.TempEmployee when 0 then '('+ dbo.Employee.EmployeeNumber + ') ' + dbo.Employee.LastName + ', ' + dbo.Employee.FirstName Else '('+ dbo.Employee.TempNumber + ') ' + dbo.Employee.LastName + ', ' + dbo.Employee.FirstName end ";
                strSQL +=  "FROM dbo.EmployeeTaskWorked INNER JOIN                     ";
                strSQL +=  "dbo.Employee ON dbo.EmployeeTaskWorked.EmployeeId = dbo.Employee.Id LEFT OUTER JOIN   ";
                strSQL +=  "dbo.OtherTasks ON dbo.EmployeeTaskWorked.OtherTaskID = dbo.OtherTasks.Id LEFT OUTER JOIN   ";
                strSQL +=  "dbo.Tasks ON dbo.EmployeeTaskWorked.TaskId = dbo.Tasks.Id  ";
                strSQL +=  "WHERE (dbo.EmployeeTaskWorked.Id = " + Replace(rs__MMColParam, "'", "''") + ")  ";

                rs = new DataReader(strSQL);

                rs_numRows = 0;
                sStatus = UCase(Trim(rs.Item("PayrollStatus").ToString()));

            }

        }

    }
}