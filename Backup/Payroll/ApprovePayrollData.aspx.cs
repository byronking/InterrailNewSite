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
    public partial class ApprovePayrollData : PageBase
    {

        public string rsEmp__PSTART;
        public string rsEmp__PEND;
        string strSQL = "";
        public DataReader rsEmp;
        public DataReader rsOTType;
        string FacilityOTType = "";
        string[] arStartEnd;
        int defpayperiodend;
        DateTime deflastend;
        DateTime deflaststart;


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");
           
            if (InStr(0,Request["PayPeriod"] , ",",1) > 0 ){
               arStartEnd = Split(Request["PayPeriod"] , ",", 2);
            }else{
               defpayperiodend = 5;  //Thursday
               deflastend = System.DateTime.Now.AddDays(  - (WeekDay(System.DateTime.Now) + (7 - defpayperiodend) % 7));
               deflaststart = deflastend.AddDays(-(6));
               arStartEnd = Split(deflaststart + "," + deflastend , ",", 2);
            }

            if (isArray(arStartEnd) ){
               rsEmp__PSTART = arStartEnd[0];
               rsEmp__PEND = arStartEnd[1];
            }


            if ( Request["ApproveAction"] == "OPEN"  || Request["ApproveAction"] == "APPROVED"  || Request["ApproveAction"] == "LOCKED" ){

                  strSQL =  "";
                  strSQL +=  "UPDATE  dbo.EmployeeTaskWorked set  PayrollStatus = '" + Request["ApproveAction"] + "',  ";
                  strSQL +=  " LastModifiedBy = '" + Session["UserName"] + "', LastModifiedOn = '" + DateTime.Now + "' ";
                  strSQL +=  "WHERE     (WorkDate BETWEEN '" + rsEmp__PSTART + "' AND '" + rsEmp__PEND + "') AND FacilityID = " + Request["FID"];

                  Execute(strSQL);
  

                  //
                  // Set ApprovalStatus for Production Data as well....
                  //
                  if (Request["ApproveAction"] == "LOCKED" ){
                    strSQL = " Update FacilityProductionDetail ";

                    if (Trim(UCase(System.Convert.ToString(Session["UserType"]))) == "SUPER") {
                      strSQL +=  "  Set ApprovalStatus = 'CORPORATE',";
                    }else{
                      strSQL +=  "  Set ApprovalStatus = 'FACILITY',";
                    }

                    strSQL +=  "      LastModifiedOn = '" + System.DateTime.Now.ToString()  +  "',";
                    strSQL +=  "      LastModifiedBy = '" + Session["UserName"] + "' ";
                    strSQL +=  "WHERE     (WorkDate BETWEEN '" + rsEmp__PSTART + "' AND '" + rsEmp__PEND + "') AND FacilityID = " + Request["FID"];

                    Execute(strSQL);
    
                  }

              }

            
        }


        public string  GetLines(){


               string sHTML = "";
               string strSQL = "";
               string fid = "";
               int nRows = 0;
               DataReader rsProdStatus;
               DataReader rsFacList;
               int nOpenCount = 0;
               bool allowApprove = false;
               bool allowOpen = false;
               bool allowLock = false;
               bool canApprove = false;
               bool canOpen = false;
               bool canLock = false;
               bool showApprove = false;
               bool showOpen = false;
               bool showLock = false;

               string lc = "";
               string gotPayRollStatus;


              // For each Facility for the user

            if (UCase(Session["UserType"].ToString()) == "USER") {
                strSQL = "";
                strSQL +=  "SELECT     Facility.Id, Facility.Name ";
                strSQL +=  "FROM         Facility INNER JOIN ";
                strSQL +=  "                     UserRights ON Facility.Id = UserRights.FacilityId ";
                strSQL += "WHERE   Facility.Active=1 AND  UserRights.UserProfileId = " + Session["UserID"];
                strSQL +=  "ORDER BY Facility.Name ";

            }else{
              strSQL = "";
              strSQL += "Select Facility.ID, Facility.Name from  Facility WHERE   Facility.Active=1 Order by Facility.Name ";
            }



             rsFacList = new DataReader(strSQL);
             rsFacList.Open();


             sHTML = sHTML + "<Table width='100%' border='0' cellspacing='0' cellpadding='0'>";

           while(! rsFacList.EOF){

               rsFacList.Read();

               //'  For each Facility for the user
               fid = rsFacList.Item("ID");

              gotPayRollStatus = getPayRollStatus(rsEmp__PSTART, fid);

              strSQL = "";
              strSQL = " Select isNull(Count(*), 0) as OpenCount from FacilityProductionDetail ";
              strSQL +=  "WHERE    ApprovalStatus = 'OPEN' AND  (WorkDate BETWEEN '" + rsEmp__PSTART + "' AND '" + rsEmp__PEND + "') AND FacilityID = " + fid;


               rsProdStatus = new DataReader(strSQL);
               rsProdStatus.Open();

               nOpenCount = 0;

               if (!rsProdStatus.EOF && rsProdStatus.Read())
               {
                   nOpenCount = cInt(rsProdStatus.Item("OpenCount"));
                }

                allowApprove = false;
                allowOpen = false;
                allowLock = false;

                canApprove = false;
                canOpen = false;
                canLock = false;

                showApprove = false;
                showOpen = false;
                showLock = false;

                if (Session["UserType"].ToString() == "Super" ) {
                    allowApprove = true;
                    allowOpen = true;
                    allowLock = true;
                    canOpen = true;
                }
                if (Session["UserType"].ToString() == "Admin" ) {
                    allowApprove = true;
                    allowLock = true;
                }
                if (Session["UserType"].ToString() == "User" ) {
                    allowApprove = true;
                }

                if (gotPayRollStatus.ToString() == "OPEN" ) {
                    canApprove = false;
                    canLock = false;
                }
                if ( gotPayRollStatus.ToString() == "PAYROLL" ) {
                        canApprove = true;
                        canOpen = true;
                }
                if (gotPayRollStatus.ToString() == "APPROVED" ) {
                        canLock = true;
                        canOpen = true;
                }

                showApprove = canApprove && allowApprove;
                showOpen = canOpen && allowOpen;
                showLock = canLock && allowLock;
              


                //write a table row with open approve lock links and the facility id name and current status


                nRows = nRows + 1;
                if (nRows % 2 == 0) {
                    lc = "reportEvenLine";
                }else{
                    lc = "reportOddLine";
                }

                    sHTML = sHTML + "<tr class='" + lc + "'>";


                if (showOpen && gotPayRollStatus != "OPEN") {
                    sHTML = sHTML + "<td width='15%' align='left'><a href=ApprovePayrollData.aspx?PayPeriod=" + Request["PayPeriod"] + "&ApproveAction=OPEN&FID=" + fid + ">Open</a></td>";
                }else{
                    sHTML = sHTML + "<td width='15%' align='left'>&nbsp;</td>";
                }
                if (showApprove ) {
                    sHTML = sHTML + "<td width='15%' align='left'><a href=ApprovePayrollData.aspx?PayPeriod=" + Request["PayPeriod"] + "&ApproveAction=APPROVED&FID=" + fid + ">Approve</a></td>";
                        }else{
                            sHTML = sHTML + "<td width='15%' align='left'>&nbsp;</td>";
                }
                if (showLock ){
                    sHTML = sHTML + "<td width='15%' align='left'><a href=ApprovePayrollData.aspx?PayPeriod=" + Request["PayPeriod"] + "&ApproveAction=LOCKED&FID=" + fid + ">Lock</a></td>";
                }else{
                    sHTML = sHTML + "<td width='15%' align='left'>&nbsp;</td>";
                }

                sHTML = sHTML + "<td width='*' align='left'>" + rsFacList.Item("Name") + " - " + gotPayRollStatus;
                sHTML = sHTML + "</td>";
                if (nOpenCount > 0 ) {
                    sHTML = sHTML + "<td  width='15%' class='required' align='left'> &nbsp;&nbsp;&nbsp;&nbsp;Production Not Approved </td>";
                }else{
                    sHTML = sHTML + "<td width='15%' align='left'> &nbsp;</td>";
                }
                sHTML = sHTML + "</tr>";


           } //Loop

           sHTML = sHTML + "</table>";

           return sHTML;


            }
        }
}