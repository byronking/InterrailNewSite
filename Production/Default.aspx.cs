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

namespace InterrailPPRS.Production
{
    public partial class Default : PageBase
    {

        public DataReader rsStatus;
        public int rsStatus_numRows;

        public DataReader tmpUFRS;

        public string[] sRange = new string[2];
        public string[] sDays = new string[7];
        
        public string sNoData;
        public int nRows;

        public string startDate = "";

        public string sFacName = "";
        public string sStatus = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

             base.Page_Load(sender, e);


             GrantAccess("Super, Admin, User, Production");

            string sPayRange = "";
            string strSQL = "";
            string sWhere = "";

            if (Request["Type"] == "Approve") {

                string apStartDate = Request["StartDate"];
                string apEndDate = Request["EndDate"];
   
                if ( isDate(apStartDate) && isDate(apEndDate)){
                    string apStatus = "CORPORATE";
                    strSQL = "";
                    strSQL += " Update FacilityProductionDetail Set ";
                    strSQL +=  "  ApprovalStatus = '" + apStatus + "' ";
                    strSQL +=  " WHERE (WorkDate Between '" + apStartDate + "'  AND '" + apEndDate + "') ";
                    strSQL +=  " And FacilityID = " + Request["FID"];
                    strSQL +=  " And ApprovalStatus = 'FACILITY' ";
      
                    //response.write strSQL;
      
                    this.Execute(strSQL);


                    strSQL = "";
                    strSQL += " Update NoProductionData Set ";
                    strSQL +=  "  ApprovalStatus = '" + apStatus + "' ";
                    strSQL +=  " WHERE (WorkDate Between '" + apStartDate + "'  AND '" + apEndDate + "') ";
                    strSQL +=  " And FacilityID = " + Request["FID"];
                    strSQL +=  " And ApprovalStatus = 'FACILITY' ";
      
                    //response.write strSQL;
      
                    this.Execute(strSQL);

      
                }
            }

            startDate = Request["StartDate"];

            if(startDate == ""){
                startDate = System.DateTime.Now.ToShortDateString();
            }

            sRange[0] = getStartPay(startDate);
            sRange[1] = cStr(cDate(sRange[0]).AddDays(6));
            sPayRange = sRange[0] + " - " + sRange[1];

            if(UCase(System.Convert.ToString(Session["UserType"])) == "USER"){
                sWhere = "    WHERE (FacilityID = " + cStr(Session["FacilityID"]) + ")";
            }else{
                sWhere = "    WHERE (1=1)";
            }

            //****************** 5/14/2007 *******************************;
            if(UCase(System.Convert.ToString(Session["UserType"])) == "PRODUCTION")  {
                //get the regions for the user and tailor queries to suit;
                string strUsrFacilities;
                strUsrFacilities = "";
                tmpUFRS = new DataReader("SELECT FacilityId FROM UserRights WHERE UserProfileID = " + Session["UserID"]);
                tmpUFRS.Open();
                if(tmpUFRS.RecordCount > 0){

                    while ( !tmpUFRS.EOF){
                            tmpUFRS.Read();
                            strUsrFacilities = strUsrFacilities + "," + tmpUFRS.Item("FacilityId");

                    }
                    strUsrFacilities = Replace(strUsrFacilities,",","");
                    sWhere = sWhere + " AND Facility.Id IN (" + strUsrFacilities + ")";
                }else{
                    //nothing;
                }
            }

