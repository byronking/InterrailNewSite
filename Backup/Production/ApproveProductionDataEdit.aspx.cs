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
    public partial class ApproveProductionDataEdit : PageBase
    {

        public DataReader rs;
        public int rs_numRows = 0;
        public string sStatus = "";
        public string sApprovalStatus, sRecID, sWhere, sReturnTo;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);


            GrantAccess("Super, Admin, User, Production");


            sReturnTo = "&ReturnTo=%2FProduction%2FApproveProductionDataEdit%2Easp%3FID%3D" ;

            if(cStr(Request["Approval"]) != ""){
            sApprovalStatus = UCase(Trim(Request["Approval"]));
  
            if(cStr(Request["Which"]) != ""){
            if(cStr(Request["cbxApprove"]) != ""){
            if(cStr(Request["Which"]) == "CHECKED"){
            sWhere = " Where ID IN (0, " + Request["cbxApprove"] + ")";
            }else{
            sWhere = " Where ID IN (0)";
            }
            }else{ 
              sWhere = " Where ID IN (0)";
            }
	
              if(cStr(Request["Which"]) == "OPEN"){
              sWhere = " Where ApprovalStatus='OPEN' AND FacilityID = " + cStr(Session["FacilityID"]);
              if(cStr(Request["From"]) != ""){
                sWhere = sWhere + " AND  WorkDate Between '" + cStr(Request["From"] ) + "'AND '" + cStr(Request["To"]) + "'";
              }
              }
            }else{
            sWhere = " Where ID = " + Request["ID"];
            }

            string strSQL = " Update FacilityProductionDetail Set ";
            strSQL +=  "  ApprovalStatus = '" + sApprovalStatus     + "',";
            strSQL +=  "  LastModifiedOn = '" + cStr(System.DateTime.Now.ToShortDateString())  + "',";
            strSQL +=  "  LastModifiedBy = '" + Session["UserName"] + "' ";
            strSQL = strSQL + sWhere;


            this.Execute (strSQL);

            Response.Redirect("ApproveProductionData.aspx");
            }



            string rs__MMColParam;
            rs__MMColParam = "1";
            if (Request.QueryString["ID"] != "") { rs__MMColParam = System.Convert.ToString(Request.QueryString["ID"]); }



            rs = new DataReader("SELECT  dbo.FacilityProductionDetail.Id, dbo.FacilityProductionDetail.WorkDate, dbo.FacilityProductionDetail.ShiftID,  dbo.FacilityProductionDetail.RailCarNumber, dbo.FacilityProductionDetail.Units, dbo.FacilityProductionDetail.Notes,  dbo.FacilityProductionDetail.ApprovalStatus, dbo.FacilityProductionDetail.LastModifiedBy, dbo.FacilityProductionDetail.LastModifiedOn,                         dbo.IRGManufacturer.ManufacturerName, dbo.IRGManufacturer.ManufacturerCode, dbo.IRGOrigin.OriginCode, dbo.IRGOrigin.OriginName,                         dbo.IRGRailCarType.CarTypeCode, dbo.IRGRailCarType.CarTypeDescription, dbo.FacilityCustomer.CustomerCode,                         dbo.FacilityCustomer.CustomerName, dbo.Tasks.TaskCode, dbo.Tasks.TaskDescription, dbo.FacilityProductionDetail.LevelType,                         dbo.FacilityProductionDetail.NewUsed  FROM         dbo.FacilityProductionDetail INNER JOIN                        dbo.IRGManufacturer ON dbo.FacilityProductionDetail.ManufacturerID = dbo.IRGManufacturer.ID INNER JOIN                        dbo.IRGOrigin ON dbo.FacilityProductionDetail.OriginID = dbo.IRGOrigin.ID INNER JOIN                        dbo.IRGRailCarType ON dbo.FacilityProductionDetail.CarTypeID = dbo.IRGRailCarType.ID INNER JOIN                        dbo.FacilityCustomer ON dbo.FacilityProductionDetail.FacilityCustomerId = dbo.FacilityCustomer.Id INNER JOIN                        dbo.Tasks ON dbo.FacilityProductionDetail.TaskId = dbo.Tasks.Id  WHERE FacilityProductionDetail.Id = " + Replace(rs__MMColParam, "'", "''") + "");
            rs.Open();
            rs.Read();
            rs_numRows = 0;
            sStatus = UCase(Trim(rs.Item("ApprovalStatus")));


        }
    }
}