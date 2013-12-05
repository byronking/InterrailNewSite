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

    public partial class FacilityAnnualEntry : PageBase
    {

        public string sField, sValue, i;
        public string[] facilityId = new string[100];
        public string[] facilityName = new string[100];
        public string[] loadTotal = new string[100];
        public string[] unloadTotal = new string[100];
        public string[] budgetedCPU = new string[100];
        public string[] miscellaneousCPU = new string[100];
        public string item, itemName;

        public string sMode;
        public string selYear;

        public string sFac;
        public string strUnLoad = "";
        public string sJan, sFeb, sMar, sApr, sMay, sJun, sJul, sAug, sSep, sOct, sNov, sDec;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

             GrantAccess("Super, Admin, User");

   
               sMode = Request["MODE"];
               selYear = Request["selYear4"];
               sFac = System.Convert.ToString(Session["FacilityID"]);
               Session["Year"] = selYear;


            if(sMode == "SAVE"){

              string strSQL, strLoad, strUnload, strBudgetedCPU, strMiscellaneousCPU;

              strSQL =   " DELETE FROM MonthlyBudgetPerc ";
              strSQL +=  " WHERE ReportingYear = '" + Request["selYear"] + "'; ";
              strSQL +=  " DELETE FROM FacilityAnnualBudget ";
              strSQL +=  " WHERE ReportingYear = '" + Request["selYear"] + "'; ";

              for( int x = 1; x < Request.Form.Count; x++){

                item = Request.Form.Keys[x];
	            if (Len(item) == 3){

                   sField  = item;
                   if( Trim(Request.Form[item]) != ""){
      	             sValue  = Request.Form[item];
                   }else{
      	             sValue  = "0";
                  }

                   strSQL +=  "";
                   strSQL +=  "INSERT INTO MonthlyBudgetPerc ";
                   strSQL +=  "   (ReportingYear, ReportingMonth, MonthlyPercentage ) ";
                   strSQL +=  "VALUES (";
                   strSQL +=  " '" + Request["selYear"] + "', '" + sField + "', " + sValue;
                   strSQL +=  "); ";

                }else{

		            if (InStr(0, item, "_",0) > 0) {

		              int iSection   = cInt(InStr(0, item, "_", 1) - 1);
		              int iField     = cInt(iSection + 2);
		              int iLen 	  = Len(Trim(item));
		              sField = Left(item, iSection);
		              string sFacilityId   = Mid(item, iField, iLen);

			              if( UCase(sField) == "LOAD") {
					            if (Trim(Request.Form[x]) != "") {
						            strLoad = Request.Form[x];
					            }else{
						            strLoad = "0";
					            }
					            if (Trim(Request.Form[x+1]) != ""){
						            strUnLoad = Request.Form[x+1];
					            }else{
						            strUnLoad = "0";
					            }
					            if(Trim(Request.Form[x+2]) != ""){
						            strBudgetedCPU = Request.Form[x+2];
					            }else{
						            strBudgetedCPU = "0";
					            }
					            if(Trim(Request.Form[x+3]) != ""){
						            strMiscellaneousCPU = Request.Form[x+3];
					            }else{
						            strMiscellaneousCPU = "0";
					            }

					            strSQL +=  "";
					            strSQL +=  "INSERT INTO FacilityAnnualBudget ";
					            strSQL +=  "   (FacilityId, LoadTotal, UnloadTotal, ReportingYear, BudgetedCPU, MiscellaneousCPU ) ";
					            strSQL +=  "VALUES (";
					            strSQL +=  " " + sFacilityId + ", " + strLoad + ", " + strUnLoad + ", '" + Request["selYear"] + "', " + strBudgetedCPU + ", " + strMiscellaneousCPU + " ";
					            strSQL +=  ")  ";
			             }
		            }
               }

              }


              this.Execute(strSQL);

              Response.Redirect("FacilityMonitor.aspx");

            }

}
        
        public DataReader getData(string sDataSection){

           DataReader rsData;
           string strSQL = "";

          if( sDataSection == "MONTHLYPERC" ){
	          strSQL =        "SELECT * ";
	          strSQL +=  " FROM MonthlyBudgetPerc ";
	          strSQL +=  "WHERE ReportingYear = '" + selYear + "' ";

          }else{
              if( sDataSection == "FACILITYBUDGET" ){

  		        strSQL = " Select Name As FacilityName, Id AS facilityId FROM Facility Order by Name";
              }
         }

          rsData = new DataReader(strSQL);

           rsData.Open();

          return rsData;

}

        public string FormatTheDate(DateTime inDate){

              string smDate, sdDate, syDate;

              smDate = Right(cStr( inDate.Month + 100), 2);
              sdDate = Right(cStr(inDate.Day + 100), 2);
              syDate = cStr(inDate.Year);
              return  smDate + "/" + sdDate + "/" + syDate;

        }

        public void ShowMonthlyPercentage(){

          DataReader rs;
          rs = getData("MONTHLYPERC");

          Response.Write("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
          Response.Write("<tr><td colspan='4' class='pageTitle' align='Left'><div class='cellBottomBorder'>Monthly Percentages</div>&nbsp;&nbsp;&nbsp;<font size=1>(Combined total of all months should !exceed 100%)</font></td></tr>");
          Response.Write("<tr><td width='100%' colspan='4'>&nbsp;</td></tr>");
  

          if (!rs.EOF){
            while (!rs.EOF){

              rs.Read();
              sField = rs.Item("ReportingMonth");
              string sDataValue = rs.Item("MonthlyPercentage");

              switch( sField){
                  case "jan" :
                      sJan    = sDataValue;
                      break;
                 case "feb" : 
                     sFeb    = sDataValue;
                      break;
                 case "mar" : 
                     sMar    = sDataValue;
                      break;
                 case "apr" : 
                     sApr    = sDataValue;
                      break;
                 case "may" : 
                     sMay    = sDataValue;
                      break;
                 case "jun" : 
                     sJun    = sDataValue;
                      break;
                 case "jul" : 
                     sJul    = sDataValue;
                      break;
                 case "aug" : 
                     sAug    = sDataValue;
                      break;
                 case "sep" : 
                     sSep    = sDataValue;
                      break;
                 case "oct" :
                     sOct    = sDataValue;
                      break;
                 case "nov" :
                     sNov    = sDataValue;
                      break;
                 case "dec" : 
                     sDec    = sDataValue;
                      break;
             }

            } //End Loop
 }

  Response.Write("<tr><td width='42%' align='right'>January: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='jan' value='" + sJan + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>February: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='feb' value='" + sFeb + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>March: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='mar' value='" + sMar + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>April: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='apr' value='" + sApr + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>May: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='may' value='" + sMay + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>June: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='jun' value='" + sJun + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>July: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='jul' value='" + sJul + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>August: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='aug' value='" + sAug + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>September: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='sep' value='" + sSep + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>October: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='oct' value='" + sOct + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>November: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='nov' value='" + sNov + "' maxlength=5 size=3>%</td></tr>");

  Response.Write("<tr><td width='42%' align='right'>December: &nbsp;</td><td width='58%' colspan='3' align='left'>");
  Response.Write("    <input type='text' name='dec' value='" + sDec + "' maxlength=5 size=3>%</td></tr>");

}

        public void ShowFacilityBudget(){

            DataReader rs;
            int  i;

              rs = getData("FACILITYBUDGET");

              Response.Write("<tr><td width='100%' colspan='12'>&nbsp;</td></tr>");
              Response.Write("<tr><td colspan='12' class='pageTitle' align='Left'><div class='cellBottomBorder'>Annual Budget</div>&nbsp;&nbsp;&nbsp;<font size=1></font></td></tr>");

              if (!rs.EOF){
                i = 0;
                while (!rs.EOF){
                    rs.Read();
                  facilityId[i] = rs.Item("facilityId");
                  facilityName[i] = rs.Item("facilityName");
                  i = i + 1;

                } //End Loop
             }
  


              Response.Write("<tr><td><b><u>FACILITY</u></b></td></tr>");
              Response.Write("<tr><td height='10'></td></tr>");

              for( i = 0; i < UBound(facilityId); i ++){
  	            if( facilityId[i] != null){
  		            Response.Write("<tr><td align='left' width='23%'><a href='FacilityAnnualEntryDetail.aspx?FID=" + facilityId[i] + "&selYear=" + selYear + "&FNAME=" + Server.UrlEncode(facilityName[i]) + "'>" + facilityName[i] + "</a></td>");
 
   	            }
              }

              Response.Write("<tr><td  height='7'></td></tr>");
              Response.Write("<tr><td  height='7'></td></tr>");

         }


    }
}