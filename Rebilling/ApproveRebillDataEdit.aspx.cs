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

namespace InterrailPPRS.Rebilling
{
    public partial class ApproveRebillDataEdit : PageBase
    {
        public DataReader rs;
        public int rs_numRows = 0;
        public DataReader rsETW;
        public int rsETW_numRows = 0;
        public DataReader rsAttach;
        public int rsAttach_numRows = 0;

        public string sStatus;
        public string sRebillStatus;
        public string sRecID;
        public string sWhere;
        public string sReturnTo;
        public int Repeat2__index;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            sReturnTo = "&ReturnTo=ApproveRebillDataEdit%2Easp%3FID%3D";

            if(Request["Approval"] != null && cStr(Request["Approval"]) != "")
            {
                sRebillStatus = UCase(Trim(Request["Approval"]));

                if(Request["Which"] != null && cStr(Request["Which"]) != "")
                {
                    if(Request["cbxApprove"]!= null && cStr(Request["cbxApprove"]) != "")
                    {
                        if(cStr(Request["Which"]) == "CHECKED")
                        {
                            sWhere = " Where ID IN (0, " + cStr(Request["cbxApprove"]) + ")";
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

                    if(cStr(Request["Which"]) == "OPEN")
                    {
                        sWhere = " Where RebillStatus='OPEN' AND FacilityID = " + cStr(Session["FacilityID"]);
                        if(Request["From"] != null && cStr(Request["From"]) != "")
                        {
                            sWhere = sWhere + " AND WorkDate Between '" + cStr(Request["From"] ) + "' AND '" + cStr(Request["To"]) + "'";
                        }
                    }
                }
                else
                {
                    sWhere = "Where ID = " + Request["ID"];
                }

                string strSQL = " Update RebillDetail Set ";
                strSQL +=  "  RebillStatus = '" + sRebillStatus     + "',";
                strSQL +=  "      LastModifiedOn = '" + cStr(System.DateTime.Now)  + "',";
                strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
                strSQL = strSQL + sWhere;
                this.Execute(strSQL);

                Response.Redirect("ApproveRebillData.aspx");
            }

            string rs__MMColParam;
            rs__MMColParam = "4";
            if(Request.QueryString["ID"]   != "")
            {
                rs__MMColParam = Request.QueryString["ID"];
            }

            rs = new DataReader( "SELECT InvoiceNumber, MaterialCosts, Vendors, dbo.RebillDetail.Id, dbo.RebillDetail.WorkDate, dbo.RebillDetail.ShiftID, dbo.RebillDetail.RebillStatus, dbo.RebillDetail.LastModifiedBy,  WorkDescription,                       dbo.RebillDetail.LastModifiedOn, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription, dbo.RebillDetail.TotalHours, dbo.RebillDetail.TotalUnits, dbo.RebillSubTasks.Description,  dbo.RebillSubTasks.HoursOrUnits, dbo.FacilityCustomer.CustomerCode, dbo.FacilityCustomer.CustomerName  FROM dbo.RebillDetail INNER JOIN                        dbo.RebillSubTasks ON dbo.RebillDetail.RebillSubTasksId = dbo.RebillSubTasks.Id INNER JOIN                        dbo.Tasks ON dbo.RebillSubTasks.TaskID = dbo.Tasks.Id INNER JOIN                        dbo.FacilityCustomer ON dbo.RebillSubTasks.FacilityCustomerId = dbo.FacilityCustomer.Id  WHERE RebillDetail.Id = " + Replace(rs__MMColParam, "'", "''") + "");
            rs.Open();
            rs_numRows = rs.RecordCount;
            rs.Read();
            sStatus = UCase(Trim(rs.Item("RebillStatus")));

            string rsETW__PFPDID;
            rsETW__PFPDID = "79";
            if (Request.QueryString["ID"] != "") 
            { 
                rsETW__PFPDID = Request.QueryString["ID"]; 
            }
           
            rsETW = new DataReader("SELECT EmployeeTaskWorked.ID, EmployeeID,  HoursWorked, LastName, FirstName, RebillDetailID,  EmpNum = CASE WHEN TempEmployee = 1 Then TempNumber ELSE EmployeeNumber END  FROM EmployeeTaskWorked INNER JOIN Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id  WHERE RebillDetailID = " + Replace(rsETW__PFPDID, "'", "''") + "");
            rsETW.Open();
            rsETW_numRows = rsETW.RecordCount;

            if(cStr(Request["ID"]) != "0")
            {
              rsAttach = new DataReader("SELECT path, title FROM RebillAttachments WHERE RebillDetailId = " + Request["ID"] + "");
              rsAttach.Open();
              rsAttach_numRows = rsAttach.RecordCount;
            }
        }
    }
}