            //************************************************************;
            strSQL = "";
            if(UCase(System.Convert.ToString(Session["UserType"])) != "USER" && Request["rdoFac"] == "Show" ){
                strSQL +=  "SELECT WorkDate='1/1/1900', AlphaCode, FacilityNumber, ID AS FacilityID, Name, ApprovalStatus= '?'";
                strSQL +=  "  FROM Facility ";
                strSQL +=  "UNION ";
            }
            strSQL +=  "  SELECT WorkDate, AlphaCode, FacilityNumber, FacilityID, Name,          ";
            strSQL +=  "         ApprovalStatus=Case ApprovalStatus When 'CORPORATE' Then 'CORP' ";
            strSQL +=  "         WHEN 'FACILITY' Then 'FAC' ELSE 'OPEN' END  ";
            strSQL +=  "    FROM Facility INNER JOIN                                             ";
            strSQL +=  "         FacilityProductionDetail ON Facility.Id = FacilityID            ";
            strSQL = strSQL + sWhere;
            strSQL +=  "     AND (WorkDate Between '" + cStr(sRange[0]) + "'  AND '" + cStr(sRange[1]) + "')";
            strSQL +=  "GROUP BY WorkDate, AlphaCode, FacilityNumber, ApprovalStatus, FacilityID, Name ";
            strSQL +=  "UNION  ";
            strSQL +=  "  SELECT WorkDate, AlphaCode, FacilityNumber, FacilityID, Name,          ";
            strSQL +=  "         ApprovalStatus=Case ApprovalStatus When 'CORPORATE' Then 'CORP' ";
            strSQL += "          WHEN 'FACILITY' Then 'FAC' Else 'OPEN' END  ";
            strSQL +=  "    FROM Facility INNER JOIN                                             ";
            strSQL +=  "         NoProductionData ON Facility.Id = FacilityID                    ";
            strSQL = strSQL + sWhere;
            strSQL +=  "     AND (WorkDate Between '" + cStr(sRange[0]) + "'  AND '" + cStr(sRange[1]) + "')";
            strSQL +=  "GROUP BY WorkDate, AlphaCode, FacilityNumber, ApprovalStatus, FacilityID, Name ";
            strSQL +=  "ORDER BY Name, AlphaCode, WorkDate, ApprovalStatus                             ";


            rsStatus = new DataReader( strSQL);
            rsStatus.Open();
            rsStatus_numRows = 0;

            sNoData = "<font color='red'><b>?</b></font>";
            nRows = 0;

            for( int i=0 ; i <= 6; i++){
                sDays[i] = sNoData;
            }

}

