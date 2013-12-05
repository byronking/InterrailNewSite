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

using System.Text;
using System.IO;

namespace InterrailPPRS.Admin
{
    public partial class MAS90Generate : PageBase
    {

        public string sselFacilities;
        public string sfromDateDetail = "";
        public string stoDateDetail = "";
        public string sType = "";
        public string Destination = "";
        public string companyid = "";
        public string sFileInfo = "";


        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            sfromDateDetail   = Request["startdate"];
            stoDateDetail     = Request["enddate"];
            sType             = Request["Type"];
            Destination       = Request["savetype"];
            companyid         = Request["company"];


              GrantAccess("Super, Admin");


            //  Server side file name && path for( csv file;
            //Const CSVFileName = "C:\ADP\PCPW\ADPDATA\prdmnepi.csv";

            string CSVFileName = Server.MapPath("/") + "\\Mas90" + sType + ".csv";


            if (sType == "production"){

               if (Destination == "Server"){

                  if(File.Exists(CSVFileName)){
                      File.Delete(CSVFileName);
                  }
                  StreamWriter sw =  File.CreateText(CSVFileName);
                  sw.Write(GetLinesProduction());
                  sw.Close();
       
                  FileInfo fil = new FileInfo(CSVFileName);

                  sFileInfo = "File Created<br>";
                  sFileInfo = sFileInfo + "Path " + fil.FullName + "<br>" ;
                  sFileInfo = sFileInfo + "Last Modified: " + fil.CreationTime ;
   

               }else{
                  Response.ContentType = "text/csv";
                  Response.AddHeader("content-disposition", "attachment; filename=\"Mas90Production.csv\"");
                  Response.Write(GetLinesProduction());
                  Response.End();
              }

            }else{
              if (sType == "payrollirg"){

               if (Destination == "Server"){

                  if(File.Exists(CSVFileName)){
                      File.Delete(CSVFileName);
                  }
                  StreamWriter sw =  File.CreateText(CSVFileName);
                  sw.Write(GetLinesPayrollIRG());
                  sw.Close();
       
                  FileInfo fil = new FileInfo(CSVFileName);

                   sFileInfo = "File Created<br>";
                   sFileInfo = sFileInfo + "Path " + fil.FullName + "<br>" ;
                   sFileInfo = sFileInfo + "Last Modified: " + fil.CreationTime ;

               }else{
                  Response.ContentType = "text/csv";
                  Response.AddHeader( "content-disposition", "attachment; filename=\"Mas90PayrollIRG.csv\"");
                  Response.Write(GetLinesPayrollIRG());
                  Response.End();
              }
              }else{

               if ( Destination == "Server"){

                  if(File.Exists(CSVFileName)){
                      File.Delete(CSVFileName);
                  }
                  StreamWriter sw =  File.CreateText(CSVFileName);
                  sw.Write(GetLinesPayrollTemp());
                  sw.Close();
       
                  FileInfo fil = new FileInfo(CSVFileName);

                  sFileInfo = "File Created<br>";
                  sFileInfo = sFileInfo + "Path " + fil.FullName + "<br>" ;
                  sFileInfo = sFileInfo + "Last Modified: " + fil.CreationTime ;

               }else{

                  Response.ContentType = "text/csv";
                  Response.AddHeader("content-disposition", "attachment; filename=\"Mas90PayrollTemp.csv\"");
                  Response.Write(GetLinesPayrollTemp());
                  Response.End();
              }
             }
            }



    }

