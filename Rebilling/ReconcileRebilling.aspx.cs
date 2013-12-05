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
    public partial class ReconcileRebilling : PageBase
    {

        public string sFrom, sTo, sSubTasks, sWorkDates, sWhere, sRBSubTaskID, sReturnTo;
        public bool sViewReconcile;
        public DataReader rs;
        public DataReader rst; 
        public DataReader rsSubTask;
        public DataReader rsRecon;

        public int rsSubTask_numRows;
        public int rsRecon_numRows;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            sFrom     = Request["fromDate"];
            sTo       = Request["toDate"];
            sSubTasks = Request["selSubTasks"];

            sReturnTo = "&ReturnTo=%2FRebilling%2FReconcileRebilling%2Easp%3FfromDate%3D" + sFrom + "%26toDate%3D" + sTo + "%26selSubTasks%3D" + sSubTasks;

            if(sFrom != ""){

              sViewReconcile = true;
              sWorkDates = " '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ";
              if (sSubTasks != null && sSubTasks != "")
              {
                sRBSubTaskID = " AND rbs.ID IN (" + sSubTasks + ")";
              }else{
                sRBSubTaskID = " ";
              }

              //This query does !have a server behavior ---;

              string strSQL = " ";
              strSQL +=  "       SELECT rbd.WorkDate, rbd.TotalHours, rbd.RebillStatus,      ";
              strSQL +=  "              rbd.TotalUnits, HoursOrUnits,                        ";
              strSQL +=  "              rbd.FacilityID, t.Id AS TaskID, t.TaskCode,          ";
              strSQL +=  "              t.TaskDescription, rbs.Id AS RBSubTaskID, rID=rbd.ID,";
              strSQL +=  "              rbs.Description, c.CustomerName, c.CustomerCode      ";
              strSQL +=  "         FROM RebillDetail rbd  INNER JOIN RebillSubTasks rbs      ";
              strSQL +=  "              ON rbd.RebillSubTasksId = rbs.Id INNER JOIN Tasks t  ";
              strSQL +=  "              ON rbs.TaskID = t.Id INNER JOIN FacilityCustomer c   ";
              strSQL +=  "              ON rbs.FacilityCustomerId = c.Id                     ";
              strSQL +=  "        WHERE rbd.FacilityID = " + Session["FacilityID"];
              strSQL +=  "          AND (WorkDate BETWEEN " + sWorkDates + ") " + sRBSubTaskID;
              strSQL +=  "     ORDER BY WorkDate, rbs.TaskID ";

              rs = new DataReader(strSQL);
              rs.Open();

              strSQL = " ";
              strSQL +=  "       SELECT etw.HoursWorked, etw.PayrollStatus, etw.Id AS ETWID, ";
              strSQL +=  "              e.LastName, e.FirstName, etw.WorkDate, etw.TaskID,   ";
              strSQL +=  "              t.TaskDescription, t.TaskCode                        ";
              strSQL +=  "         FROM EmployeeTaskWorked etw INNER JOIN Employee e         ";
              strSQL +=  "              ON etw.EmployeeId = e.Id  INNER JOIN Tasks t         ";
              strSQL +=  "              ON etw.TaskID = t.Id                                 ";
              strSQL +=  "        WHERE (t.Rebillable = 1) AND (WorkDate BETWEEN " + sWorkDates + ") ";
              strSQL +=  "          AND etw.FacilityID = " + Session["FacilityID"];
              strSQL +=  "     ORDER BY WorkDate, etw.TaskID";

              rst = new DataReader(strSQL);
              rst.Open();


            }else{
              sViewReconcile = false;
              sFrom = System.DateTime.Now.ToShortDateString();
              sTo = System.DateTime.Now.ToShortDateString();
            }


            string rsSubTask__PFacID;
            rsSubTask__PFacID = "0";
            if(Session["FacilityID"] != null && cStr(Session["FacilityID"]) != ""){rsSubTask__PFacID = cStr(Session["FacilityID"]);}


            rsSubTask = new DataReader("SELECT dbo.RebillSubTasks.Id, dbo.RebillSubTasks.Description, dbo.Tasks.TaskCode, dbo.FacilityCustomer.CustomerCode, dbo.FacilityCustomer.CustomerName  FROM dbo.RebillSubTasks INNER JOIN  dbo.Tasks ON dbo.RebillSubTasks.TaskID = dbo.Tasks.Id INNER JOIN  dbo.FacilityCustomer ON dbo.RebillSubTasks.FacilityCustomerId = dbo.FacilityCustomer.Id  WHERE FacilityID = " + Replace(rsSubTask__PFacID, "'", "''") + "  Order By Description");
            rsSubTask.Open();
            rsSubTask_numRows = 0;


            rsRecon = new DataReader( "SELECT TOP 100 PERCENT dbo.Employee.LastName, dbo.Employee.FirstName, dbo.Employee.EmployeeNumber, dbo.Employee.TempNumber, dbo.Employee.TempEmployee, dbo.RebillSubTasks.Description, dbo.RebillDetail.WorkDate, dbo.EmployeeTaskWorked.WorkDate AS ETWDate,  dbo.EmployeeTaskWorked.HoursWorked, dbo.RebillDetail.TotalHours, dbo.Tasks.TaskCode, dbo.RebillDetail.FacilityID  FROM dbo.Employee RIGHT OUTER JOIN  dbo.RebillSubTasks RIGHT OUTER JOIN dbo.Tasks INNER JOIN  dbo.EmployeeTaskWorked ON dbo.Tasks.Id = dbo.EmployeeTaskWorked.TaskID INNER JOIN dbo.RebillDetail ON dbo.EmployeeTaskWorked.FacilityID = dbo.RebillDetail.FacilityID AND  dbo.EmployeeTaskWorked.WorkDate = dbo.RebillDetail.WorkDate ON dbo.RebillSubTasks.Id = dbo.RebillDetail.RebillSubTasksId AND  dbo.RebillSubTasks.TaskID = dbo.Tasks.Id ON dbo.Employee.Id = dbo.EmployeeTaskWorked.EmployeeId");
            rsRecon.Open();
            rsRecon_numRows = 0;

        }

        public void ShowReconcileReport() {

        string sTitle;
        DateTime dDate;
        DateTime endDate;
        double rbTotalHours = 0.0;
        double rbTotalUnits = 0.0;
        double pTotalHours = 0.0;

        sTitle = "Reconcile Re-Billing<br>" + sFrom + " - " + sTo;

        Response.Write("<table border='0' cellspacing='0' cellpadding='0' width='500' align='center'>");
        Response.Write("   <tr><td colspan='7' align='center'><b>" + sTitle + "</b></td></tr>");
        Response.Write("   <tr><td align='left' colspan='7'>&nbsp;</td></tr>");
        Response.Write("   <tr>");
        Response.Write("       <td colspan='1' align='center' Width='10%' class='cellTopBottomBorder'><b>Date</b></td>");
        Response.Write("       <td colspan='3' align='center' Width='45%' class='cellTopBottomBorder'><b>Rebilling</b></td>");
        Response.Write("       <td colspan='3' align='center' Width='45%' class='cellTopBottomBorder'><b>Payroll</b></td>");
        Response.Write("   </tr>");
        Response.Write("   <tr><td  colspan=7>&nbsp;</td></tr>");

        if ((rs.EOF) && (rst.EOF))
        {
            Response.Write("   <tr>");
            Response.Write("       <td colspan='7' align='center'><b>No records found.</b></td>");
            Response.Write("   </tr>");
        }
        else
        {

            if (rs.EOF)
            {
                dDate = cDate(rst.Item("WorkDate"));
            }
            else
            {
                if (rst.EOF)
                {
                    dDate = cDate(rs.Item("WorkDate"));
                }
                else
                {
                    if (cDate(rs.Item("WorkDate")) <= cDate(rst.Item("WorkDate")))
                    {
                        dDate = cDate(rs.Item("WorkDate"));
                    }
                    else
                    {
                        dDate = cDate(rst.Item("WorkDate"));
                    }
                }
            }

            endDate = cDate(sTo);

            while (dDate <= endDate)
            {

                bool WriteLine = false;
                string sRow = " ";
                sRow = sRow + "   <tr>";
                sRow = sRow + "       <td colspan='1' align='left' Width='10%' valign='top'><b>" + cStr(dDate) + "</b></td>";
                sRow = sRow + "       <td colspan='3' align='left' Width='45%' valign='top'>";
                sRow = sRow + "          <table border='0' cellspacing='0' cellpadding='0' valign='top' width='98%'>";

                if (rs.EOF)
                {
                    //bxmResponse.Write("               &nbsp;";
                }
                else
                {

                    while (!rs.EOF)
                    {
                        rs.Read();

                        if (cDate(rs.Item("WorkDate")) == dDate)
                        {
                            WriteLine = true;
                            string sHRef = "RebillEdit.aspx?ID=" + cStr(rs.Item("rID")) + sReturnTo;

                            rbTotalHours = rbTotalHours + cDbl(rs.Item("TotalHours"));
                            rbTotalUnits = rbTotalUnits + cDbl(rs.Item("TotalUnits"));
                            sRow = sRow + "            <tr class='reportOddLine'>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Task: &nbsp;</div></td><td><a href='" + sHRef + "'>" + rs.Item("TaskDescription") + " (" + rs.Item("TaskCode") + ")" + "</a></td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>SubTask: &nbsp;</div></td><td>" + rs.Item("Description") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Customer: &nbsp;</div></td><td>" + rs.Item("CustomerName") + " (" + rs.Item("CustomerCode") + ")" + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Rebill Status: &nbsp;</div></td><td>" + rs.Item("RebillStatus") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Hours: &nbsp;</div></td><td>" + rs.Item("TotalHours") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Units: &nbsp;</div></td><td>" + rs.Item("TotalUnits") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'>&nbsp;</td><td>&nbsp;</td>";
                            sRow = sRow + "            </tr>";
                        }

                    }
                }


                sRow = sRow + "          </table>";
                sRow = sRow + "       </td>";
                sRow = sRow + "       <td colspan='3' align='left' Width='45%' valign='top'>";
                sRow = sRow + "          <table border='0' cellspacing='0' cellpadding='0' valign='top' width='98%'>";

                if (rst.EOF)
                {
                    //Response.Write(" &nbsp;");
                }
                else
                {

                    while (!rst.EOF)
                    {
                        rst.Read();

                        if (cDate(rst.Item("WorkDate")) == dDate)
                        {
                            WriteLine = true;

                            string sHRef = "/Payroll/TaskWorkedEdit.aspx?Id=" + cStr(rst.Item("ETWID")) + sReturnTo;

                            pTotalHours = pTotalHours + cDbl(rst.Item("HoursWorked"));
                            sRow = sRow + "            <tr class='reportOddLine'>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Employee: &nbsp;</div></td><td><a href='" + sHRef + "'>" + rst.Item("LastName") + ", " + rst.Item("FirstName") + "</a></td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Task: &nbsp;</div></td><td>" + rst.Item("TaskDescription") + " (" + rst.Item("TaskCode") + ")" + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Payroll Status: &nbsp;</div></td><td>" + rst.Item("PayrollStatus") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>Hours: &nbsp;</div></td><td>" + rst.Item("HoursWorked") + "</td>";
                            sRow = sRow + "            </tr>";
                            sRow = sRow + "            <tr>";
                            sRow = sRow + "               <td valign='top' align='Left'><div class='lblColor'>&nbsp;</td><td>&nbsp;</td>";
                            sRow = sRow + "            </tr>";
                        }

                    }
                }

                sRow = sRow + "          </table>";
                sRow = sRow + "       </td>";
                sRow = sRow + "   </tr>";
                sRow = sRow + "   <tr class='reportOddLine'>";
                if (rbTotalHours != pTotalHours)
                {
                    sRow = sRow + "     <td width='10%' align='center' class='cellBottomBorder'><div class='required'>??</div></td>";
                }
                else
                {
                    sRow = sRow + "     <td width='10%'  align='center' class='cellBottomBorder'><img src='/images/check.gif'></td>";
                }

                sRow = sRow + "       <td colspan='3' align='left' Width='45%' valign='top' class='cellBottomBorder'>Total Rebill Hours: &nbsp;" + rbTotalHours + "</td>";
                sRow = sRow + "       <td colspan='3' align='left' Width='45%' valign='top' class='cellBottomBorder'>Total Payroll Hours: &nbsp;" + pTotalHours + "</td>";
                sRow = sRow + "   </tr>";
                sRow = sRow + "   <tr><td  colspan=7>&nbsp;</td></tr>";

                if (WriteLine)
                {
                    Response.Write(sRow);
                }
                dDate = dDate.AddDays(1); 
            } //Loop
        }

        Response.Write("</table>");

    }

    }
}