 public void ShowApprovalStatus(){

   Response.Write("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
   Response.Write("  <tr> ");
   Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
   Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
   Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
   Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
   Response.Write("    <td width='10%'>&nbsp;</td><td width='10%'>&nbsp;</td>");
   Response.Write(" </tr>");
   Response.Write(" <tr class='pageTitle'>");
   Response.Write("   <td colspan='10' align='center'  valign='top'>Production Approval Status</td>");
   Response.Write(" </tr>");
   Response.Write(" <tr class='pageTitle'>");
   Response.Write("   <td colspan='10' align='center'  valign='top'>for Period</td>");
   Response.Write(" </tr>");
   Response.Write(" <form name='form1' action=''>");
   Response.Write(" <tr class='pageTitle'>");
   Response.Write("   <td colspan='10' align='center'  valign='top'>");
   Response.Write("     <select name='selDateRange' onChange='goNewDate();'>" + getPayPeriods(0,12,startDate) + "</select>");
   Response.Write("     <input type='hidden' name='startDate'>");
   Response.Write("   </td>");
   Response.Write(" </tr>");
   
   if(UCase(System.Convert.ToString(Session["UserType"])) != "USER"){  
     Response.Write(" <tr>");
     Response.Write("   <td colspan='10' align='center'  valign='top'>");
     if(System.Convert.ToString(Request["rdoFac"]) == "Show"){
        Response.Write("     <input id='rdoShow' name='rdoFac' type='radio' value='Show' checked onClick='goNewDate();'><label for='rdoShow'>Show All Facilities.</label>");
        Response.Write("     <input id='rdoHide' name='rdoFac' type='radio' value='Hide' onClick='goNewDate();'><label for='rdoHide'>Hide</label>");
     }else{
        Response.Write("     <input id='rdoShow' name='rdoFac' type='radio' value='Show' onClick='goNewDate();'><label for='rdoShow'>Show</label>");
        Response.Write("     <input id='rdoHide' name='rdoFac' type='radio' value='Hide' checked onClick='goNewDate();'><label for='rdoHide'>Hide, when no data.</label>");
     }
     Response.Write("   </td>");
     Response.Write(" </tr>");
   }
   Response.Write(" </form>");
   
   
   if(rsStatus.EOF){
     Response.Write("  <tr><td colspan='10'>&nbsp;</td>");
     Response.Write("<tr>");
     Response.Write("   <td colspan='10' align='left'  valign='top' class='required'>No production data found this period.</td>");
     Response.Write("</tr>");
   }else{

     WriteHeadings();
   
     int sFacID = 0;
     while (!rsStatus.EOF){
         rsStatus.Read();
        if(sFacID != cInt(rsStatus.Item("FacilityID"))){
             if(sFacID != 0){
                   WriteRow(sFacName, sFacID);
             }
         for( int i=0; i <= 6; i++){
           sDays[i] = sNoData;
         } 
         sFacName = rsStatus.Item("Name") + " (" + cStr(rsStatus.Item("FacilityNumber")) + ")";
         sFacID   = cInt(rsStatus.Item("FacilityID"));
       }

       if(cDate(rsStatus.Item("WorkDate")) == cDate("1/1/1900") ){
         for( int i=0; i <= 6; i++){
                   sDays[i] = sNoData;
         }  
           }else{
         for( int i=0 ; i <= 6; i ++){
           if((cDate(sRange[0]).AddDays(i)) == cDate(rsStatus.Item("WorkDate"))){
                     sStatus = rsStatus.Item("ApprovalStatus");
                         if(sStatus == "CORP"){
                            sStatus = "<font color='green'>" + sStatus + "</font>";
                         }else{
                             if(sStatus == "FAC"){
                                sStatus = "<font color='blue'>" + sStatus + "</font>";
                             }else{
                                sStatus = "<font color='red'>" + sStatus + "</font>";
                             }
                        }
                         
                     if(sDays[i] == sNoData){
                       sDays[i] = sStatus;
             }else{
                       sDays[i] = sDays[i] + "<br>" + sStatus;
                        }
                         
                  }
        }  
          }

     } //End Loop

     // Write row for( last facility;
     WriteRow( sFacName, sFacID);

  }
   Response.Write("</table>");
}

 public void WriteHeadings(){
   
   Response.Write("  <tr><td colspan='10'>&nbsp;</td>");
   Response.Write("  <tr> ");
   Response.Write("   <td colspan='3' class='cellTopBottomBorder' align='center'>FACILITY</td>");

   for( int i =0; i <= 6; i ++){
     Response.Write("    <td width='10%' class='cellTopBottomBorder' align='center'>" + cStr(Day(cDate(sRange[0]).AddDays(i))) + "</td>");
   }  

   Response.Write(" </tr>");
  
 }

 public void WriteRow(string FacilityName, int FacilityID){
   
      nRows = nRows + 1;
      string lc;

       if(nRows % 2 == 0){
         lc = "reportEvenLine";
       }else{  
         lc = "reportOddLine";
      }
   
       Response.Write("<tr class='" + lc + "'>");
       Response.Write(" <td>");
       if (UCase(System.Convert.ToString(Session["UserType"])) != "USER"){
          Response.Write("<a href='default.aspx?Type=Approve&FID=" + FacilityID.ToString() + "&StartDate=" + sRange[0] + "&EndDate=" + sRange[1] + "'>Approve</a> ");
       }else{
          Response.Write("&nbsp;");
      }
       Response.Write(" </td> ");
       Response.Write(" <td colspan='2' align='right'  valign='top'><b>" + FacilityName + "&nbsp;:</b>&nbsp;</td>");
       for( int i=0; i <= 6; i ++){
         Response.Write("    <td width='10%' align='center'>" + sDays[i] + "</td>");
      }  
       Response.Write("</tr>");
       Response.Write("<tr class='" + lc + "'><td colspan='10'>&nbsp;</td>");
 }

    }
}