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
    public partial class Calc : PageBase
    {

        public string rsEmp__PSTART;
        public string rsEmp__PEND;
        //public string strSQL;
        //public DataTableReader rsEmp;
        //public DataTableReader rsOTType;
        //public string FacilityOTType;
        public string StatusMessage;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender,e);

            GrantAccess("Super, Admin, User");


        StatusMessage = "";
        string[] arStartEnd;
        bool isDoCalc;
        string paystatus;
        string MM_Main_STRING = "";


        MM_Main_STRING = HttpContext.Current.Session["dbPath"].ToString();
     
        if(InStr(0,System.Convert.ToString(Request["PayPeriod"]) , ",",0) > 0 ){
           arStartEnd = Split(Request["PayPeriod"] , ",", 2);
         
           if(isArray(arStartEnd) ){
              rsEmp__PSTART = arStartEnd[0];
              rsEmp__PEND = arStartEnd[1];
           }
        }

        if(System.Convert.ToString(Request["DoCalc"]) == "Yes"  ){
          isDoCalc = true;
        }else{
          isDoCalc = false;
        }


        if(isDoCalc && isDate(rsEmp__PSTART) && isDate(rsEmp__PEND) ){
          paystatus = getPayRollStatus(rsEmp__PSTART, Session["FacilityID"].ToString());

          if(paystatus == "LOCKED" ){
              isDoCalc = false;
              StatusMessage = " The Pay Period " + cStr(rsEmp__PSTART) + " - " + cStr(rsEmp__PEND) + " is Locked and can not be Calculated. ";
          }
        }


        if(isDoCalc  && isDate(rsEmp__PSTART) && isDate(rsEmp__PEND) ){


          Response.Redirect( "CalcWait.aspx?StartDate=" + cStr(rsEmp__PSTART) + "&EndDate=" + cStr(rsEmp__PEND) + "&ConnectString=" + Replace(MM_Main_STRING, "Provider=SQLOLEDB;", "") + "&FacilityID=" +  Session["FacilityID"] + "&User=" + Session["UserName"]);  
          Response.End();

        } // do calc

        }

    }
}