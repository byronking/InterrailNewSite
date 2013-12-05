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
    public partial class FacilityDataEntry : PageBase
    {

        public string sMode = "";
        public string selDate = "";
        public string sFac = "";
        public string selYear = "";
        public string selMonth = "";
        public string selDay = "";

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User");

            sMode = Request["MODE"];
            selDate  = Request["selDate"];
            sFac     = cStr(Session["FacilityID"]);

            Session["LastStartDate"] = FormatTheDate(cDate(selDate));

            selYear  = cStr(cDate(selDate).Year);
            selMonth = cStr(cDate(selDate).Month);
            selDay   = cStr(cDate(selDate).Day);

            if(sMode == "SAVE"){

              DataReader rsSave;
              string strSQL = "";
              string sSection,sField;
              int iSection,iField,iLen;

              strSQL =   " Delete From FacilityMonitoringDataEntry ";
              strSQL +=  " WHERE FacilityID = " + sFac;
              strSQL +=  "  AND WorkDate = '" + selDate + "' ";

              foreach (string item in Request.Form)       {
                  string[] splititems = item.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
                  if (splititems.Length > 1)
                  {
                      sSection = splititems[0];
                      sField = splititems[1];
              /*for( int x = 0; x < Request.Form.Count; x++){
                if( InStr(1, Request.Form.Keys[x], "__",0) > 0){
                  iSection   = cInt(InStr(1, Request.Form.Keys[x], "__", 1) - 1);
                  iField     = cInt(iSection + 3);
                  iLen = Len(Trim(Request.Form[x]));
                  sSection = Left(Request.Form[x], iSection);
                  sField   = Mid(Request.Form[x], iField, iLen);*/

                  strSQL +=  " ";
                  strSQL +=  " Insert Into FacilityMonitoringDataEntry ";
                  strSQL +=  "   (FacilityID, WorkDate, DataSection, FieldName, DataValue ) ";
                  strSQL +=  "VALUES (";
                  strSQL +=  " " + sFac + ", '" + selDate + "', '" + sSection + "', '" + sField + "', '" + Replace(Request[item],"'","''") + "'";
                  strSQL +=  ")";
               }
              }


              this.Execute(strSQL);
              //rsSave = new DataReader(strSQL);
              //rsSave.Open();
              Response.Redirect("FacilityMonitor.aspx");

            }

}

        ////////// ==============================================================================;
        ////////// ==============================================================================;
        ////////// ==============================================================================;

        public DataReader getData(string sDataSection){

            DataReader rsData;
            string strSQL;

            strSQL =        "SELECT * ";
            strSQL +=  " FROM FacilityMonitoringDataEntry ";
            strSQL +=  "WHERE FacilityID = " + sFac;
            strSQL +=  "  AND RTrim(LTrim(DataSection)) = '" + Trim(sDataSection) + "'";
            strSQL +=  "  AND DatePart(d,WorkDate)      = " + selDay;
            strSQL +=  "  AND DatePart(m,WorkDate)      = " + selMonth;
            strSQL +=  "  AND DatePart(yyyy,WorkDate)   = " + selYear;

            rsData = new DataReader(strSQL);
            rsData.Open();

            return rsData;

        }


        public void ShowSpotting(){

            DataReader rs;
            rs = getData("SPOTTING");

            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>SPOTTING</div></td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr class='pageTitle'><td width='42%' align='center'>Field Name&nbsp;&nbsp;</td><td width='58%' colspan='3' align='Left'>Value</td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");

            string sSpotTime    = "";
            string sRC_In       = "";
            string sRC_Out      = "";
            string sDown_Time   = "";
            string sHrs_Worked  = "";
            string sTotal_Pay   = "";

            if (!rs.EOF){
            while (!rs.EOF){
                rs.Read();
                string sField = UCase(rs.Item("FieldName"));
                string sDataValue = rs.Item("DataValue");

                switch (sField){
                    case "SPOTTIME": 
                        sSpotTime   = sDataValue;
                        break;
                    case "RC_IN":
                        sRC_In      = sDataValue;
                        break;
                    case "RC_OUT":      
                        sRC_Out     = sDataValue;
                        break;
                    case "DOWN_TIME":   
                        sDown_Time  = sDataValue;
                        break;
                }

            } //End Loop
            }

            Response.Write("<tr><td width='42%' align='right'>RailCar Spot Time: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name='SPOTTING__SPOTTIME' value='" + sSpotTime + "' maxlength=8 size=8></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>RailCars In: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=SPOTTING__RC_IN value='" + sRC_In + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>RailCars Out: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=SPOTTING__RC_OUT value='" + sRC_Out + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>Down Time: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=SPOTTING__DOWN_TIME value='" + sDown_Time + "' maxlenght=20 size=20></td></tr>");


        }


        public void ShowKPIs(){

            DataReader rs;
            rs = getData("KPI");

            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>KPI's</div></td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr class='pageTitle'><td width='42%' align='center'>Field Name&nbsp;&nbsp;</td><td width='58%' colspan='3' align='left'>Value</td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");

        string sAccidents    = "";
        string sRep_Injuries = "";
        string sLost_Time    = "";
        //sNon_Medical  = "";
        string sFirst_Aid  = "";

            if (!rs.EOF){

            while (!rs.EOF){
                rs.Read();
                string sField = UCase(rs.Item("FieldName"));
                string sDataValue = rs.Item("DataValue");

                switch(sField){
                    case "ACCIDENTS":          
                        sAccidents    = sDataValue;
                        break;
                    case "REP_INJURIES":      
                        sRep_Injuries = sDataValue;
                        break;
                    case "LOST_TIME":         
                        sLost_Time    = sDataValue;
                        break;
                    case "NON_MEDICAL":       
                        sFirst_Aid    = sDataValue;
                        break;
                }

            } //End Loop
            }

            Response.Write("<tr><td width='42%' align='right'>Accidents: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=KPI__ACCIDENTS value='" + sAccidents + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>Reportable Injuries: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=KPI__REP_INJURIES value='" + sRep_Injuries + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>Lost Time: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=KPI__LOST_TIME value='" + sLost_Time + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>1st Aid: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=KPI__NON_MEDICAL value='" + sFirst_Aid + "' maxlenght=20 size=20></td></tr>");

        }


        public void ShowSelfAudits(){

            DataReader rs;
            rs = getData("SELF_AUDITS");

            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>SELF AUDITS</div></td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr class='pageTitle'><td width='42%' align='center'>Field Name&nbsp;&nbsp;</td><td width='58%' colspan='3' align='left'>Value</td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");

        string sBay      = "";
        string sFacility = "";

            if (!rs.EOF){
            while (!rs.EOF){
                rs.Read();
                string sField = UCase(rs.Item("FieldName"));
                string sDataValue = rs.Item("DataValue");

                switch(sField){
                    case "BAY" :        
                    sBay    = sDataValue;
                    break;
                    case "FACILITY":     
                    sFacility = sDataValue;
                    break;
                }

            } //End Loop
            }

            Response.Write("<tr><td width='42%' align='right'>Bay: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=SELF_AUDITS__BAY value='" + sBay + "' maxlength=20 size=20></td></tr>");

            Response.Write("<tr><td width='42%' align='right'>Facility: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=SELF_AUDITS__FACILITY value='" + sFacility + "' maxlength=20 size=20></td></tr>");

        }


        public void ShowDetermingFactors(){

            DataReader rs;
            rs = getData("FACTORS");

            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>DETERMINING FACTORS</div>&nbsp;&nbsp;&nbsp;<font size=1>(Only enter on last day of the reporting week.)</font></td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr class='pageTitle'><td width='42%' align='center'>Field Name&nbsp;&nbsp;</td><td width='58%' colspan='3' align='left'>Value</td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");

            string sFactor = "";

            if (!rs.EOF){
            while (!rs.EOF){
                rs.Read();
                string sField = UCase(rs.Item("FieldName"));
                string sDataValue = rs.Item("DataValue");

                switch(sField){
                    case "DETERMINING_FACTOR":
                    sFactor = sDataValue;
                    break;
                }

            } //End Loop
            }

            Response.Write("<tr><td colspan='3' align='left'><table border=0 cellpadding=0 cellspacing=0 width='100%'><tr><td width=50></td><td align=left>Possible Determining Factors for Variances: &nbsp;</td></tr>");
            Response.Write("<tr><td width=50><img src='../images/spacer.gif' width=50></td><td align='left'><TEXTAREA NAME=FACTORS__DETERMINING_FACTOR COLS=40 ROWS=3 onkeyup='CheckChars(this.value)' >" + sFactor + "</TEXTAREA></td></tr>");
            Response.Write("<tr><td width=50></td><td><div id='divCharCount' name='divCharCount'></div></td></tr></table></td></tr>");

        }


        public void ShowEmployees(){

            DataReader rs;
            rs = getData("EMPLOYEES");

            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>EMPLOYEES</div>&nbsp;&nbsp;&nbsp;<font size=1>(Only enter on first day of the reporting year.)</font></td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
            Response.Write("<tr class='pageTitle'><td width='42%' align='center'>Field Name&nbsp;&nbsp;</td><td width='58%' colspan='3' align='left'>Value</td></tr>");
            Response.Write ("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");

            string sBML      = "";

            if (!rs.EOF){
            while (!rs.EOF){
                rs.Read();
                string sField = UCase(rs.Item("FieldName"));
                string sDataValue = rs.Item("DataValue");

                switch(sField){
                    case "BUD_MISC_LABOR" :
                    sBML    = sDataValue;
                    break;
                }

            } //End Loop
            }

            Response.Write("<tr><td width='42%' align='right'>Budgeted Misc. Labor: &nbsp;</td><td width='58%' colspan='3' align='left'>");
            Response.Write("    <input type='text' name=EMPLOYEES__BUD_MISC_LABOR value='" + sBML + "' maxlength=20 size=20></td></tr>");

        }


    }
}