    public string GetLinesProduction(){

     string strSQL;
     DataReader rs;
     string sLine;
     DateTime dtEndDate;
     


    strSQL = "";
    strSQL = strSQL + " SELECT    dbo.Tasks.GLAcctNumber, dbo.Facility.GLCostCenter, dbo.Tasks.TaskCode, SUM(dbo.FacilityProductionDetail.Units) as UnitSum ";
    strSQL = strSQL + " FROM         dbo.Facility INNER JOIN ";
    strSQL = strSQL + "                       dbo.FacilityProductionDetail ON dbo.Facility.Id = dbo.FacilityProductionDetail.FacilityID INNER JOIN ";
    strSQL = strSQL + "                       dbo.Tasks ON dbo.FacilityProductionDetail.TaskId = dbo.Tasks.Id ";
    strSQL = strSQL + " WHERE     dbo.FacilityProductionDetail.WorkDate BETWEEN '" + sfromDateDetail + "' AND '" + stoDateDetail + "' " ;
    strSQL = strSQL + "        AND Facility.IRGCompanyID = " + companyid;
    strSQL = strSQL + " GROUP BY dbo.Tasks.GLAcctNumber, dbo.Tasks.TaskCode, dbo.Facility.GLCostCenter ";
    strSQL = strSQL + " HAVING      (dbo.Tasks.TaskCode = 'LO' OR ";
    strSQL = strSQL + "                       dbo.Tasks.TaskCode = 'UL') ";
    strSQL = strSQL + " ORDER BY dbo.Facility.GLCostCenter, dbo.Tasks.GLAcctNumber, dbo.Tasks.TaskCode ";

     
     
     
    rs = new DataReader(strSQL);
    rs.Open();
     
     sLine = "";
     dtEndDate = cDate(stoDateDetail);

     while (!rs.EOF){
         rs.Read();

        int lo = 0;
        int ul = 0 ;
        int totalunits = 0;
        string lastfacility = rs.Item("GLCostCenter");
        if (Trim(rs.Item("TaskCode")) == "LO"){
           lo = System.Convert.ToInt32(rs.Item("UnitSum"));
        }
        if (Trim(rs.Item("TaskCode")) == "UL"){
           ul = System.Convert.ToInt32(rs.Item("UnitSum"));
        }

        if (!rs.EOF){
           if (lastfacility == rs.Item("GLCostCenter")){
              if (Trim(rs.Item("TaskCode")) == "LO"){
                 lo = System.Convert.ToInt32(rs.Item("UnitSum"));
             }
              if (Trim(rs.Item("TaskCode")) == "UL"){
                 ul = System.Convert.ToInt32(rs.Item("UnitSum"));
             }

          }
       }
        totalunits = lo + ul;
        
        
     
        sLine = sLine +  "9300" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Loaded,";
        sLine = sLine + lo ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9310" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Unloaded,";
        sLine = sLine + ul ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9390" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Total Units Handled,";
        sLine = sLine + totalunits ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine +  "9400" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Loaded,";
        sLine = sLine + " -" + lo ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9410" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Unloaded,";
        sLine = sLine + " -" + ul ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9490" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Total Units Handled,";
        sLine = sLine + " -" + totalunits ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine +  "9500" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Loaded,";
        sLine = sLine + lo * 100 ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9510" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Unloaded,";
        sLine = sLine + ul * 100 ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9590" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Total Units Handled,";
        sLine = sLine + totalunits * 100 ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine +  "9600" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Loaded,";
        sLine = sLine + " -" + lo * 100 ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9610" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Units Unloaded,";
        sLine = sLine + " -" + ul * 100 ;
        sLine = sLine + System.Environment.NewLine;

        sLine = sLine + "9690" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "UT, 0, 0, 0, Total Units Handled,";
        sLine = sLine + " -" + totalunits * 100 ;
        sLine = sLine + System.Environment.NewLine;

     } // End Loop
     

     
     
     return sLine;
     
}


    public string GetLinesPayrollTemp(){

     string strSQL;
     DataReader rs;
     string sLine;
     

    strSQL = "";
    strSQL = strSQL + " SELECT      Tasks.GLAcctNumber, Facility.GLCostCenter, Tasks.TaskCode, SUM(EmployeeTaskWorkedPay.PayAmount) AS cost, Facility.Name, Tasks.TaskDescription ";
    strSQL = strSQL + " FROM         EmployeeTaskWorked INNER JOIN ";
    strSQL = strSQL + "                       Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id INNER JOIN ";
    strSQL = strSQL + "                       Facility ON EmployeeTaskWorked.FacilityID = Facility.Id INNER JOIN ";
    strSQL = strSQL + "                       EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId INNER JOIN ";
    strSQL = strSQL + "                       Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id ";
    strSQL = strSQL + " WHERE     (Facility.IRGCompanyId = " + companyid + ") ";
    strSQL = strSQL + "   AND     (EmployeeTaskWorked.WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') ";
    strSQL = strSQL + "   AND     ((Employee.HireDate > EmployeeTaskWorked.WorkDate) OR (Employee.HireDate IS NULL)) ";

    strSQL = strSQL + " GROUP BY Tasks.GLAcctNumber, Tasks.TaskCode, Facility.GLCostCenter,  Facility.Name, Tasks.TaskDescription ";
    strSQL = strSQL + " ORDER BY Facility.GLCostCenter, Tasks.GLAcctNumber, Tasks.TaskCode ";

     
      
     rs = new DataReader(strSQL);
     rs.Open();
     
     sLine = "";
     DateTime dtEndDate = cDate(stoDateDetail);

     double totalcost = 0;

     while (rs.Read()){

        //rs.Read();

        string lastfacility = rs.Item("GLCostCenter");
        string lastfacilityName = Replace(rs.Item("Name"), ",", "");
        
        
     
        sLine = sLine +  rs.Item("GLAcctNumber") + "" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "TP, 0, 0, 0,";
        sLine = sLine + rs.Item("TaskDescription") + ",";
        sLine = sLine + rs.Item("Cost") ;
        sLine = sLine + System.Environment.NewLine;
        
        totalcost  = totalcost + cDbl(rs.Item("Cost"));
        

        
        if (rs.EOF) {

           sLine = sLine + "2150" + lastfacility + "000,," ;
           sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
           sLine = sLine + "TP, 0, 0, 0,Accrued sublet - " + lastfacilityName + ",";
           sLine = sLine + "-" + totalcost ;
           sLine = sLine + System.Environment.NewLine;

           totalcost = 0;
           
           
        }else{
        
        if (lastfacility != rs.Item("GLCostCenter")){

           sLine = sLine + "2150" + lastfacility + "000,," ;
           sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
           sLine = sLine + "TP, 0, 0, 0,Accrued sublet - " + lastfacilityName + ",";
           sLine = sLine + "-" + totalcost ;
           sLine = sLine + System.Environment.NewLine;

           totalcost = 0;
           lastfacility = rs.Item("GLCostCenter");
           
       }
       }


     } //End Loop
     

     
     return sLine;

}


