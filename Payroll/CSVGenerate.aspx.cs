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
using System.Collections.Specialized;
using System.IO;

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
    public partial class CSVGenerate : PageBase    {


        public string CSVFileName;
        public string sselFacilities;
        public string sfromDateDetail;
        public string stoDateDetail;
        public string lastcompanycode;
        public string Destination;
        public string sFileInfo;
        public object fs;
        public string outputstring;

        protected override void Page_Load(object sender, EventArgs e) {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin");

            sselFacilities    = Request["selFacilities"];
            sfromDateDetail   = Request["fromDateDetail"];
            stoDateDetail     = Request["toDateDetail"];
            Destination       = Request["savetype"];
            CSVFileName = Server.MapPath("/") + "\\prdmnepi.csv";

            if(Destination == "Server" ){

                if (File.Exists(CSVFileName)) { File.Delete(CSVFileName); }
                File.Create(CSVFileName);
                File.WriteAllText(CSVFileName,GetLines());

                sFileInfo = "File Created<br>";
                sFileInfo = sFileInfo + "Path " + CSVFileName + "<br>";
                sFileInfo = sFileInfo + "Last Modified: " + File.GetCreationTime(CSVFileName);

            }else{
               outputstring = GetLines();
               Response.ContentType = "text/csv";
               Response.AddHeader( "content-disposition", "attachment; filename=\"pr\"" + lastcompanycode + "epi.csv\"");
               Response.Write(outputstring);
               Response.End();
            }


        }

 

    public string GetLines(){

        string wFacilities;
        string wOFacilities;
        string strSQL;
        DataReader rs;
        string sLine;

         if(sselFacilities != ""){
           wFacilities = "  AND (e.FacilityID IN  (" + sselFacilities + ") ) ";
         }else{
           wFacilities = "  AND (e.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
         }

         strSQL = "";
         strSQL +=  "SELECT c.PayrollCompanyCode, f.FacilityNumber, isNull(SUM(pay.PayAmount), 0.0) AS Pay, isNull(SUM(pay.HoursPaid), 0.0) AS Hours, e.EmployeeNumber, etw.EmployeeId  ";
         strSQL +=  "FROM   dbo.Employee e INNER JOIN                                                                                                       ";
         strSQL +=  "       dbo.EmployeeTaskWorked etw INNER JOIN                                                                                           ";
         strSQL +=  "                     dbo.IRGCompany c INNER JOIN                                                                                          ";
         strSQL +=  "                     dbo.Facility f ON c.Id = f.IRGCompanyId ON etw.FacilityID = f.Id INNER JOIN                                          ";
         strSQL +=  "                     dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId INNER JOIN                                        ";
         strSQL +=  "                     dbo.Tasks t ON etw.TaskID = t.Id ON e.Id = etw.EmployeeId                                                    ";
         strSQL +=  "WHERE     (pay.PayMultiplier = 1) AND  (etw.OtherTaskID = 0) AND (e.Salaried = 0) AND (e.TempEmployee = 0) ";
         strSQL = strSQL + wFacilities;
         strSQL +=  "      AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
         strSQL +=  "GROUP BY c.PayrollCompanyCode, f.FacilityNumber, e.EmployeeNumber, etw.EmployeeId ";

          rs = new DataReader(strSQL);
          rs.Open();

         sLine = "co code,batch id,file #,reg hours ,reg earnings ,o/t hours ,o/t earnings ,hours 4 code,hours 4 amount,earnings 4 code ,earnings 4 amount ,hours 3 code,hours 3 amount,earnings 3 code,earnings 3 amount " + "\n";

         while(rs.Read()){
            lastcompanycode = Left(rs.Item("PayrollCompanyCode"),3);
            sLine = sLine + Left(rs.Item("PayrollCompanyCode"),3) + ",";
            sLine = sLine + Right("00" + rs.Item("FacilityNumber"), 2) + ",";
            sLine = sLine + Right("000000" + rs.Item("EmployeeNumber"), 6) + ",";
            sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("Hours"), 2),",",""), 6) + ",";
            sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("Pay"), 2),",",""), 8) + ",";
            sLine = sLine + GetOT(rs.Item("EmployeeId") + ",");
            sLine = sLine + GetHolAndVac(rs.Item("EmployeeId"));
            sLine = sLine + "\n";

         };



         // get employee with just OtherTasks
         if(sselFacilities != "") {
           wOFacilities = "  AND (FacilityID IN  (" + sselFacilities + ") ) ";
         }else{
           wOFacilities = "  AND (FacilityID IN  (" + Session["FacilityID"] + ") ) ";
         }


         strSQL = "";
         strSQL +=  "  select DISTINCT c.PayrollCompanyCode, f.FacilityNumber,  e.EmployeeNumber, etw.EmployeeID ";
         strSQL +=  "   FROM   dbo.Employee e INNER JOIN                                                                                                       ";
         strSQL +=  "        dbo.EmployeeTaskWorked etw INNER JOIN                                                                                           ";
         strSQL +=  "                      dbo.IRGCompany c INNER JOIN                                                                                          ";
         strSQL +=  "                      dbo.Facility f ON c.Id = f.IRGCompanyId ON etw.FacilityID = f.Id INNER JOIN                                          ";
         strSQL +=  "                      dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId INNER JOIN                                        ";
         strSQL +=  "                      dbo.OtherTasks t ON etw.OtherTaskID = t.Id ON e.Id = etw.EmployeeId                                                    ";
         strSQL +=  "  where ";
         strSQL +=  "         etw.TaskID = 0  AND (e.Salaried = 0) AND (e.TempEmployee = 0) ";
         strSQL = strSQL + wFacilities;
         strSQL +=  "      AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
         strSQL +=  "    AND                                       ";
         strSQL +=  "        employeeid NOT IN                                                           ";
         strSQL +=  "           ( select  employeeID                                                    ";
         strSQL +=  "               from EmployeeTaskWorked                                             ";
         strSQL +=  "                 where                                                              ";
         strSQL +=  "                     OtherTaskID = 0  AND (Salaried = 0) AND (TempEmployee = 0) ";
         strSQL = strSQL + wOFacilities;
         strSQL +=  "               AND (WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
         strSQL +=  "            ) ";
         strSQL +=  " ORDER by c.PayrollCompanyCode, f.FacilityNumber,  e.EmployeeNumber, etw.EmployeeID  ";


          rs = new DataReader(strSQL);


         while(!rs.Read()){
            lastcompanycode = Left(rs.Item("PayrollCompanyCode"),3);

            sLine = sLine + Left(rs.Item("PayrollCompanyCode"),3) + ",";
            sLine = sLine + Right("00" + rs.Item("FacilityNumber"), 2) + ",";
            sLine = sLine + Right("000000" + rs.Item("EmployeeNumber"), 6) + ",";
            sLine = sLine + "0,0,0,0,";
            sLine = sLine + GetHolAndVac(rs.Item("EmployeeId"));
            sLine = sLine + "\n";
         }



         return sLine;

    }



    public string GetOT(string eid){

         string wFacilities;
         string strSQL;
         DataReader rs;
         string sLine;
         decimal OTHours;
         decimal OTPay;

         if(sselFacilities != ""){
           wFacilities = "  AND (etw.FacilityID IN  (" + sselFacilities + ") ) ";
         }else{
           wFacilities = "  AND (etw.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
         }

         strSQL = "";
         strSQL +=  "SELECT     ISNULL(SUM(pay.PayAmount), 0.0) AS Pay, ISNULL(SUM(pay.HoursPaid), 0.0) AS Hours ";
         strSQL +=  "FROM         dbo.EmployeeTaskWorked etw INNER JOIN ";
         strSQL +=  "             dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId ";
         strSQL +=  "WHERE     (pay.PayMultiplier > 1)  ";
         strSQL = strSQL + wFacilities;
         strSQL +=  "    AND (etw.OtherTaskID = 0) ";
         strSQL +=  "    AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
         strSQL +=  "    AND etw.EmployeeId = "  + cStr(eid);

         OTHours = (decimal)0.0;
         OTPay = (decimal)0.0;



          rs = new DataReader(strSQL);

         if(!rs.Read()){
            OTHours = System.Convert.ToDecimal(rs.Item("Hours"));
            OTPay = System.Convert.ToDecimal(rs.Item("Pay"));
         }

         sLine = "";
         sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("Hours"), 2),",",""), 7) + ",";
         sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("Pay"), 2),",",""), 8);

         return sLine;

    }


    public string GetHolAndVac(string eid) {

         string wFacilities;
         string strSQL;
         DataReader rs;
         string sLine;
         double HolHours;
         double HolPay;
         double VacHours;
         double VacPay;
         int Done;

         Done = 0;
         HolHours = 0;
         HolPay = 0;
         VacHours = 0;
         VacPay = 0;
         sLine = "";

         if(sselFacilities != "") {
           wFacilities = "  AND (etw.FacilityID IN  (" + sselFacilities + ") ) ";
         }else{
           wFacilities = "  AND (etw.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
         }

         strSQL = " SELECT     ISNULL(SUM(pay.PayAmount), 0.0) AS Pay, ISNULL(SUM(pay.HoursPaid), 0.0) AS Hours, dbo.OtherTasks.TaskCode  ";
         strSQL +=  "FROM         dbo.EmployeeTaskWorked etw INNER JOIN  ";
         strSQL +=  "                     dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId INNER JOIN  ";
         strSQL +=  "                     dbo.OtherTasks ON etw.OtherTaskID = dbo.OtherTasks.Id  ";
         strSQL +=  "WHERE     ";
         strSQL +=  "     (etw.TaskID = 0) ";
         strSQL = strSQL + wFacilities;
         strSQL +=  "    AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
         strSQL +=  "    AND etw.EmployeeId = "  + cStr(eid);
         strSQL +=  "GROUP BY dbo.OtherTasks.TaskCode ";


          rs = new DataReader(strSQL);

         HolPay = 0;
         HolHours = 0;
         VacPay = 0;
         VacHours = 0;

         while(!rs.Read()){
            if(Trim(rs.Item("TaskCode")) == "HO" ){
                HolPay = cDbl(rs.Item("Pay"));
                HolHours = cDbl(rs.Item("Hours"));
            }
            if(Trim(rs.Item("TaskCode")) == "VA" ){
                VacPay = cDbl(rs.Item("Pay"));
                VacHours = cDbl(rs.Item("Hours"));
            }

         }

         if(HolHours > 0 ){
            Done = Done + 1;
            sLine = sLine + "H," +  Right("000000" + Replace(FormatNumber(HolHours.ToString(), 2),",",""), 5) + ",H," + Right("00000000" + Replace(FormatNumber(HolPay.ToString(), 2),",",""), 7) + ",";
         }else{
            sLine = sLine + ",,,,";
         }

         if(VacHours > 0 ){
            Done = Done + 1;
            sLine = sLine + "V," +  Right("000000" + Replace(FormatNumber(VacHours.ToString(), 2),",",""), 5) + ",V," + Right("00000000" + Replace(FormatNumber(VacPay.ToString(), 2),",",""), 7);
         }else{
            sLine = sLine + ",,,";
         }

    
        return sLine;

        }




    }
}