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
    public partial class Payroll1 : PageBase
    {

        public int PFacilityID;
        public string PWorkDate;
        public string PShift;
        public string strTaskIDsOnPage = "";
        public string strOtherTaskIDsOnPage = "";
 
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");


            PFacilityID = System.Convert.ToInt32(Session["FacilityID"]);
            PWorkDate = Request["WorkDate"];
            PShift = Request["Shift"];
            strTaskIDsOnPage = "";
            strOtherTaskIDsOnPage = "";


            if (! isDate(PWorkDate) ){
                if (Session["PayrollLastWorkDate"] != null)
                {
                    PWorkDate = System.Convert.ToString(Session["PayrollLastWorkDate"]);
                }
                if(! isDate(PWorkDate) ){
                    PWorkDate = System.DateTime.Now.ToShortDateString();
                }
            }

            if (PShift == null || PShift.Length == 0)
            {
                if (Session["PayrollLastShift"] != null)
                {
                    PShift = Session["PayrollLastShift"].ToString();
                }
                if (PShift == null || PShift.Length == 0)
                {
                    PShift = "1";
                }
            }
     
            Session["PayrollLastShift"] = PShift;
            Session["PayrollLastWorkDate"]   = PWorkDate;
   
        }


        public string GetPayLinks(string d){

          DateTime st;
          string sHtml;

           st = cDate(getStartPay(d));

           sHtml = "<a href=\"payroll.aspx?workdate=" + st.AddDays(-1).ToShortDateString() + "&shift=" + PShift + "\">&lt;&nbsp;</a>" + st.ToString("MMM") + "&nbsp;";

           for (int i = st.Day; i < (st.Day + 7); i++)
           {
               if (cDate(st.AddDays(i - st.Day)) == cDate(d))
               {
                        sHtml = sHtml + "<font size=+1><b>";
                }
                sHtml = sHtml + "<a href=\"payroll.aspx?workdate=" + st.AddDays(i - st.Day).ToShortDateString() + "&shift=" + PShift + "\">" + st.AddDays(i - st.Day).Day + "</a>&nbsp;";
                if (cDate(st.AddDays(i - st.Day)) == cDate(d))
                {
                    sHtml = sHtml + "</b></font>";
                }            }
            sHtml = sHtml + "<a href=\"payroll.aspx?workdate=" + st.AddDays(7).ToShortDateString() + "&shift=" + PShift + "\">&gt;</a>";
            return sHtml;
        }

        public string GetOtherTaskInfo(string tid){

           DataReader LocalRS;
           string sql;
           string sHtml;
      
           sql = "";
           sql = sql + " SELECT     isNull(SUM(HoursWorked), 0) AS HourSum, COUNT(Id) AS EmpCount   ";
           sql = sql + " FROM         dbo.EmployeeTaskWorked    ";
           sql = sql + " WHERE     (FacilityID = " + PFacilityID + ") AND (WorkDate = '" + PWorkDate + "') AND (ShiftID = '" + PShift + "') AND (OtherTaskID = " + tid + ") ";
   
   
           LocalRS =  new DataReader(sql);
           LocalRS.Open();

           sHtml = "";
           if(LocalRS.Read() ){
                 sHtml = sHtml + "<td>" + LocalRS.Item("EmpCount") +  "</td><td>" + LocalRS.Item("HourSum") +  "</td> ";

           }
   
           return sHtml;
  
        }

        public string GetRebillTaskInfo(string rid){
          DataReader LocalRS;
           string sql;
           string sHtml;
      
           sql = "";
           sql = sql + " SELECT     isNull(SUM(HoursWorked), 0) AS HourSum, COUNT(Id) AS EmpCount   ";
           sql = sql + " FROM         dbo.EmployeeTaskWorked    ";
           sql = sql + " WHERE     (FacilityID = " + PFacilityID + ") AND (WorkDate = '" + PWorkDate + "') AND (ShiftID = '" + PShift + "') AND (RebillDetailID = " + rid + ") ";
   
   
           LocalRS = new DataReader(sql);
           LocalRS.Open();

           sHtml = "";
           if(LocalRS.Read() ){
               sHtml = sHtml + "<td>" + LocalRS.Item("EmpCount") + "<a href=TaskWorkedEdit.aspx?ID=0&Type=Rebill&WorkDate=" + PWorkDate + "&Shift=" + PShift + "&RebillDetailID=" + rid + ">Add</a></td><td>" + LocalRS.Item("HourSum") + "</td> ";
           }
   
           return sHtml;
 
        }

        public string GetTaskInfo(string tid){

           DataReader LocalRS;
           string sql;
           string sHtml;
      
           sql = "";
           sql = sql + " SELECT     isNull(SUM(HoursWorked), 0) AS HourSum, COUNT(Id) AS EmpCount   ";
           sql = sql + " FROM         dbo.EmployeeTaskWorked    ";
           sql = sql + " WHERE     (FacilityID = " + PFacilityID + ") AND (WorkDate = '" + PWorkDate + "') AND (ShiftID = '" + PShift + "') AND (TaskID = " + tid + ") ";
   
            LocalRS = new DataReader(sql);
            LocalRS.Open();
 
           sHtml = "";
           if(LocalRS.Read() ){
               sHtml = sHtml + "<td>" + LocalRS.Item("EmpCount") + "</td><td>" + LocalRS.Item("HourSum") + "</td> ";

           }
   
           return sHtml;
  
        }

        public string GetTasksandUnit(){

           DataReader LocalRS;
           string sql;
           string sHtml;
      
          // Billable
           sql = "";
           sql = sql + " SELECT     fpd.TaskId, dbo.Tasks.TaskCode, dbo.Tasks.PayType, COUNT(fpd.RailCarNumber) AS RCCount, SUM(fpd.Units) AS UnitSum  ";
           sql = sql + " FROM         dbo.Tasks RIGHT OUTER JOIN   dbo.FacilityProductionDetail fpd ON dbo.Tasks.Id = fpd.TaskId  " ;
           sql = sql + " WHERE     (fpd.WorkDate = '" + PWorkDate + "') AND (fpd.ShiftID = '" + PShift + "') AND (fpd.FacilityID = " + PFacilityID + ")  GROUP BY fpd.TaskId, dbo.Tasks.TaskCode, dbo.Tasks.PayType ";
   
           LocalRS = new DataReader(sql);
           LocalRS.Open();

           sHtml = "";
           while(LocalRS.Read()){
               strTaskIDsOnPage = strTaskIDsOnPage + ", " + cStr(LocalRS.Item("TaskId"));
              sHtml = sHtml + "<tr><td><a href=\"Payemployee.aspx?WorkDate=" + PWorkDate + "&Shift=" + PShift + "&Task=" + LocalRS.Item("TaskId") + "\">" + LocalRS.Item("TaskCode") +  "</a></td><td>" + LocalRS.Item("RCCount") +  "</td><td>"; 
              if (System.Convert.ToString(LocalRS.Item("PayType")) == "HOURS" ){ 
                    sHtml = sHtml + "&nbsp;";
              }else{
                    sHtml = sHtml + LocalRS.Item("UnitSum");
              }
              sHtml = sHtml +  "</td>" + GetTaskInfo(LocalRS.Item("TaskID")) + "</tr>";
        }

          return sHtml;
        }

        public string GetRebillTasks(){

           DataReader LocalRS;
           string sql; 
           string sHtml;
           //Rebillable
           sql = "";
           sql = sql + " SELECT  rbd.Id, dbo.Tasks.TaskCode, rbd.TotalHours, rst.TaskID, rst.Description ";
           sql = sql + " FROM    dbo.RebillSubTasks rst INNER JOIN ";
           sql = sql + "         dbo.RebillDetail rbd ON rst.Id = rbd.RebillSubTasksId LEFT OUTER JOIN ";
           sql = sql + "         dbo.Tasks ON rst.TaskID = dbo.Tasks.Id ";
           sql = sql + " WHERE (rbd.WorkDate = '" + PWorkDate + "') AND (rbd.ShiftID = '" + PShift + "') AND (rbd.FacilityID = " + PFacilityID + ")  ";
 
           sHtml = "";
           LocalRS = new DataReader(sql);
           LocalRS.Open();

 
           while( LocalRS.Read()){
              strTaskIDsOnPage = strTaskIDsOnPage + ", " + cStr(LocalRS.Item("TaskId"));
              sHtml = sHtml + "<tr><td><a href=\"../Rebilling/Rebilledit.aspx?ID=" + LocalRS.Item("Id") + "&ReturnTo="+ Server.UrlEncode("../PayRoll/Payroll.aspx?WorkDate="+PWorkDate+"&Shift="+PShift)+"\">" + LocalRS.Item("Description") +  "</a></td><td>" + LocalRS.Item("TotalHours") +  "</td><td>" ;
              sHtml = sHtml + "&nbsp;";
              sHtml = sHtml + "</td>" + GetRebillTaskInfo(LocalRS.Item("ID")) + "</tr>";

            }

           return sHtml;
        }

        public string GetOtherTasks(){

           DataReader LocalRS;
           string sql = "";
           string sHtml = "";

           //Other Tasks
           sql = "";
           sql = sql + " SELECT     dbo.OtherTasks.Id, dbo.OtherTasks.TaskCode  ";
           sql = sql + " FROM         dbo.OtherTasks   " ;
           sql = sql + " where (SingleFacility Is Null or SingleFacility = " + Session["FacilityID"] + ") AND (ExcludeFacility is null or ExcludeFacility <> " + Session["FacilityID"] + ") ";
   
           LocalRS = new DataReader(sql);
           LocalRS.Open();

           while(LocalRS.Read()){
              strOtherTaskIDsOnPage = strOtherTaskIDsOnPage + ", " + cStr(LocalRS.Item("Id"));   
              sHtml = sHtml + "<tr><td><a href=\"Payemployee.aspx?WorkDate=" + PWorkDate + "&Shift=" + PShift + "&OtherTask=YES&Task=" + LocalRS.Item("Id") + "\">" + LocalRS.Item("TaskCode") +  "</a></td><td>&nbsp;</td><td>"; 
              sHtml = sHtml + "&nbsp;";
              sHtml = sHtml+  "</td>" + GetOtherTaskInfo(LocalRS.Item("ID").ToString()) + "</tr>";
           }
   
           return sHtml;
   
        }

        public string GetAdditionalTasks(){

          DataReader LocalRS;
           string sql = "";
           string sHtml = "";

           if (strOtherTaskIDsOnPage.Length > 0 && strOtherTaskIDsOnPage.Substring(0, 1) == ",")
           {
               strOtherTaskIDsOnPage = strOtherTaskIDsOnPage.Substring(2);
           }
           if (strTaskIDsOnPage.Length > 0 && strTaskIDsOnPage.Substring(0, 1) == ",")
           {
               strTaskIDsOnPage = strTaskIDsOnPage.Substring(2);
           } 
   
           sql = "";
           sql = sql + "  SELECT     COUNT(etw.EmployeeId) AS People, SUM(etw.HoursWorked) AS Hours, t.TaskCode, t.Id ";
           sql = sql + " FROM         dbo.EmployeeTaskWorked etw INNER JOIN ";
           sql = sql + "                       dbo.Tasks t ON etw.TaskID = t.Id  ";
           sql = sql + " WHERE (etw.WorkDate = '" + PWorkDate + "') AND (etw.ShiftID = '" + PShift + "') AND (etw.FacilityID = " + PFacilityID + ")  ";
           if ( strOtherTaskIDsOnPage.Length > 0 ){
              sql = sql + "      AND (etw.OtherTaskID not in ("+ strOtherTaskIDsOnPage +") ) ";
           } 
           if (strTaskIDsOnPage.Length > 0){ 
           sql = sql + "         AND (etw.TaskID not in ("+ strTaskIDsOnPage +") ) ";
           } 
           sql = sql + " GROUP BY t.TaskCode, t.Id ";

           LocalRS = new DataReader(sql);
           LocalRS.Open();
            
           while(LocalRS.Read()){
              sHtml = sHtml + "<tr><td><a href=\"Payemployee.aspx?WorkDate=" + PWorkDate + "&Shift=" + PShift + "&Task=" + LocalRS.Item("Id") + "\">" + LocalRS.Item("TaskCode") +  "</a></td><td>&nbsp;</td><td>";
              sHtml = sHtml + "&nbsp;";
              sHtml = sHtml+  "</td><td>" + cStr(LocalRS.Item("People")) + "</td><td>" + cStr(LocalRS.Item("Hours")) + "</td></tr>";
           }

           return sHtml;
   
        }

    }
}