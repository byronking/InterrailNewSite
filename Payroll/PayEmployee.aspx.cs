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
    public partial class PayEmployee : PageBase
    {

        public string outoftowntype;
        public string sql;
        public string strSQL;
        public string sTaskOptionList;
        public DataReader rsT;
        public DataReader rsTaskName;
        public DataReader rsUnitCount;
        public DataReader rsEmpWorked;
        public DataReader rsTeams;
        public DataReader rsEmp;

        public string PFacilityID;
        public string PWorkDate;
        public string PShift;
        public int PTask;
        public string strBackTo;
        public string PisOtherTask;
        public string PNewTask;
        public string strUnitCount;
        public string strTaskCode;
        public string AllTeamMembers;
        public string TeamMembers;


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            GrantAccess("Super, Admin, User");

            if (Request["ActionType"] == "UPDATE")
            {
                string Insert_Query = "";

                if (Request["TeamMembers"].ToString() == "0")
                {
                    // Employee task worked records will not be generated
                }
                else
                {
                    string[] arEmp;
                    //int nHours;

                    arEmp = Request["TeamMembers"].ToString().Split(',');

                    for (int i = 0; i < arEmp.Length; i++)
                    {
                        if (isOutofTown(System.Convert.ToInt32(arEmp[i]), System.Convert.ToInt32(Session["FacilityID"])))
                        {
                            outoftowntype = "O";
                        }
                        else
                        {
                            outoftowntype = "N";
                        }

                        Insert_Query = Insert_Query + " Insert Into EmployeeTaskWorked ";
                        if (Request["OtherTask"] == "YES")
                        {
                            Insert_Query = Insert_Query + "   (OtherTaskID, ";
                        }
                        else
                        {
                            Insert_Query = Insert_Query + "   (TaskID, ";
                        }
                        Insert_Query = Insert_Query + "         FacilityID, EmployeeID, RebillDetailID, WorkDate, ShiftID, OutOfTownType,  ";
                        Insert_Query = Insert_Query + "    HoursWorked, UPM, PayrollStatus, LastModifiedBy, LastModifiedOn, Notes ) ";
                        Insert_Query = Insert_Query + "    VALUES ( ";
                        Insert_Query = Insert_Query + "      " + Request["Task"] + " , " + cStr(Session["FacilityID"]) + " , " + cStr(arEmp[i]) + ",  0, ";
                        Insert_Query = Insert_Query + "     '" + Request["WorkDate"] + "','" + Request["Shift"] + "', '" + outoftowntype + "', " + Request["Hours"] + ", " + Request["UPM"] + ", ";
                        Insert_Query = Insert_Query + "     'OPEN', '" + Request["LastModifiedBy"] + "', '" + Request["LastModifiedOn"] + "', '')";
                    }

                    this.Execute(Insert_Query);

                }

            } //action type update 


            if (Request["ActionType"] == "RECALC" || Request["ActionType"] == "UPDATE")
            {

                UpdateUPM(Request["WorkDate"].ToString(), System.Convert.ToInt32(Request["Task"]), Request["Shift"].ToString(), System.Convert.ToInt32(Session["FacilityID"]), 0);

            } //action type Recalc

            if (Request["NewTask"] != null && Request["NewTask"].ToString() == "Yes")
            {
                // get task list
                sql = "";
                sql = sql + " SELECT  dbo.Tasks.TaskCode, dbo.Tasks.Id ";
                sql = sql + " FROM    dbo.Tasks INNER JOIN ";
                sql = sql + "         dbo.FacilityTasks ON dbo.Tasks.Id = dbo.FacilityTasks.TaskId ";
                sql = sql + " WHERE   dbo.Tasks.Rebillable = 0 and dbo.FacilityTasks.FacilityID = " + cStr(Session["FacilityID"]);

                rsT = new DataReader(sql);
                rsT.Open();

                sTaskOptionList = "";
                while (rsT.Read())
                {
                    sTaskOptionList = sTaskOptionList + "<option value=\"" + cStr(rsT.Item("ID")) + "\">" + rsT.Item("TaskCode") + "</option>";
                }
            }

            if (Request["ReturnTo"] != null && Request["ReturnTo"].ToString().Length > 0)
            {
                strBackTo = Request["ReturnTo"].ToString();
            }
            else
            {
                strBackTo = Server.UrlEncode(Request.ServerVariables["URL"] + "?" + Request.ServerVariables["QUERY_STRING"]);
            }

            PFacilityID = Session["FacilityID"].ToString();
            PWorkDate = Request["WorkDate"];
            PShift = Request["Shift"];
            PTask = System.Convert.ToInt32(Request["Task"]);
            PisOtherTask = Request["OtherTask"];
            PNewTask = Request["NewTask"];


            if (Request["NewTask"] != null && Request["NewTask"].ToString() == "Yes")
            {
                PTask = 0;
                strUnitCount = "0";
            }
            else
            {

                sql = "";
                if (PisOtherTask == "YES")
                {
                    sql = sql + " SELECT     TaskCode From OtherTasks where ID = " + PTask;
                }
                else
                {
                    sql = sql + " SELECT     TaskCode From Tasks where ID = " + PTask;
                }

                rsTaskName =  new DataReader(sql);
                rsTaskName.Open();

                strTaskCode = "";
                if (rsTaskName.Read())
                {
                    strTaskCode = rsTaskName.Item("TaskCode").ToString();
                }

                sql = "";
                sql = sql + " SELECT  isNull(SUM(Units), 0) as UnitCount from FacilityProductionDetail ";
                sql = sql + " WHERE (TaskID = " + PTask + ") AND (FacilityID = " + PFacilityID + ") AND (WorkDate = '" + PWorkDate + "') AND (ShiftID = '" + PShift + "')  ";

                rsUnitCount = new DataReader(sql);
                rsUnitCount.Open();

                strUnitCount = "0";
                if (rsUnitCount.Read())
                {
                    strUnitCount = System.Convert.ToString(rsUnitCount.Item("UnitCount"));
                }

                sql = "";
                sql = sql + " SELECT     ";
                if (PisOtherTask == "YES")
                {
                    sql = sql + "  etw.OtherTaskID, 0 as TaskID, ";
                }
                else
                {
                    sql = sql + "  etw.TaskID, ";
                }

                sql = sql + "  e.EmployeeNumber, e.TempNumber, e.LastName, e.FirstName,   ";
                sql = sql + "            etw.FacilityID, etw.WorkDate,  ";
                sql = sql + "            etw.ShiftID, etw.UPM, etw.HoursWorked,  ";
                sql = sql + "            etw.Notes, e.TempEmployee, etw.Id, etw.EmployeeId, ";
                sql = sql + "            ENum = Case ";
                sql = sql + "                when  e.TempEmployee = 0 then e.EmployeeNumber  ";
                sql = sql + "              else ";
                sql = sql + "                e.TempNumber  ";
                sql = sql + "              end  ";
                sql = sql + " FROM  dbo.EmployeeTaskWorked etw INNER JOIN ";
                sql = sql + "       dbo.Employee e ON etw.EmployeeId = e.Id ";
                sql = sql + " WHERE ";

                if (PisOtherTask == "YES")
                {
                    sql = sql + "    (etw.OtherTaskID = " + PTask + ") ";
                }
                else
                {
                    sql = sql + "    (etw.TaskID = " + PTask + ") ";
                }

                sql = sql + "    AND (etw.FacilityID = " + PFacilityID + ") AND (etw.WorkDate = '" + PWorkDate + "') AND (etw.ShiftID = '" + PShift + "')  ";

                rsEmpWorked = new DataReader(sql);
                rsEmpWorked.Open();
            }


            string rsTeams__PFacID;
            rsTeams__PFacID = "0";

            if (Session["FacilityID"].ToString() != "") { rsTeams__PFacID = Session["FacilityID"].ToString(); }

            rsTeams = new DataReader("SELECT ID=0, TeamName='  Select Members', TeamMembers='0' UNION   SELECT ID, TeamName, TeamMembers  FROM dbo.Teams  WHERE FacilityID=" + rsTeams__PFacID.Replace("'", "''") + " AND Teams.Active=1  ORDER BY TeamName ASC");
            rsTeams.Open();
            
            if (rsTeams.EOF)
            {
                TeamMembers = "0";
            }
            else
            {
                rsTeams.Read();
                TeamMembers = rsTeams.Item("TeamMembers").ToString();
            }

            AllTeamMembers = "";

            while ( rsTeams.LastReadSuccess)
            {
                if ((AllTeamMembers.Length > 0) && (rsTeams.Item("TeamMembers").Trim().Length > 0))
                {
                    AllTeamMembers = AllTeamMembers + ", ";
                }
                AllTeamMembers = AllTeamMembers + rsTeams.Item("TeamMembers");
                rsTeams.Read();
            }
            rsTeams.Requery();

            string rsEmp__PFACID;
            rsEmp__PFACID = "2";

            if (Session["FacilityID"].ToString() != "")
            {

                rsEmp__PFACID = Session["FacilityID"].ToString();

                strSQL = ""; 
                strSQL +=  "SELECT ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) Then 0 Else 1 End  ), FacilityID, Employee.Id,  LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 Then EmployeeNumber Else TempNumber End  FROM Employee   WHERE ( (FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " )  OR  ( FacilityID IN ( Select AssociatedFacilityID from AssociatedFacility where FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + "))) AND (Employee.Active=1) ";
                strSQL +=  "union ";
                strSQL +=  "SELECT Distinct ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) Then 0 Else 1 End  ), FacilityID, Employee.Id,  LastName, FirstName, EmployeeNumber = Case When TempEmployee=0 Then EmployeeNumber Else TempNumber End  FROM Employee  WHERE (Employee.Active=1 and employee.id in ";
                strSQL +=  "  ( ";
                strSQL = strSQL + AllTeamMembers;
                strSQL +=  "  ) ";
                strSQL +=  ")  ORDER BY ( Case When ( FacilityID = " + Replace(rsEmp__PFACID, "'", "''") + " ) Then 0 Else 1 End  ),FacilityID, LastName, FirstName, EmployeeNumber ";


                rsEmp = new DataReader(strSQL);
                rsEmp.Open();

            }
        }

        /// <summary>
        /// This gets the employee hourly rate. If the employee is not local, it gets the employee's home rate.
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="wd"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        public double  getUnitRate(string tid, string wd, string eid)
        {
            string strSQL = string.Empty;
            DataReader localRS;

            // Check the home facility of the employee
            var facilityIdQuery = "";
            facilityIdQuery += "select facilityId from employee where id = " + eid;

            var resultSet = new DataReader(facilityIdQuery);
            resultSet.Open();

            var facilityId = 0;

            if (resultSet.Read())
            {
                facilityId = Convert.ToInt32(resultSet.Item("FacilityId"));
            }

            if (facilityId.ToString() == PFacilityID)
            {
                // Get the rate for workers of the facility
                strSQL = "";
                strSQL += "Select Top 1 isNull(HoursPayRate, 0) as HoursPayRate, isNull(UnitsPayRate, 0) as UnitsPayRate from EmployeeRates ";
                strSQL += "WHERE  FacilityID = " + PFacilityID;
                strSQL += "  AND  TaskID = " + tid;
                strSQL += "  AND  ShiftID = " + PShift;
                strSQL += "  AND  EmployeeID = " + eid;
                strSQL += "  AND  EffectiveDate <= '" + wd + "' ";
                strSQL += " order by  EffectiveDate DESC ";
            }
            else
            {
                // Get the rate for out-of-town workers.
                strSQL = "";
                strSQL += "Select Top 1 isNull(HoursPayRate, 0) as HoursPayRate, isNull(UnitsPayRate, 0) as UnitsPayRate from EmployeeRates ";
                strSQL += "WHERE  FacilityID = " + facilityId;
                strSQL += "  AND  TaskID = " + tid;
                strSQL += "  AND  ShiftID = " + PShift;
                strSQL += "  AND  EmployeeID = " + eid;
                strSQL += "  AND  EffectiveDate <= '" + wd + "' ";
                strSQL += " order by  EffectiveDate DESC ";
            }
    
             localRS = new DataReader(strSQL);
             localRS.Open();

            if (localRS.Read())
            {
               return cDbl(localRS.Item("UnitsPayRate"));
            }
            else
            {
                return 0.0;
            }
        }
    }
}