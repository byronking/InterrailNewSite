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
    public partial class FacilityCorporateUnits : PageBase
    {

        public DataReader rsUnits;
        public double unLoadTotalUnits = 0.0;
        public double LoadTotalUnits = 0.0;
        public double TotalUnits = 0.0;
        public double count = 0.0;
        public string selFrom,selTo;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            selFrom = Request["selFrom"];
            selTo = Request["selTo"];

            //;
            // Get Total Units for UNLOAD;
            //;
            rsUnits = new DataReader(getSQL(selFrom, selTo));
            rsUnits.Open();


        }

        public string getSQL(string sFrom,string sTo){

              string strSQL;

		            strSQL = " SELECT IRGCompany.CompanyID, IRGCompany.CompanyName, Facility.FacilityNumber, ";
		            strSQL += " Facility.AlphaCode, Facility.Name, ";
		            strSQL += " isNull((select sum(units) from FacilityProductionDetail, Tasks ";
		            strSQL += " where FacilityProductionDetail.TaskId = Tasks.Id AND TaskCode = 'LO' ";
		            strSQL += " AND FacilityID = Facility.ID AND WorkDate ";
		            strSQL += " BETWEEN '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ),0) as LoadCount, "      ;
		            strSQL +="  IsNull((select sum(units) from FacilityProductionDetail, Tasks ";
		            strSQL += " where FacilityProductionDetail.TaskId = Tasks.Id AND TaskCode = 'UL' ";
		            strSQL += " AND FacilityID = Facility.ID AND WorkDate ";
		            strSQL += " BETWEEN '" + cStr(sFrom) + "' AND '" + cStr(sTo) + "' ),0) as UnloadCount ";
		            strSQL += " from IRGCompany INNER JOIN Facility ON IRGCompany.Id = Facility.IRGCompanyId ";
		            strSQL += " where IRGCompany.Active = 1 and Facility.Active =1 ";
		            strSQL += " order by IRGCompany.CompanyName,Facility.Name";
	
              return strSQL;

            }

        public void CreateUnitsData(){

                Response.Write("<table border='0' cellspacing='0' align='center' cellpadding='0' width='550'>");

                //Set Company and write first row and column heading;
                string CompanyId;

                //Response.Write("<tr><td colspan=4 align=//left//><b>" + rsUnits.Fields(1 + "</b><br>&nbsp;</td>  </tr>");
	            Response.Write("<tr><td align='Left'   width='35%' class='cellTopBottomBorder'>Company</td>");
	            Response.Write("<td align='Left'   width='22%' class='cellTopBottomBorder'>Facility</td>");
                Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Load</td>");
                Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Unload</td>");
                Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Total</td></tr>");
	
	
	            //initiate company fields;
	            double CompanyUnloadTotal = 0.0;
	            double CompanyLoadTotal = 0.0;
	            double CompanyUnitTotal = 0.0;
	            string sumrowColor;
                int sumiRow = 0;

                bool isNotEOF = rsUnits.Read();
                while (isNotEOF){

                    CompanyId = rsUnits.Fields(0);

                    sumiRow += 1;

    	            if(sumiRow % 2 == 0){
			            sumrowColor = "reportOddLine";
    	            }else{
			            sumrowColor = "reportEvenLine";
		            }

				
		            double FacilityTotal = 0.0;
				
		            //Write Facility row;
		            Response.Write(" <tr class='" + sumrowColor + "'>");
		
		            if (count == 0.0) {

                        var field5 = rsUnits.Fields(5);
                        var field6 = rsUnits.Fields(6);

			            Response.Write("<td class='cellTopLine' align='left' width='35%'>" + rsUnits.Fields(1) + "</td>");
			            Response.Write("<td class='cellTopLine' align='left' width='22%'>" + rsUnits.Fields(4) + "</td>");
			            Response.Write("<td class='cellTopLine' align='right' width='14%'>" + SafeDbl(rsUnits.Fields(5)).ToString("0") + "</td>");
			            Response.Write("<td class='cellTopLine' align='right' width='14%'>" + SafeDbl(rsUnits.Fields(6)).ToString("0") + "</td>");
                        FacilityTotal = Convert.ToDouble(rsUnits.Fields(5)) + Convert.ToDouble(rsUnits.Fields(6));
			            Response.Write("<td class='cellTopLine' align='right' width='14%'>" + SafeDbl(cStr(FacilityTotal)).ToString("0") + "</td>");
			            Response.Write("</tr>");
		
		            }else{
			            Response.Write("<td align='left' width='35%'>&nbsp;</td>");// + rsUnits.Fields(1) + "</td>");
			            Response.Write("<td align='left' width='22%'>" + rsUnits.Fields(4) + "</td>");
			            Response.Write("<td align='right' width='14%'>" + SafeDbl(rsUnits.Fields(5)).ToString("0") + "</td>");
			            Response.Write("<td align='right' width='14%'>" + SafeDbl(rsUnits.Fields(6)).ToString("0") + "</td>");
                        FacilityTotal = Convert.ToDouble(rsUnits.Fields(5)) + Convert.ToDouble(rsUnits.Fields(6));
			            Response.Write("<td align='right' width='14%'>" + SafeDbl(cStr(FacilityTotal)).ToString("0") + "</td>");
			            Response.Write("</tr>");
		            }
		
		            //Add to Totals;
		            CompanyLoadTotal = SafeDbl(cStr(CompanyLoadTotal)) + SafeDbl(rsUnits.Fields(5));
		            CompanyUnloadTotal = SafeDbl(cStr(CompanyUnloadTotal)) + SafeDbl(rsUnits.Fields(6));
		            CompanyUnitTotal = SafeDbl(cStr(CompanyUnitTotal)) + SafeDbl(cStr(FacilityTotal));
				
		            //Add to Report Total;
		            LoadTotalUnits = SafeDbl(cStr(LoadTotalUnits)) + SafeDbl(rsUnits.Fields(5));
		            unLoadTotalUnits = SafeDbl(cStr(unLoadTotalUnits)) + SafeDbl(rsUnits.Fields(6));
		            TotalUnits = SafeDbl(cStr(TotalUnits)) + SafeDbl(cStr(FacilityTotal));
		
		            //increment counter;
		            count = count + 1;
				
		            //move next	;

                    isNotEOF = rsUnits.Read();
		            //check for new company, if new write old Totals;
		            if (isNotEOF ){
			            if (rsUnits.Fields(0) != CompanyId){
			
				            if (count != 1){
					            //Write Totals for current company;
					            Response.Write("<tr class='reportTotalLine'>");
					            Response.Write("<td align='left' width='35%' class='cellTopBottomBorder'>&nbsp;</td>");
					            Response.Write("<td align='left' width='22%' class='cellTopBottomBorder'>&nbsp;</td>");
					            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyLoadTotal)).ToString("0") + "</td>");
					            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyUnloadTotal)).ToString("0") + "</td>");
					            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyUnitTotal)).ToString("0") + "</td>");
					            Response.Write("</tr>");
									
					            sumiRow = 0;
					
				            }
				            CompanyId = rsUnits.Fields(0);
				            count = 0.0;
				            CompanyUnloadTotal = 0.0;
				            CompanyLoadTotal = 0.0;
				            CompanyUnitTotal = 0.0;
			            }
		            }else{

			            if (count != 1){
				            //Write Totals for last company;
				            Response.Write("<tr class='reportTotalLine'>");
				            Response.Write("<td align='left' width='35%' class='cellTopBottomBorder'>&nbsp;</td>");
				            Response.Write("<td align='left' width='22%' class='cellTopBottomBorder'>&nbsp;</td>");
				            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyLoadTotal)).ToString("0") + "</td>");
				            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyUnloadTotal)).ToString("0") + "</td>");
				            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(CompanyUnitTotal)).ToString("0") + "</td>");
				            Response.Write("</tr>");
			
			            }
		            }
	  		
              } //End Loop
  
              //Write Final Summary;
  		            Response.Write("<tr><td align='Left'   width='35%' class='cellTopBottomBorder'>&nbsp;</td>");
		            Response.Write("<td align='Left'   width='22%' class='cellTopBottomBorder'>&nbsp;</td>");
    	            Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Load</td>");
    	            Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Unload</td>");
    	            Response.Write("<td align='right'  width='14%' class='cellTopBottomBorder'>Total</td></tr>");
		            Response.Write("<tr><td align='left' width='35%' class='cellTopBottomBorder'>&nbsp;</td>");
		            Response.Write("<td align='left' width='22%' class='cellTopBottomBorder'>&nbsp;</td>");
		            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(LoadTotalUnits)).ToString("0") + "</td>");
		            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(unLoadTotalUnits)).ToString("0") + "</td>");
		            Response.Write("<td align='right' width='14%' class='cellTopBottomBorder'>" + SafeDbl(cStr(TotalUnits)).ToString("0") + "</td></tr>");
		            Response.Write("</tr><tr><td colspan=5><br>&nbsp;</td></tr></table>");

            }
    }
}