     public string GetLinesPayrollIRG(){

     string strSQL;
     DataReader rs;
     string sLine;


    strSQL = "";
    strSQL = strSQL + " SELECT      Tasks.GLAcctNumber, Facility.GLCostCenter, Tasks.TaskCode, SUM(EmployeeTaskWorkedPay.PayAmount) AS cost, Facility.Name, Tasks.TaskDescription ";
    strSQL = strSQL + " FROM         EmployeeTaskWorked INNER JOIN ";
    strSQL = strSQL + "                       Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id INNER JOIN ";
    strSQL = strSQL + "                       Facility ON EmployeeTaskWorked.FacilityID = Facility.Id INNER JOIN ";
    strSQL = strSQL + "                       EmployeeTaskWorkedPay ON EmployeeTaskWorked.Id = EmployeeTaskWorkedPay.EmployeeTaskWorkedId INNER JOIN ";
    strSQL = strSQL + "                       Employee ON EmployeeTaskWorked.EmployeeId = Employee.Id ";
    strSQL = strSQL + " WHERE     (Facility.IRGCompanyId = " + companyid + ") AND (EmployeeTaskWorked.WorkDate Between '" + sfromDateDetail + "' AND '" + stoDateDetail + "') AND (Employee.HireDate < '" + stoDateDetail + "') AND (Employee.HireDate IS NOT NULL) ";
    strSQL = strSQL + " GROUP BY Tasks.GLAcctNumber, Tasks.TaskCode, Facility.GLCostCenter,  Facility.Name, Tasks.TaskDescription ";
    strSQL = strSQL + " ORDER BY Facility.GLCostCenter, Tasks.GLAcctNumber, Tasks.TaskCode ";
    
     
     rs = new DataReader(strSQL);
     rs.Open();
     
     sLine = "";
     DateTime dtEndDate = cDate(stoDateDetail);

     double totalcost = 0;
     while (rs.Read()){

         //rs.Read();

        string lastfacility = rs.Item("GLCostCenter");
        string lastfacilityName = Replace(rs.Item("Name"), ",", "");
     
        sLine = sLine +  rs.Item("GLAcctNumber") + "" + lastfacility + "000,," ;
        sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
        sLine = sLine + "PR, 0, 0, 0,";
        sLine = sLine + rs.Item("TaskDescription") + ",";
        sLine = sLine + rs.Item("Cost") ;
        sLine = sLine + System.Environment.NewLine;
        
        totalcost  = totalcost + cDbl(rs.Item("Cost"));
        

        
        if (rs.EOF ){

           sLine = sLine + "210000000,," ;
           sLine = sLine + Right("0" + Month(dtEndDate), 2) + "/" + Right("0" + Day(dtEndDate), 2) + "/" + Right(Year(dtEndDate),4) + ",";
           sLine = sLine + "PR, 0, 0, 0,Accrued salaries,";
           sLine = sLine + "-" + totalcost ;
           sLine = sLine + System.Environment.NewLine;

           totalcost = 0;
           
       }


    } //End Loop
     

     

     
    return sLine;
     
}

    }
}