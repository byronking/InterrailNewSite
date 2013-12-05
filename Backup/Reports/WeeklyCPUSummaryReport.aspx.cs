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
    public partial class WeeklyCPUSummaryReport : PageBase
    {

        public DataReader rsPay;
        public DataReader rsTotalValue;
        public string rc;
        public string PageOneLines = "";
        public string selDateWeeklySummary;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            selDateWeeklySummary = Request["selDate"];

            // Get Total Pay for ALL Tasks;
  
            rsPay = new DataReader("EXECUTE WeeklyCPUSummary '" + selDateWeeklySummary + " '");
            rsPay.Open();

            //-------- PAY WILL BE FOR "ALL".... UNITS FOR "SP"-----------------;
            //**************** 6/14/2007 changed to statement below ************;
            rsTotalValue = new DataReader("EXECUTE WeeklyCPUSummaryTotal '" + selDateWeeklySummary + " '");
            rsTotalValue.Open();

            CreatePageOneLines();

       }

        public string FCurSP(string num)
        {
            string FCurSP = "";
            if (!(num == null))
            {
                if (isNumeric(cStr(num)))
                {
                    if (cDbl(num) != 0)
                    {
                        FCurSP = "$ " + cStr(FormatNumber(num, 2));
                    }
                }
            }

            return FCurSP;

        }

        public void WritePageOneLines(){
              Response.Write(PageOneLines);
        } 

        public void  CreatePageOneLines(){

          double SumTRCostPerUnit = 0;
          double SumTRPay = 0;
          double SumTRUnits = 0;
          double RowsWithData = 0;
          int i = 0;
          string rc = "";	

          while (!rsPay.EOF ){

            rsPay.Read();

	        if(i % 2 == 0){
                rc = "reportEvenLine";
             }else{
                rc = "reportOddLine";
            }
  
	         string TRDate = "";
	         string TRFacility = "";
	         string TRPay = "";
	         string TRUnits = "";
	         string TRCostPerUnit = "";
	         string TRVariance = "";
	         string TRDetermingFactor = "";
     
             TRFacility = rsPay.Fields(0);
             TRPay = rsPay.Fields(1);
             TRUnits = rsPay.Fields(2);
             TRCostPerUnit = rsPay.Fields(3);
             TRVariance = rsPay.Fields(4);
             TRDetermingFactor = rsPay.Fields(5);
     
             if(TRPay.Length > 0){
                TRPay = FCur(TRPay ,2);
             }else{
                TRPay = "";
            }

             if(isNumeric(TRCostPerUnit)){
                TRCostPerUnit = FCur(TRCostPerUnit ,2);
            }

             if(isNumeric(TRVariance)){
                TRVariance = FCur(TRVariance ,2);
            }

             if(isNumeric(TRUnits)){
                TRUnits = cStr(TRUnits);
            }
     
              PageOneLines = PageOneLines + "<tr class='" + rc + "'>";
              PageOneLines = PageOneLines + "  <td class=xl57font9 valign='top'>" + TRFacility + "</td>"      ;
              PageOneLines = PageOneLines + "<td class=xl43font9 style='border-left:none' align='right' valign='top'>";
              PageOneLines = PageOneLines +  TRPay;
              PageOneLines = PageOneLines + "</td>";
              PageOneLines = PageOneLines + "<td class=xl56font9 style='border-left:none' align='right' valign='top'>" + TRUnits + "</td>";
              PageOneLines = PageOneLines + "<td class=xl50font9 style='border-left:none' align='right' valign='top'>" + TRCostPerUnit;
              PageOneLines = PageOneLines + "</td>";
              PageOneLines = PageOneLines + "<td class=xl58font9 style='border-left:none' align='right' valign='top'>" + TRVariance + "</td>";
              PageOneLines = PageOneLines + "<td colspan=5 class=xl75font9 style='border-left:none' align='left' valign='top'>&nbsp;" + Trim(TRDetermingFactor) + "</td>";
              PageOneLines = PageOneLines + "<td colspan=5 style='mso-ignore:colspan' valign='top' bgcolor='white'></td>";
              PageOneLines = PageOneLines + "<td class=xl44 valign='top' bgcolor='white'></td>";
              PageOneLines = PageOneLines + "</tr>";
	

	        i = i + 1;
          } //End Loop
  
            // Total Values;
	        rsTotalValue.Requery();
            rsTotalValue.Read();

	        PageOneLines = PageOneLines + "<tr>";
            PageOneLines = PageOneLines + "  <td class=xl57font9 valign='top'><b>Weekly Total </b></td>";
            PageOneLines = PageOneLines + "<td class=xl43font9 style='border-left:none' align='right' valign='top'>";
            PageOneLines = PageOneLines +  FCur(cStr(SafeDbl(rsTotalValue.Fields(0))),2);
            PageOneLines = PageOneLines + "</td>";
            PageOneLines = PageOneLines + "<td class=xl56font9 style='border-left:none' align='right' valign='top'>" + rsTotalValue.Fields(1) + "</td>";
            PageOneLines = PageOneLines + "<td class=xl50font9 style='border-left:none' align='right' valign='top'>" + FCur(cStr(SafeDbl(rsTotalValue.Fields(2))),2);
            PageOneLines = PageOneLines + "</td>";
            PageOneLines = PageOneLines + "<td class=xl56font9 style='border-left:none' align='right' valign='top' bgcolor='white'>" + "</td>";
            PageOneLines = PageOneLines + "<td colspan=5 class=xl75font9 style='border-left:none' align='left' valign='top' bgcolor='white'>&nbsp;" + "</td>";
            PageOneLines = PageOneLines + "<td colspan=5 style='mso-ignore:colspan' valign='top' bgcolor='white'></td>";
            PageOneLines = PageOneLines + "<td class=xl44 valign='top' bgcolor='white'></td>";
            PageOneLines = PageOneLines + "</tr>";
  
  
        }


    }
}