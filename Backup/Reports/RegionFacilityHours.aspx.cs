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

namespace InterrailPPRS.Reports
{
    public partial class RegionFacilityHours : PageBase
    {

        public DataReader rsRegion;
        public int MoveDate;
        public string regionName = "";
        public string sRegion = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");


            if (System.Convert.ToString(Request["LastWeek"]) == "Yes")
            {
                MoveDate = -7;
            }
            else
            {
                MoveDate = 0;
            }

            //Get Region information from URL;
            sRegion = Request["selRegion"];

            string strSQL = " SELECT RegionDescription FROM irgregion WHERE id = " + sRegion;
            rsRegion = new DataReader(strSQL);

            while (!rsRegion.EOF)
            {
                rsRegion.Read();
                regionName = rsRegion.Fields(0);

            } //End Loop


        }


            public DataReader getData(){

              DataReader rsData;
              string strSQL;

              strSQL =        "";

             strSQL +=  "  SELECT Facility.Name as Facility, ";
             strSQL +=  " (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "            FROM          EmployeeTaskWorked  ";
             strSQL +=  "            WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND   workdate =(DateAdd(day, " + 0 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "            ) as Friday,    ";
             strSQL +=  "   (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "            FROM          EmployeeTaskWorked  ";
             strSQL +=  "            WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 1 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "            ) as Saturday, ";
             strSQL +=  "   (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "           FROM          EmployeeTaskWorked  ";
             strSQL +=  "            WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 2 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "            ) as Sunday, ";
             strSQL +=  "   (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "           FROM          EmployeeTaskWorked  ";
             strSQL +=  "           WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 3 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "           ) as Monday, ";
             strSQL +=  "  (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "           FROM          EmployeeTaskWorked  ";
             strSQL +=  "           WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 4 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "           ) as Tuesday, ";
             strSQL +=  "  (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "           FROM          EmployeeTaskWorked  ";
             strSQL +=  "           WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 5 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "           ) as Wednesday, ";
             strSQL +=  "  (SELECT   Count(Distinct EmployeeID) ";
             strSQL +=  "           FROM          EmployeeTaskWorked  ";
             strSQL +=  "           WHERE   EmployeeTaskWorked.FacilityID = Facility.Id AND    workdate = (DateAdd(day, " + 6 + MoveDate + ", dbo.F_START_OF_WEEK(getdate(), DEFAULT))) ";
             strSQL +=  "           ) as Thursday ";
             strSQL +=  "  from Facility Where Facility.Active = 1 and RegionID=" + sRegion + " ";
             strSQL +=  "  Order by Facility.Name ";



              rsData = new DataReader( strSQL);
              rsData.Open();

              return rsData;

            }

            public string FormatTime(string s){

               if ( s == "0"){
                  return "<font color='red'>0</font>";
               }else{
                  return s;
              }

            }

            public void ShowData(){

              DataReader rs;
              rs = getData();
              string ic = "";

              Response.Write ("<tr><td>Facility</td><td align='right'>Fri</td><td align='right'>Sat</td><td align='right'>Sun</td><td align='right'>Mon</td><td align='right'>Tues</td><td align='right'>Wed</td><td align='right'>Thur</td></tr>");


              int i = 0;
              if (!rs.EOF){
    
                while (!rs.EOF){
                    rs.Read();
                if(i % 2 == 0){
                   ic = "reportOddLine";
                }else{
                   ic = "reportEvenLine";
               }

               Response.Write ("<tr class='" + ic + "'><td>" + rs.Item("Facility") + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Friday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Saturday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Sunday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Monday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Tuesday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Wednesday")) + "</td><td align='right'>");
               Response.Write(FormatTime(rs.Item("Thursday")) + "</td></tr>");

                  i = i + 1;

                } //End Loop
             }

            }


    }
}