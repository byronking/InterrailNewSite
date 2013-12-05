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
    public partial class TestPayPeriod : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

        }

        public string getTPayPeriods(int nStart,int nPeriods,string strSelectStartDate,string d){

           DateTime LastEnd;
           DateTime LastStart;
           string strHTML;
           int sDay;
           string strDay;
           int sMon;
           string strMon;
           string sFrom;
           string sTo;

           LastStart = cDate(getStartPay(d));
           LastEnd = LastStart.AddDays(6);
   
           strHTML = "";
   
           for( int i = nStart;  i < nPeriods; i++){

              sDay = LastStart.AddDays(-(7*i)).Day  ;
                  if(sDay < 10 ){
                    strDay = "0" + cStr(sDay);
                  }else{
                    strDay = cStr(sDay);
                  }

              sMon = LastStart.AddDays(-(7*i)).Month ;
                  if(sMon < 10 ){
                    strMon = "0" + cStr(sMon);
                  }else{
                    strMon = cStr(sMon);
                  }

                  sFrom =  sMon + "/" + sDay + "/" + cStr(LastStart.AddDays(-(7*i)).Year);

              sDay = LastEnd.AddDays(-(7*i)).Day;
                  if(sDay < 10 ){
                    strDay = "0" + cStr(sDay);
                  }else{
                    strDay = cStr(sDay);
                  }

              sMon = LastEnd.AddDays(-(7*i)).Month;
                  if(sMon < 10 ){
                    strMon = "0" + cStr(sMon);
                  }else{
                    strMon = cStr(sMon);
                  }
          
                  sTo =  sMon + "/" + sDay + "/" + cStr( LastEnd.AddDays(-(7*i)).Year );
      
                string strSelect = " ";

                if(isDate(strSelectStartDate) ){
                if((cDate(sFrom) <= cDate(strSelectStartDate))  &&  (cDate(strSelectStartDate) <= cDate(sTo)) ){
                    strSelect = " SELECTED ";
                }
                }

              strHTML = strHTML + "<option value='" + sFrom + "," + sTo + "'" + strSelect + ">" + sFrom + " - " + sTo + "</option>" ;
           }  
   
           return strHTML;
        }

        public string gettest(){

          DateTime sd = cDate("12/1/2001");
          DateTime ed = cDate("12/31/2001");
  
  
          string s = "<tr><td>Date</td><td>GetStartPay(d)</td><td>getPayPeriods(0,3,\"\"\"\")</td></tr>";
          
          while(sd.CompareTo(ed) == 1) {

              string d = sd.ToShortDateString();
 
             s = s + "<tr><td>" + cStr(d) + "</td>";
             s = s + "<td>" + getStartPay(d) + "</td>";
             s = s + "<td><select>" + getTPayPeriods(0, 3, "", d) + "</select></td></tr>";

             sd.AddDays(1);

          } 
  
  
          return s ;

        }

    }
}