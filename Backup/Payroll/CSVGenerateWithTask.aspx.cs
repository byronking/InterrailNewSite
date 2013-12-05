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
    public partial class CSVGenerateWithTask : PageBase
    {

        string sselFacilities;
        string sfromDateDetail;
        string stoDateDetail;
        string lastcompanycode;
        string CSVFileName;
        string Destination;
        string sFileInfo;

        
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin");


            sselFacilities    = Request["selFacilities"];
            sfromDateDetail   = Request["fromDateDetail"];
            stoDateDetail     = Request["toDateDetail"];
            Destination       = Request["savetype"];
            CSVFileName = Server.MapPath("/") + "\\prdmnepi.csv";
            string outputstring;

            string locked = CheckForLocked();
            if(Len(locked) > 1 ){
                Response.Redirect("CSVSelect.aspx?NotLocked=True&Fac=" + Server.UrlEncode(locked));
                Response.End();
            }


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
               Response.AddHeader( "content-disposition", "attachment; filename='pr" + lastcompanycode + "epi.csv'");
               Response.Write(outputstring);
               Response.End();
            }


        }

        public string CheckForLocked(){

             string wFacilities;
             string strSQL;
             string results;
             DataReader rs;
             //string sLine;
             //double currentCompanyCode;
             //double currentFacilityNumber;

             if((sselFacilities != "") ){
               wFacilities = "  && (Facility.ID IN  (" + sselFacilities + ") ) ";
             }else{
               wFacilities = "  && (Facility.ID IN  (" + Session["FacilityID"] + ") ) ";
             }

             strSQL = "";
             strSQL = strSQL +  " SELECT DISTINCT Facility.Name ";
             strSQL = strSQL +  " FROM         EmployeeTaskWorked INNER JOIN ";
             strSQL = strSQL +  "                       Facility ON EmployeeTaskWorked.FacilityID = Facility.Id ";
             strSQL = strSQL +  " WHERE     (EmployeeTaskWorked.PayrollStatus != 'LOCKED')  ";
             strSQL = strSQL +  wFacilities;
             strSQL = strSQL +  " AND (EmployeeTaskWorked.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";


              rs = new DataReader(strSQL);
              rs.Open();
     
             results = "";
             while(!rs.Read()){
                if(Len(results) > 1 ){
                   results = results + "<br>";
                }
                results = results + rs.Item("Name");

             }
     
             return results;
     
        }

        public string GetLines(){

             string wFacilities;
             string strSQL;
             DataReader rs;
             string sLine;
             double currentCompanyCode = 0;
             double currentFacilityNumber = 0;


             if(sselFacilities != "") {
               wFacilities = "  AND (f.ID IN  (" + sselFacilities + ") ) ";
             }else{
               wFacilities = "  AND (f.ID IN  (" + Session["FacilityID"] + ") ) ";
             }

             strSQL = "";

             strSQL = strSQL +  " SELECT     c.PayrollCompanyCode, f.FacilityNumber  ";
             strSQL = strSQL +  "     FROM   dbo.IRGCompany c INNER JOIN ";
             strSQL = strSQL +  "            dbo.Facility f ON c.Id = f.IRGCompanyId  ";
             strSQL = strSQL +  wFacilities;

             rs = new DataReader(strSQL);
             rs.Open();

             if(!rs.Read()){
                currentCompanyCode = System.Convert.ToDouble(rs.Item("PayrollCompanyCode"));
                currentFacilityNumber = System.Convert.ToDouble(rs.Item("FacilityNumber"));
                lastcompanycode = Left(rs.Item("PayrollCompanyCode"),3);
             }
     



             if((sselFacilities != "") ){
               wFacilities = "  && (e.FacilityID IN  (" + sselFacilities + ") ) ";
             }else{
               wFacilities = "  && (e.FacilityID IN  (" + Session["FacilityID"] + ") ) ";
             }

             strSQL = "";

             strSQL = strSQL +  " SELECT     c.PayrollCompanyCode, f.FacilityNumber, f.AlphaCode, e.EmployeeNumber, etw.EmployeeId, t.TaskCode, t.GLAcctNumber, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier = 1 Then pay.PayAmount Else 0 END), 0.0) AS RegPay, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier = 1 Then pay.HoursPaid Else 0 END), 0.0) AS RegHours, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier > 1 Then pay.PayAmount Else 0 END), 0.0) AS OTPay, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.Paymultiplier > 1 Then pay.HoursPaid Else 0 END), 0.0) AS OTHours ";
             strSQL = strSQL +  " FROM         dbo.Employee e INNER JOIN ";
             strSQL = strSQL +  "                       dbo.EmployeeTaskWorked etw INNER JOIN " ;
             strSQL = strSQL +  "                       dbo.IRGCompany c INNER JOIN ";
             strSQL = strSQL +  "                       dbo.Facility f ON c.Id = f.IRGCompanyId ON etw.FacilityID = f.Id INNER JOIN ";
             strSQL = strSQL +  "                       dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId INNER JOIN ";
             strSQL = strSQL +  "                       dbo.Tasks t ON etw.TaskID = t.Id ON e.Id = etw.EmployeeId ";
             strSQL = strSQL +  " WHERE     (etw.OtherTaskID = 0) AND (e.Salaried = 0) AND (e.TempEmployee = 0) ";
             strSQL = strSQL +  wFacilities;
             strSQL = strSQL +  " AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
             strSQL = strSQL +  " GROUP BY c.PayrollCompanyCode, f.FacilityNumber, f.AlphaCode, e.EmployeeNumber, etw.EmployeeId, t.TaskCode, t.GLAcctNumber ";
             strSQL = strSQL +  " union ";
             strSQL = strSQL +  " SELECT     c.PayrollCompanyCode, f.FacilityNumber, f.AlphaCode, e.EmployeeNumber, etw.EmployeeId, t.TaskCode, t.GLAcctNumber, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier = 1 Then pay.PayAmount Else 0 END), 0.0) AS RegPay, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier = 1 Then pay.HoursPaid Else 0 END), 0.0) AS RegHours, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.PayMultiplier > 1 Then pay.PayAmount Else 0 END), 0.0) AS OTPay, ";
             strSQL = strSQL +  "                       ISNULL(SUM(CASE WHEN pay.Paymultiplier > 1 Then pay.HoursPaid Else 0 END), 0.0) AS OTHours ";
             strSQL = strSQL +  " FROM         dbo.Employee e INNER JOIN ";
             strSQL = strSQL +  "                       dbo.EmployeeTaskWorked etw INNER JOIN ";
             strSQL = strSQL +  "                       dbo.IRGCompany c INNER JOIN ";
             strSQL = strSQL +  "                       dbo.Facility f ON c.Id = f.IRGCompanyId ON etw.FacilityID = f.Id INNER JOIN ";
             strSQL = strSQL +  "                       dbo.EmployeeTaskWorkedPay pay ON etw.Id = pay.EmployeeTaskWorkedId INNER JOIN ";
             strSQL = strSQL +  "                       dbo.OtherTasks t ON etw.OtherTaskID = t.Id ON e.Id = etw.EmployeeId ";
             strSQL = strSQL + " WHERE     (etw.OtherTaskID <> 0) AND (e.Salaried = 0) AND (e.TempEmployee = 0) ";
             strSQL = strSQL +  wFacilities;
             strSQL = strSQL +  " AND (etw.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
             strSQL = strSQL +  " GROUP BY c.PayrollCompanyCode, f.FacilityNumber, f.AlphaCode, e.EmployeeNumber, etw.EmployeeId, t.TaskCode, t.GLAcctNumber " ;




              rs = new DataReader(strSQL);

             sLine = "co code,batch id,file #,task id,task code,reg hours,reg earnings,o/t hours,o/t earnings,HO hours,HO earnings,VA hours,VA earnings,FU hours,FU earnings,SK Hours,SK Earnings,PN hours,PN earnings,BT hours,BT earnings,Project" + "\n";

             while( !rs.Read()){

                sLine = sLine + Left(currentCompanyCode.ToString(),3) + ",";
                sLine = sLine + Right("00" + currentFacilityNumber, 2) + ",";
                sLine = sLine + Right("000000" + rs.Item("EmployeeNumber"), 6) + ",";
                sLine = sLine + Left(rs.Item("TaskCode"),3) + ",";
                sLine = sLine + Left(rs.Item("GLAcctNumber"), 4) + ",";

                switch( Left(rs.Item("TaskCode"),2)){

                    case "HO":
                   sLine = sLine + "000000," ;  //Reg hours
                   sLine = sLine + "00000000,"; //Reg earn
                   sLine = sLine + "000000,";   //OT hours
                   sLine = sLine + "00000000,"; //OT earn
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";  //HO hours
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ",";  //HO Earn
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + "000000,";   //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                        break;
                    case "VA":
                   sLine = sLine + "000000,";   //Reg hours;
                   sLine = sLine + "00000000,"; //Reg earn;
                   sLine = sLine + "000000,";   //OT hours;
                   sLine = sLine + "00000000,"; //OT earn;
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO earn;
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ","; //VA hours;
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ","; //VA Earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + "000000,";   //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                        break;
                    case "FU":
                   sLine = sLine + "000000,";   //Reg hours;
                   sLine = sLine + "00000000,"; //Reg earn;
                   sLine = sLine + "000000,";   //OT hours;
                   sLine = sLine + "00000000,"; //OT earn;
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO earn;
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";  //FU hours;
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ",";  //FU Earn;
                   sLine = sLine + "000000,";   //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                        break;
                    case "SK":
                   sLine = sLine + "000000,";   //Reg hours;
                   sLine = sLine + "00000000,"; //Reg earn;
                   sLine = sLine + "000000,";   //OT hours;
                   sLine = sLine + "00000000,"; //OT earn;
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO earn;
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";  //SK hours;
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ",";  //SK Earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                        break;
                    case "PN":
                   sLine = sLine + "000000,";   //Reg hours;
                   sLine = sLine + "00000000,"; //Reg earn;
                   sLine = sLine + "000000,";   //OT hours;
                   sLine = sLine + "00000000,"; //OT earn;
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO earn;
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + "000000,";   //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";  //PN hours;
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ",";  //PN Earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                        break;
                    case "BT":
                   sLine = sLine + "000000,";   //Reg hours;
                   sLine = sLine + "00000000,"; //Reg earn;
                   sLine = sLine + "000000,";   //OT hours;
                   sLine = sLine + "00000000,"; //OT earn;
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO earn;
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + "000000";   //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";  //BT hours
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ","; //BT Earn
                        break;
        
                    default :
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("RegHours"), 2),",",""), 6) + ",";
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("RegPay"), 2),",",""), 8) + ",";
                   sLine = sLine + Right("000000" + Replace(FormatNumber(rs.Item("OTHours"), 2),",",""), 6) + ",";
                   sLine = sLine + Right("00000000" + Replace(FormatNumber(rs.Item("OTPay"), 2),",",""), 8) + ",";
                   sLine = sLine + "000000,";   //HO hours;
                   sLine = sLine + "00000000,"; //HO Earn;
                   sLine = sLine + "000000,";   //VA hours;
                   sLine = sLine + "00000000,"; //VA earn;
                   sLine = sLine + "000000,";   //FU hours;
                   sLine = sLine + "00000000,"; //FU earn;
                   sLine = sLine + "000000,";  //SK hours;
                   sLine = sLine + "00000000,"; //SK earn;
                   sLine = sLine + "000000,";   //PN hours;
                   sLine = sLine + "00000000,"; //PN earn;
                   sLine = sLine + "000000,";   //BT hours;
                   sLine = sLine + "00000000,";  //BT earn;
                   break;
                }

                if(currentFacilityNumber != System.Convert.ToDouble(rs.Item("FacilityNumber")) ){
                  sLine = sLine + rs.Item("AlphaCode");
                }

                sLine = sLine + "\n";
        
                }
             return sLine;

        }
        

    }
}