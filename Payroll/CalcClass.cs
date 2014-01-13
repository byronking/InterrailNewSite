//#define TRACE
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;



namespace InterrailPPRS.Payroll
{
	/// <summary>
	/// Summary description for CalcClass.
	/// </summary>
	/// 
    public class PayItem
    {
       public TaskWorkedForCalc Parent;
       public double HoursPayed = 0.0;
       public double PayRate = 0.0;
       public double PayMultiplier = 0.0;
       public double PayAmount = 0.0;

        public PayItem(TaskWorkedForCalc p, double mux, double hours, double amount, double rate)
        {
            Parent = p;
            PayRate = rate;
            PayMultiplier = mux;
            HoursPayed = hours;
            PayAmount = amount;
        }

        public override string  ToString()
        {
            string s = "";
            s = s + " Hours: " + HoursPayed.ToString() + ", ";
            s = s + " Mux: " + PayMultiplier.ToString() + ", ";
            s = s + " Pay: " + PayAmount.ToString() + " ";
            return s;
        }
    }

	public class TaskWorkedForCalc 
	{

		public int Id;
		public int TaskID;
		public int OtherTaskID;
		public int FacilityID;
		public int EmployeeId;
		public DateTime WorkDate;
		public string ShiftID;
        public int Rebillable;
		public double UPM;
		public double HoursWorked;
		public string OutOfTownType;
		public string PayrollStatus;
		public string PayType;
        public double UnitPayRate;
        public double HourPayRate;
        public double RebillUnitPayRate;
        public double RebillHourPayRate;
        public double RebillSubTaskUnitPayRate;
        public double RebillSubTaskHourPayRate;

        public DayWorked Parent;
		public ArrayList PayList;

        public TaskWorkedForCalc(DayWorked p, SqlDataReader reader)
		{
             Parent = p;

			 Id            =   (int)     reader["Id"];            
			 TaskID        =   (int)     reader["TaskID"];
			 OtherTaskID   =   (int)     reader["OtherTaskID"];   
			 FacilityID    =   (int)     reader["FacilityID"];    
			 EmployeeId    =   (int)     reader["EmployeeId"];    
			 WorkDate      =   System.DateTime.Parse(reader["WorkDate"].ToString());      
			 ShiftID       =   (string)     reader["ShiftID"];       
             Rebillable    =   (int)     reader["Rebillable"];
             
            if (reader["UPM"].ToString() == string.Empty)
             {
                 UPM = 0.0;
             }
             else
             {
                 UPM = (double)System.Convert.ToDouble(reader["UPM"]);
             }
			 HoursWorked   =   (double)   System.Convert.ToDouble(reader["HoursWorked"]);   
			 OutOfTownType =   (string)  reader["OutOfTownType"]; 
			 PayrollStatus =   (string)  reader["PayrollStatus"]; 
			 PayType       =   (string)  reader["PayType"];       
             UnitPayRate   =   (double)     System.Convert.ToDouble(reader["UnitPayRate"]);           
             HourPayRate   =   (double)   System.Convert.ToDouble(reader["HourPayRate"]);   
             RebillUnitPayRate   =   (double)     System.Convert.ToDouble(reader["RebillUnitPayRate"]);           
             RebillHourPayRate   =   (double)   System.Convert.ToDouble(reader["RebillHourPayRate"]);   
            RebillSubTaskUnitPayRate   =   (double)     System.Convert.ToDouble(reader["RebillSubTaskUnitPayRate"]);           
            RebillSubTaskHourPayRate   =   (double)   System.Convert.ToDouble(reader["RebillSubTaskHourPayRate"]);   

            PayList = new ArrayList();
		}

        public override string ToString()
        {
            string s = "\t";
            s = s + Id.ToString() + ", ";
            s = s + TaskID.ToString() + ", ";
            s = s + OtherTaskID.ToString() + ", ";
            s = s + FacilityID.ToString() + ", ";
            s = s + EmployeeId.ToString() + ", ";
            s = s + WorkDate.ToString() + ", ";
            s = s + ShiftID.ToString() + ", ";
            s = s + " UPM: " + UPM.ToString() + ", ";
            s = s + " HOURS: " + HoursWorked.ToString() + ", ";
            s = s + OutOfTownType.ToString() + ", ";
            s = s + PayrollStatus.ToString() + ", ";
            s = s + PayType.ToString() + ",";
            s = s + " UnitRate: " + UnitPayRate.ToString() + ", ";
            s = s + " HourRate: " +  HourPayRate.ToString() + ", ";
            s = s + " Rebillable: " +  Rebillable.ToString() + ", ";
            s = s + " RebillUnitRate: " + RebillUnitPayRate.ToString() + ", ";
            s = s + " RebillHourRate: " +  RebillHourPayRate.ToString() + ", ";
            s = s + " RebillSubTaskUnitRate: " + RebillSubTaskUnitPayRate.ToString() + ", ";
            s = s + " RebillSubTaskHourRate: " +  RebillSubTaskHourPayRate.ToString() + ", ";
            foreach (PayItem p in PayList)
            {
                s = s + "\t\t" + p.ToString() + "\n";
            }
            return s;
        }

        public double BestPayRate()
        {
            double bp = 0.0;
            double ur = UnitPayRate * UPM ;
            double hr =      HourPayRate;
            if (Rebillable == 1) 
            { 
                
                double urRB = RebillUnitPayRate * UPM;
                double hrRB = RebillHourPayRate;
                double urR1 = RebillSubTaskUnitPayRate * UPM;
                double hrR1 = RebillSubTaskHourPayRate;
                if (urR1 > urRB)
                {
                    ur = urR1;
                    PayType = "EITHER";
                }
                else
                {
                    ur = urRB;
                }
                if (hrR1 > hrRB)
                {
                    hr = hrR1;
                    PayType = "EITHER";
                }
                else
                {
                    hr = hrRB;
                }
            }

            if (OtherTaskID == 0)
            {  
                if (OutOfTownType == "N")
                {
                    switch(PayType.Trim().ToUpper())
                    {
                        case "EITHER" :
                            if ((hr) > (ur))
                            {
                                bp =  hr;
                            }
                            else
                            {
                                bp =  ur;
                            }
                            break;
                        case "UNITS" :
                            bp =  ur;
                            break;
                        case "HOURS" :
                            bp =  hr;
                            break;
                        default :
                            bp = 0.0;
                            break;
                    }
                }
                else
                {
                     bp = 0.0;
                }
            }
            else // other Task
            {
                bp = 0.0;
                // should use other task fucntion
            }

                
            return(bp);

        }
        

	}
    public class DayWorked
    {
        public DateTime Day;
        public ArrayList TaskWorkedList;
        double theDAR = -1;

        public DayWorked(Emp p, SqlDataReader reader)
        {
            TaskWorkedList = new ArrayList();
            Day      =   System.DateTime.Parse(reader["WorkDate"].ToString());    
            while(!reader.IsClosed 
                && (Day      ==   System.DateTime.Parse(reader["WorkDate"].ToString()) )
                && (p.EmployeeId ==   (int)     reader["EmployeeId"])  
                ) 
            { 
                //Loop through your rows of data 
                TaskWorkedForCalc t = new TaskWorkedForCalc(this,reader);
                TaskWorkedList.Add(t);  
                if (!(reader.Read()))
                {
                    reader.Close();
                }
            }
        }
        public double TotalPay()
        {
            double hw = 0;

            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                if (t.OtherTaskID == 0 )
                {
                    foreach (PayItem p in t.PayList)
                    {
                            hw = hw + p.PayAmount;
                    }
                }
            }
            return hw;
        }

        public double HoursWorked()
        {
            double hw = 0;

            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                if (t.OtherTaskID == 0 )
                    hw = hw + t.HoursWorked;
            }
            return hw;
        }
        public double HoursWorkedAtHomeFacility()
        {
            double hw = 0;

            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                if ((t.OtherTaskID == 0 ) && (t.OutOfTownType.Trim().ToUpper() == "N"))
                    hw = hw + t.HoursWorked;
            }
            return hw;
        }
        public bool JustHourly()
        {
            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                if ((t.PayType.Trim().ToUpper() == "UNIT"))
                {
                    return false; 
                }
            }
            return true;
        }
        public double HighestHourly()
        {
            double hr = 0.0;
            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                double hrt = t.HourPayRate;
                if (hrt > hr)
                {
                    hr = hrt; 
                }
            }
            return hr;
        }

        public double RegHours()
        {
            double hw = 0;

            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                if (t.OtherTaskID == 0 )
                {
                    foreach (PayItem p in t.PayList)
                    {
                        if (p.PayMultiplier == 1.0)
                        {
                            hw = hw + p.HoursPayed;
                        }
                    }
                }
            }
            return hw;
        }
        public double DAR()
        {
            if (theDAR == -1)
            {
                double dp = 0;

                foreach (TaskWorkedForCalc t in TaskWorkedList)
                {
                    if (t.OutOfTownType.Trim().ToUpper() == "N")
                    {
                        dp = dp + (t.BestPayRate() * t.HoursWorked);
                    }
                }
                theDAR = (double) (dp/HoursWorkedAtHomeFacility());
            }

            return theDAR;

        }


        public override string ToString()
        {
            string s = "";
            s = s + "\t\t" + Day.ToString() + ", Hours worked: " + System.Convert.ToString(HoursWorked()) + ", DAR: " + System.Convert.ToString(DAR()) + ", \n";
            foreach (TaskWorkedForCalc t in TaskWorkedList)
            {
                s = s + "\t\t\t" + t.ToString() + "\n";
            }
            return s;
        }


    }
	public class Emp
	{
        public int EmployeeId;
        public DateTime HireDate;
        public ArrayList DayWorkedList;

		public Emp(SqlDataReader reader)
		{
            DayWorkedList = new ArrayList();
            EmployeeId    =   (int)     reader["EmployeeId"];   
            if (!reader.IsDBNull(reader.GetOrdinal("HireDate")))
            {
                HireDate      =   System.DateTime.Parse(reader["HireDate"].ToString());      
            }


            while(!reader.IsClosed && ((int)reader["EmployeeID"] == EmployeeId) ) 
            { 
                //Loop through your rows of data 
                DayWorked d = new DayWorked(this,reader);
                DayWorkedList.Add(d); 
//                notEOF = reader.Read();
            }
        }
        public double HoursWorked()
        {
            double hw = 0;

            foreach (DayWorked d in DayWorkedList)
            {
                hw = hw + d.HoursWorked();
            }
            return hw;
        }

        public bool JustHourly(string connectString, int facilityID, string startDate, string endDate)
        {
            int lucount = 0;

            string sql =  "";
            sql = sql + " SELECT   COUNT(*) AS LOULCount ";
            sql = sql + " FROM    EmployeeTaskWorked INNER JOIN ";
            sql = sql + "       Tasks ON EmployeeTaskWorked.TaskID = Tasks.Id ";
            sql = sql + " WHERE     (Tasks.TaskCode = 'LO' OR Tasks.TaskCode = 'UL') ";
            sql = sql + " AND (EmployeeTaskWorked.FacilityId = " + facilityID.ToString() + ") ";
            sql = sql + " AND (EmployeeTaskWorked.EmployeeId = " + EmployeeId.ToString() + ") ";
            sql = sql + " AND (EmployeeTaskWorked.WorkDate BETWEEN '" + startDate + "'  AND  '" + endDate + "') ";

            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd.CommandTimeout = 360;
            cmd.Connection.Open(); 
            SqlDataReader reader = cmd.ExecuteReader(); 


            if(reader.Read()) 
            {
                if (!reader.IsDBNull(0))
                {
                    lucount = (int) System.Convert.ToInt32(reader["LOULCount"]);
                }
            }

            reader.Close(); //close the reader. 
            if(cmd.Connection.State == ConnectionState.Open) 
            { 
                cmd.Connection.Close(); 
            } 

            if (lucount > 0)
            {
                return false;
            }

            return true;
        }
		//--- ADDED TASKID PARAMETER 3/12/2007 [Steve Hicks]-----
		public double HighestHourly(string connectString, string startDate, string taskID)
        {
            double hr = 0.0;

            string sql =  "";
            sql = sql + " SELECT Top 1  MAX(EmployeeRates.HoursPayRate) AS MaxRate ";
            sql = sql + " FROM    Tasks INNER JOIN ";
            sql = sql + "   EmployeeRates ON Tasks.Id = EmployeeRates.TaskID INNER JOIN ";
            sql = sql + "   Employee ON EmployeeRates.EmployeeID = Employee.Id ";
            sql = sql + " Where  (Employee.Id = " + EmployeeId.ToString()+ ") ";
            sql = sql + "  AND    (EmployeeRates.EffectiveDate < '" + startDate + "')";
            sql = sql + "  AND    (Tasks.PayType = 'HOURS') ";
			//--- ADDED TASKID PARAMETER 3/12/2007 [Steve Hicks]-----
			sql = sql + "  AND (EmployeeRates.TaskID = '13') ";
			//------------------------------------------
            sql = sql + " GROUP BY EmployeeRates.EffectiveDate ";
            sql = sql + " ORDER BY EmployeeRates.EffectiveDate DESC ";

            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd.CommandTimeout = 360;
            cmd.Connection.Open(); 
            SqlDataReader reader = cmd.ExecuteReader(); 


            if(reader.Read()) 
            {
                if (!reader.IsDBNull(0))
                {
                    hr = (double) System.Convert.ToDouble(reader["MaxRate"]);
                }
            }

            reader.Close(); //close the reader. 
            if(cmd.Connection.State == ConnectionState.Open) 
            { 
                cmd.Connection.Close(); 
            } 

            
            return hr;
        }

        public bool SeventhDay(DateTime wd)
        {
            bool isit = false;
            if (DayWorkedList.Count >6)
            {
                isit = true;
                for (int i=1; i < DayWorkedList.Count; i++) // yes i=1,  start from the second item 
                {
                    if (((DayWorked)DayWorkedList[i-1]).Day.AddDays(1) != ((DayWorked)DayWorkedList[i]).Day)
                    {
                        isit = false;
                        break;
                    }
                    if ( ((DayWorked)DayWorkedList[i]).Day > wd)
                    {
                        isit = false;
                        break;
                    }
                }
            }
            return isit;
        }
        public override string ToString()
        {
            string s = "";
            s = s + " ID: " + EmployeeId.ToString() + ", Hours Worked: " + System.Convert.ToString(HoursWorked())+ ", HireDate: " + HireDate.ToString() + ", \n";
            foreach (DayWorked d in DayWorkedList)
            {
                s = s + "\t" + d.ToString() + "\n";
            }
            return s;
        }

	}
	public class CalcClass
	{
		public int facilityID;
		public string AssFacilities;
		public double OutOfTownHourlyRate;
        public double OutOfTownMinHours;
        public string connectString;
		public string startDate, endDate;
        public string UserName;
		public ArrayList EmpList;



		public CalcClass(int fID, string sDate, string eDate, string cStr, string usr)
		{
			//
			// TODO: Add constructor logic here
			//

		    int AssFacilityID;
            EmpList = new ArrayList();

			facilityID = fID;
            //connectString = cStr;
            connectString = System.Web.HttpContext.Current.Session["dbPath"].ToString();
			startDate = sDate;
			endDate = eDate;
            UserName = usr;

			AssFacilities = " -1";

			string sql = " Select isNull(AssociatedFacilityID, -1) as AssFID from AssociatedFacility where FacilityID = " + facilityID.ToString();
			SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
			cmd.CommandTimeout = 360;
			cmd.Connection.Open(); 
			SqlDataReader reader = cmd.ExecuteReader(); 

			while(reader.Read()) 
			{
				AssFacilityID = (int)  System.Convert.ToInt32(reader["AssFID"]);
				AssFacilities = AssFacilities + ", " + AssFacilityID.ToString();
			}

			reader.Close(); //close the reader. 
			if(cmd.Connection.State == ConnectionState.Open) 
			{ 
				cmd.Connection.Close(); 
			} 


			LoadData();
			DoCalc();
			SavePay();
		}
		public int LoadData ()
		{
			// execute query for reg tasks
			string sql = "";


			sql = sql + " ( select EmployeeTaskWorked.Id,  Employee.HireDate, EmployeeTaskWorked.TaskID,  EmployeeTaskWorked.OtherTaskID, ";
			sql = sql + "       EmployeeTaskWorked.FacilityID, EmployeeTaskWorked.EmployeeId, EmployeeTaskWorked.WorkDate,  ";
			sql = sql + "       EmployeeTaskWorked.ShiftID, EmployeeTaskWorked.UPM, EmployeeTaskWorked.HoursWorked,  ";
			sql = sql + "       EmployeeTaskWorked.OutOfTownType, EmployeeTaskWorked.PayrollStatus, ";
			sql = sql + "       Tasks.PayType, Tasks.Rebillable, ";

			sql = sql + "  IsNull((select  top 1  UnitsPayRate from EmployeeRates                 ";
			sql = sql + "  where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
			sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
			sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
			sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID                 ";
			sql = sql + "  and  EmployeeRates.EffectiveDate =  (select top 1 Max(EmployeeRates.EffectiveDate) from employeerates ";
			sql = sql + "                                      where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
			sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
			sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
			sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID ";
			//sql = sql + "   Order by EffectiveDate DESC ";
			sql = sql + "                 )";
			sql = sql + " ";
			sql = sql + "   ),0) as UnitPayRate,     ";
			sql = sql + "  IsNull((select  top 1  HoursPayRate from EmployeeRates     ";
			sql = sql + "  where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate      ";
			sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID         ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID          ";
			sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID          ";
			sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID                ";
			sql = sql + "  and  EmployeeRates.EffectiveDate =  (select top 1 Max(EmployeeRates.EffectiveDate) from employeerates ";
			sql = sql + "                                      where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
			sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "   and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
			sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
			sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID     ";
			//sql = sql + "   Order by EffectiveDate DESC ";
			sql = sql + "             )";
			sql = sql + "  ),0) as HourPayRate,  ";

            sql = sql + "  IsNull((select  top 1  UnitsPayRate from EmployeeRates                 ";
            sql = sql + "  where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID                 ";
            sql = sql + "  and  RTRIM(Tasks.TaskCode) = 'RB' ";
            sql = sql + "  and  EmployeeRates.EffectiveDate =  (select top 1 Max(EmployeeRates.EffectiveDate) from EmployeeRates ";
            sql = sql + "                                where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID ";
            //sql = sql + "   Order by EffectiveDate DESC";
            sql = sql + "                 )";
            sql = sql + " ),0) as ReBillUnitPayRate,     ";
            sql = sql + " ";
            sql = sql + "  IsNull((select  top 1  HoursPayRate from EmployeeRates     ";
            sql = sql + "  where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate      ";
            sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID         ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID          ";
            sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID          ";
            sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID                ";
            sql = sql + "  and  RTRIM(Tasks.TaskCode) = 'RB' ";
            sql = sql + "  and  EmployeeRates.EffectiveDate =  (select top 1 Max(EmployeeRates.EffectiveDate) from EmployeeRates ";
            sql = sql + "                                      where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID     ";
           // sql = sql + "   Order by EffectiveDate DESC";
            sql = sql + "             )";
            sql = sql + " ),0) as RebillHourPayRate,  ";

            sql = sql + "  IsNull((select  top 1  UnitsPayRate from EmployeeRebillRates                 ";
            sql = sql + "  where EmployeeRebillRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRebillRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRebillRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRebillRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRebillRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRebillRates.TaskID = EmployeeTaskWorked.RebillSubTaskID                 ";
            sql = sql + "  and  EmployeeRebillRates.EffectiveDate =  (select top 1 Max(EmployeeRebillRates.EffectiveDate) from EmployeeRebillRates ";
            sql = sql + "                                      where EmployeeRebillRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRebillRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRebillRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRebillRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRebillRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRebillRates.TaskID = EmployeeTaskWorked.RebillSubTaskID ";
            //sql = sql + "   Order by EffectiveDate DESC ";
            sql = sql + "                 )";
            sql = sql + " ),0) as ReBillSubTaskUnitPayRate,     ";
            sql = sql + " ";
            sql = sql + "  IsNull((select  top 1  HoursPayRate from EmployeeRebillRates     ";
            sql = sql + "  where EmployeeRebillRates.EffectiveDate <= EmployeeTaskWorked.WorkDate      ";
            sql = sql + "  and  EmployeeRebillRates.FacilityID = EmployeeTaskWorked.FacilityID         ";
			//sql = sql + "  and  EmployeeRebillRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "  and  EmployeeRebillRates.EmployeeID = EmployeeTaskWorked.EmployeeID          ";
            sql = sql + "  and  EmployeeRebillRates.ShiftID = EmployeeTaskWorked.ShiftID          ";
            sql = sql + "  and  EmployeeRebillRates.TaskID = EmployeeTaskWorked.RebillSubTaskID                ";
            sql = sql + "  and  EmployeeRebillRates.EffectiveDate =  (select top 1 Max(EmployeeRebillRates.EffectiveDate) from EmployeeRebillRates ";
            sql = sql + "                                      where EmployeeRebillRates.EffectiveDate <= EmployeeTaskWorked.WorkDate              ";
            sql = sql + "  and  EmployeeRebillRates.FacilityID = EmployeeTaskWorked.FacilityID                ";
			//sql = sql + "  and  EmployeeRebillRates.FacilityID = " + facilityID.ToString() ;
			sql = sql + "   and  EmployeeRebillRates.EmployeeID = EmployeeTaskWorked.EmployeeID                  ";
            sql = sql + "  and  EmployeeRebillRates.ShiftID = EmployeeTaskWorked.ShiftID                 ";
            sql = sql + "  and  EmployeeRebillRates.TaskID = EmployeeTaskWorked.RebillSubTaskID     ";
           // sql = sql + "   Order by EffectiveDate DESC ";
            sql = sql + "             )";
            sql = sql + " ),0) as RebillSubTaskHourPayRate  ";

//			sql = sql + "             IsNull((select Top 1 UnitsPayRate from EmployeeRates ";
//			sql = sql + "                   where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate ";
//			sql = sql + "                    and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID ";
//			sql = sql + "                    and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID ";
//			sql = sql + "                    and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID ";
//			sql = sql + "                    and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID ";
//			sql = sql + "                   Order by EmployeeRates.EffectiveDate DESC ";
//			sql = sql + "              ),0) as UnitPayRate, ";
//			
//			sql = sql + "             IsNull((select Top 1 HoursPayRate from EmployeeRates  ";
//			sql = sql + "                   where EmployeeRates.EffectiveDate <= EmployeeTaskWorked.WorkDate    ";
//			sql = sql + "                    and  EmployeeRates.FacilityID = EmployeeTaskWorked.FacilityID ";
//			sql = sql + "                    and  EmployeeRates.EmployeeID = EmployeeTaskWorked.EmployeeID ";
//			sql = sql + "                    and  EmployeeRates.ShiftID = EmployeeTaskWorked.ShiftID ";
//			sql = sql + "                    and  EmployeeRates.TaskID = EmployeeTaskWorked.TaskID ";
//			sql = sql + "                   Order by EmployeeRates.EffectiveDate DESC ";
//			sql = sql + "              ),0) as HourPayRate ";

			sql = sql + " from EmployeeTaskWorked inner join Tasks on  EmployeeTaskWorked.TaskID = Tasks.ID ";
            sql = sql + "                         inner join Employee on  EmployeeTaskWorked.EmployeeID = Employee.ID ";

			sql = sql + " where TaskID <> 0  ";
			sql = sql + " and   Workdate Between  '" + startDate + "' AND '" + endDate + "' ";
            //sql = sql + " and  EmployeeTaskWorked.FacilityID = " + facilityID.ToString() + "  ";
            sql = sql + " and  EmployeeTaskWorked.EmployeeID IN (Select ID from Employee where facilityID =  " + facilityID.ToString() + " OR FacilityID IN  (" + AssFacilities +") )  ) ";

			sql = sql + " UNION  ";		
            
            
            sql = sql + " ( select EmployeeTaskWorked.Id, Employee.HireDate, EmployeeTaskWorked.TaskID,  EmployeeTaskWorked.OtherTaskID, ";
            sql = sql + "       EmployeeTaskWorked.FacilityID, EmployeeTaskWorked.EmployeeId, EmployeeTaskWorked.WorkDate,  ";
            sql = sql + "       EmployeeTaskWorked.ShiftID, EmployeeTaskWorked.UPM, EmployeeTaskWorked.HoursWorked,  ";
            sql = sql + "       EmployeeTaskWorked.OutOfTownType, EmployeeTaskWorked.PayrollStatus, ";
            sql = sql + "       OtherTasks.PayType, 0 as Rebillable, 0 as UnitPayRate, 0 as HourPayRate,  0 as RebillUnitPayRate, 0 as RebillHourPayRate,  0 as RebillSubTaskUnitPayRate, 0 as RebillSubTaskHourPayRate   ";

            sql = sql + " from EmployeeTaskWorked inner join OtherTasks on  EmployeeTaskWorked.OtherTaskID = OtherTasks.ID ";
            sql = sql + "                         inner join Employee on  EmployeeTaskWorked.EmployeeID = Employee.ID ";

            sql = sql + " where OtherTaskID <> 0  ";
            sql = sql + " and   Workdate Between  '" + startDate + "' AND '" + endDate + "' ";
            //sql = sql + " and  EmployeeTaskWorked.FacilityID = " + facilityID.ToString() + "  ";
			sql = sql + " and  EmployeeTaskWorked.EmployeeID IN (Select ID from Employee where facilityID =  " + facilityID.ToString()  + " OR FacilityID IN  (" + AssFacilities +") )  ) ";
            
            sql = sql + " Order by EmployeeTaskWorked.EmployeeID, EmployeeTaskWorked.Workdate, EmployeeTaskWorked.HoursWorked  ";

            
            //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            //Trace.WriteLine(sql);


			SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString));
            cmd.CommandTimeout = 360;
			cmd.Connection.Open(); 
			SqlDataReader reader = cmd.ExecuteReader(); 


            if (!(reader.Read()))
            {
                reader.Close();
            }

			while(!reader.IsClosed) 
			{ 
				//Loop through your rows of data 
					EmpList.Add(new Emp(reader));
			} 
			reader.Close(); //close the reader. 
			if(cmd.Connection.State == ConnectionState.Open) 
			{ 
				cmd.Connection.Close(); 
			} 

            return 0;


        }
		public int DoCalc()
		{
               // Get OT type
            string sql =  "";
            sql = sql + " SELECT OvertimeBasis.Description ";
            sql = sql + " FROM   OvertimeBasis INNER JOIN ";
            sql = sql + "  Facility ON OvertimeBasis.OvertimeCalcBasis = Facility.OvertimeCalcBasis ";
            sql = sql + " WHERE  Facility.ID = " + facilityID.ToString() ;
            
            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd.CommandTimeout = 360;
            cmd.Connection.Open(); 
            SqlDataReader reader = cmd.ExecuteReader(); 

            string calcType = "STANDARD";
            while(reader.Read()) 
            {
                calcType = (string) reader["Description"];
                calcType = calcType.Trim().ToUpper();
            }

            reader.Close(); //close the reader. 
            if(cmd.Connection.State == ConnectionState.Open) 
            { 
                cmd.Connection.Close(); 
            } 

            // Get OutofTown type
            sql =  "";
            sql = sql + " Select IRGCompany.OutOfTownRate, IRGCompany.OutOfTownHoursPerDay FROM Facility ";
            sql = sql + "     INNER JOIN  IRGCompany ON Facility.IRGCompanyId = IRGCompany.Id  ";
            sql = sql + " WHERE  Facility.ID = " + facilityID.ToString() ;
            
            SqlCommand cmd2 = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd2.CommandTimeout = 360;
            cmd2.Connection.Open(); 
            SqlDataReader reader2 = cmd2.ExecuteReader(); 

            OutOfTownHourlyRate = 0.0;
            OutOfTownMinHours = 0.0;
            while(reader2.Read()) 
            {
                OutOfTownHourlyRate = (double)   System.Convert.ToDouble(reader2["OutOfTownRate"]);
                OutOfTownMinHours = (double)   System.Convert.ToDouble(reader2["OutOfTownHoursPerDay"]);
            }

            reader2.Close(); //close the reader. 
            if(cmd2.Connection.State == ConnectionState.Open) 
            { 
                cmd2.Connection.Close(); 
            } 
            
            
            //if calcType is STANDARD doStandardCalc()
            if (calcType == "STANDARD") 
            {
                DoStandardCalc();
            }

			//if calcType is CALIFORNIA doCaliforniaCalc()
            if (calcType == "CALIFORNIA") 
            {
                DoCaliforniaCalc();
            }
            
            return 0;
		}
        public int DoStandardCalc()
        {
            //

            foreach (Emp e in EmpList)
            {
                double PayPeriodHours = 0.0;
                foreach (DayWorked d in e.DayWorkedList)
                {
                    double DayHours = 0.0;
                    foreach (TaskWorkedForCalc t in d.TaskWorkedList)
                    {
                        if (t.TaskID != 0 )
                        {
                            double othours = 0.0;
                            double reghours = 0.0;
                            if (t.HoursWorked + DayHours + PayPeriodHours > 40)
                            {
                                if (DayHours + PayPeriodHours > 40)
                                {
                                    reghours = 0.0;
                                    othours  = t.HoursWorked;
                                }
                                else
                                {
                                    reghours =  40 - (DayHours + PayPeriodHours);
                                    othours  =  t.HoursWorked -  reghours;
                                }
                            }
                            else
                            {
                                reghours = t.HoursWorked;
                                othours  = 0.0;
                            }
                            if (t.OutOfTownType == "N")
                            {

                                if (reghours > 0.0)
                                {
                                    t.PayList.Add(new PayItem(t, 1.0, reghours, reghours * t.BestPayRate(), t.BestPayRate()));
                                }
                                if (othours > 0.0)
                                {
                                    t.PayList.Add(new PayItem(t, 1.5, othours, (1.5 * othours * d.DAR()), d.DAR()));
                                }
                            }
                            else // Out of  Town
                            {
                                double otr = OutOfTownHourlyRate;
                                if (e.JustHourly(connectString, facilityID, startDate, endDate))
                                {
                                    double HHR = e.HighestHourly(connectString, startDate, t.TaskID.ToString());
                                    if ( otr < HHR)
                                    {
                                        otr = HHR;
                                    }
                                }
//                                if (t.OutOfTownType == "D")
//                                {
                                //    t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, (t.HoursWorked * OutOfTownHourlyRate), OutOfTownHourlyRate));
                                    if (reghours > 0.0)
                                    {
                                        t.PayList.Add(new PayItem(t, 1.0, reghours, reghours * otr, otr));
                                    }
                                    if (othours > 0.0)
                                    {
                                        t.PayList.Add(new PayItem(t, 1.5, othours, (1.5 * othours * otr), otr));
                                    }
//                                }
//                                else  // Out of town type == O
//                                {
//                                    double DayHoursWorked = t.Parent.HoursWorked();
//                                    if (DayHoursWorked > OutOfTownMinHours)
//                                    {
//                                        //t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, (t.HoursWorked * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                        if (reghours > 0.0)
//                                        {
//                                            t.PayList.Add(new PayItem(t, 1.0, reghours, reghours *  otr,  otr));
//                                        }
//                                        if (othours > 0.0)
//                                        {
//                                            t.PayList.Add(new PayItem(t, 1.5, othours, (1.5 * othours *  otr),  otr));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        //  t.PayList.Add(new PayItem(t, 1.0, (OutOfTownMinHours / t.Parent.TaskWorkedList.Count), ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate)));
//                                        // to show actual hours worked but pay
//                                        //t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                        if (reghours > 0.0)
//                                        {
//                                            double one = ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate);
//                                            double two = ((reghours) * otr);
//                                            if (one > two)
//                                            {
//                                                t.PayList.Add(new PayItem(t, 1.0, reghours, ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                            }
//                                            else
//                                            {
//                                                t.PayList.Add(new PayItem(t, 1.0, reghours, ((reghours) * otr), otr));
//                                            }
//                                        }
//                                        if (othours > 0.0)
//                                        {
//                                            double one = ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate);
//                                            double two = ((othours) * otr);
//                                            if (one > two)
//                                            {
//                                                t.PayList.Add(new PayItem(t, 1.5, othours, (1.5 * (OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                            }
//                                            else
//                                            {
//                                                t.PayList.Add(new PayItem(t, 1.5, othours, (1.5 * (othours) * otr), otr));
//                                            }
//                                        }
//                                    }
//
//                                }
                            }
                            DayHours = DayHours + t.HoursWorked;

                        }
                        else // Other Task
                        {
							double FlatFee = 0.0;
							string sql =  "";
							sql = sql + " Select CalcMethod, isNull(FlatFeeAmount, 0.0) as FlatFeeAmount from OtherTasks where ID = " + t.OtherTaskID.ToString();     
            
							SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
							cmd.CommandTimeout = 360;
							cmd.Connection.Open(); 
							SqlDataReader reader = cmd.ExecuteReader(); 

							string OtherTaskCalcMethod = "";
							if(reader.Read()) 
							{
								OtherTaskCalcMethod = (string) reader["CalcMethod"];
								FlatFee = (double) System.Convert.ToDouble(reader["FlatFeeAmount"]);
							}

							reader.Close(); //close the reader. 
							if(cmd.Connection.State == ConnectionState.Open) 
							{ 
								cmd.Connection.Close(); 
							} 


							if ( OtherTaskCalcMethod.Trim().ToUpper() == "HOLIDAY")
							{
                                sql =  "";
                                sql = sql + " select isNull(max(hourspayrate), 0) as MaxHoursPayRate from employeerates o where o.effectivedate = ";
                                sql = sql + " ( ";
                                sql = sql + " SELECT      MAX(e.EffectiveDate) AS MaxEfDate ";
                                sql = sql + " FROM         dbo.EmployeeRates e ";
                                sql = sql + " WHERE  e.FacilityID = " + facilityID.ToString()  ;
                                sql = sql + "   AND  e.ShiftID = " + t.ShiftID.ToString();                  
                                sql = sql + "   AND  e.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  e.EffectiveDate <= '" + t.WorkDate.ToString() + "' ";
                                sql = sql + "     and e.taskid = o.taskid ";
                                sql = sql + " GROUP BY e.TaskID ";
                                sql = sql + " ) ";
                                sql = sql + "   AND  o.FacilityID = " + facilityID.ToString()  ;
                                sql = sql + "   AND  o.ShiftID = " + t.ShiftID.ToString();                  
                                sql = sql + "   AND  o.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  o.EffectiveDate <= '" + t.WorkDate.ToString() + "' ";

                                //                                sql =  "";
                                //                                sql = sql + " Select IsNull(Max(HoursPayRate), 0) as MaxHoursPayRate from EmployeeRates " ;
                                //                                sql = sql + " WHERE  FacilityID = " + facilityID.ToString()  ;
                                //                                sql = sql + "   AND  ShiftID = " + t.ShiftID.ToString();                  
                                //                                sql = sql + "   AND  EmployeeID = " + t.EmployeeId.ToString();
                                //                                sql = sql + "   AND  EffectiveDate <= '" + t.WorkDate.ToString() + "' ";

                                cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
                                cmd.CommandTimeout = 360;
                                cmd.Connection.Open(); 
                                reader = cmd.ExecuteReader(); 
                                double hr = 0.0;
                                if (reader.Read())
                                {
                                    hr = (double)  System.Convert.ToDouble(reader["MaxHoursPayRate"]);
                                }

                 
                                reader.Close(); //close the reader. 
                                if(cmd.Connection.State == ConnectionState.Open) 
                                { 
                                    cmd.Connection.Close(); 
                                } 
                 
                 


                                sql =  "";
                                sql = sql + "  SELECT    dbo.EmployeeRates.UnitsPayRate FROM dbo.Tasks ";
                                sql = sql + "     INNER JOIN dbo.Facility ON dbo.Tasks.Id = dbo.Facility.DefaultTaskID ";
                                sql = sql + "     INNER JOIN dbo.EmployeeRates ON dbo.Tasks.Id = dbo.EmployeeRates.TaskID ";
                                sql = sql + " WHERE  dbo.Facility.Id = " + facilityID.ToString();
                                sql = sql + "   AND  dbo.EmployeeRates.ShiftID = " + t.ShiftID.ToString();    
                                sql = sql + "   AND  dbo.EmployeeRates.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  EffectiveDate <= '" + t.WorkDate.ToString() + "' ";
                                sql = sql + "   Order By EffectiveDate Desc ";

                                cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
                                cmd.CommandTimeout = 360;
                                cmd.Connection.Open();
                                reader = cmd.ExecuteReader(); 
                                double ur = 0.0;
                                if (reader.Read())
                                {
                                    ur = (double)  System.Convert.ToDouble(reader["UnitsPayRate"]);
                                }
                 
                                reader.Close(); //close the reader. 
                                if(cmd.Connection.State == ConnectionState.Open) 
                                { 
                                    cmd.Connection.Close(); 
                                } 

                                double HourPay = 8.0 * hr;
                                double UnitPay = 50.0 * ur;
                                double Pay = 0.0;
                                if ( HourPay > UnitPay )
                                {
                                    Pay = HourPay ;
                                }
                                else
                                {
                                    Pay = UnitPay ;
                                }

                                //         insert pay record with mult 1 
                                double PayDivHoursworked = 0.0;
                                if (t.HoursWorked > 0)
                                {
                                    PayDivHoursworked = Pay/t.HoursWorked;
                                }

                                t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, Pay, PayDivHoursworked));

                      
                            }   // Holiday


                            if ( OtherTaskCalcMethod.Trim().ToUpper() == "VACATION")
                            {
                                //                 '       if vacation then
                                //                 '         insert hours with 0 pay
                                t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, 0.0, 0.0));
                            }

							if ( OtherTaskCalcMethod.Trim().ToUpper() == "FLATFEE")
							{
								//                 '       if flat fee then
								//                 '         insert fee with 0 hours
								t.PayList.Add(new PayItem(t, 1.0, 0.0, FlatFee, 0.0));
							}
                             
                        }

                    }
                    PayPeriodHours = PayPeriodHours + d.HoursWorked();
                }
                //check for min daily pay for Out Of Town
                double MinOutOfTownPay = OutOfTownHourlyRate *  OutOfTownMinHours;
                foreach (DayWorked d in e.DayWorkedList)
                {
                    if (d.TotalPay() < (MinOutOfTownPay))
                    {
                        foreach (TaskWorkedForCalc t in d.TaskWorkedList)
                        {
                            if (t.OutOfTownType == "O")
                            {
                                bool Regfound = false;
                                bool OTfound = false;
                                foreach (PayItem p in t.PayList)
                                {
                                    if (p.PayMultiplier == 1.0)
                                    {
                                        Regfound = true;
                                    }
                                    if (p.PayMultiplier > 1.0)
                                    {
                                        OTfound = true;
                                    }
                                    
                                }

                                if (Regfound)
                                {
                                    foreach (PayItem p in t.PayList)
                                    {
                                        if (p.PayMultiplier == 1.0)
                                        {
                                            p.PayAmount = MinOutOfTownPay/d.TaskWorkedList.Count;
                                            p.PayRate =    OutOfTownHourlyRate;
                                        }
                                        if (p.PayMultiplier > 1.0)
                                        {
                                            p.PayAmount = 0;
                                            p.PayRate = OutOfTownHourlyRate;
                                        }
                                    
                                    }
                                }
                                if ((!Regfound) && (OTfound))
                                {
                                    foreach (PayItem p in t.PayList)
                                    {
                                        if (p.PayMultiplier > 1.0)
                                        {
                                            p.PayAmount = MinOutOfTownPay/d.TaskWorkedList.Count;
                                            p.PayRate = OutOfTownHourlyRate;
                                        }
                                    
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return 0;
        }
        
		// <Tayab:Bugfix Date="9 Sep 2008">
		// this checks if there was a holiday, sick or vacation included in the week or not
		// and also check if they took some type of day off then they did not 
		// do any work task the very same day
		private bool VacationOrHolidayPresent(ArrayList poDayWorkedList)
		{
			bool bDayOff = false;

			foreach(DayWorked oDay in poDayWorkedList)
			{
				foreach(TaskWorkedForCalc oTask in oDay.TaskWorkedList)
				{
					if (oTask.TaskID == 0 && NoOtherTasksWorked(oDay.TaskWorkedList))
					{
						// from OtherTask table --> 1 is holiday, 2 is vacation, 3 Funeral, 4 sick, Personal etc
						if (oTask.OtherTaskID != 0)
						{
							bDayOff = true;
							return bDayOff;	// no need to loop further
						}
					}
				}
			}

			return bDayOff;
		}


		private bool NoOtherTasksWorked(ArrayList poTaskWorkedList)
		{
			bool bNoOtherTasksWorked = true;
			
			foreach(TaskWorkedForCalc oTask in poTaskWorkedList)
			{
				if (oTask.TaskID != 0)
				{
					bNoOtherTasksWorked = false;
					break;
				}
			}

			return bNoOtherTasksWorked;
		}
		// </Tayab:Bugfix Date="9 Sep 2008">

        public int DoCaliforniaCalc()
        {
            foreach (Emp e in EmpList)
            {
                double PayPeriodHours = 0.0;
                double PayPeriodRegHours = 0.0;
                foreach (DayWorked d in e.DayWorkedList)
                {
                    double DayHours = 0.0;
                    
                    foreach (TaskWorkedForCalc t in d.TaskWorkedList)
                    {
                        if (t.TaskID != 0 )
                        {
                            double othours15x = 0.0;
                            double othours2x = 0.0;
                            double reghours = 0.0;
                            if ((t.HoursWorked + DayHours + PayPeriodRegHours > 40.0)
                                || (t.HoursWorked + DayHours > 8.0)
                                || (e.SeventhDay(t.WorkDate))   )
                            {
								// <Tayab:Bugfix Date="9 Sep 2008">
								// Added condition !VacationOrHolidayPresent()
								// If a person works 7 days in a row, then seven day rule applies only
								// if there were NO long weekends, vacations, sick etc present in between
								// and they did not work on any other task that day
                                if  (e.SeventhDay(t.WorkDate) && !VacationOrHolidayPresent(e.DayWorkedList))
								// </Tayab:Bugfix Date="9 Sep 2008">
                                {
                                    // over 8 today 2x
                                    // whatevers left 1.5x
                                    if (t.HoursWorked + DayHours > 8.0)
                                    {
                                        othours2x = t.HoursWorked - (8.0 - DayHours);
                                        othours15x  = (t.HoursWorked) - othours2x;
                                        //                                            othours2x = (t.HoursWorked + DayHours) -  8 ;
                                        //                                            othours15x  = (t.HoursWorked + DayHours) - othours2x;
                                    }
                                    else
                                    {
                                        othours15x = t.HoursWorked;
                                    }
                                }
                                else
                                {
                                    // over 12 today 2x
                                    if (t.HoursWorked + DayHours > 12.0)
                                    {
                                        othours2x =  (t.HoursWorked + DayHours) - 12.0;
                                        if (othours2x > t.HoursWorked)
                                        {
                                            othours2x = t.HoursWorked;
                                        }
                                    }

                                    // over 8 today 1.5x
                                    if (t.HoursWorked + DayHours > 8.0)
                                    {
                                        othours15x =  ((t.HoursWorked + DayHours) - 8.0) - othours2x;
                                        if (othours15x > t.HoursWorked - othours2x)
                                        {
                                            othours15x = t.HoursWorked - othours2x;
                                        }

                                    }

                                    // over 40 this week 1.5x
//                                    if ((t.HoursWorked + DayHours + PayPeriodRegHours > 40) &&  (othours2x==0))
//                                    {
//                                        if ( ((t.HoursWorked + DayHours + PayPeriodRegHours)-40) > othours15x)
//                                        {
//                                            othours15x =  (t.HoursWorked + DayHours + PayPeriodRegHours) - 40;
//                                            if (othours15x > t.HoursWorked - othours2x)
//                                            {
//                                                othours15x = t.HoursWorked - othours2x;
//                                            }
//
//                                        }
//                                    }

                                    // whatevers left   1x
                                    reghours = t.HoursWorked - (othours2x +  othours15x);
                                    
                                    // if still over 40 a week shift extra hours to 1.5 x
                                    if ((reghours > 0.0) && ((reghours + d.RegHours() + PayPeriodRegHours) > 40.0) )
                                    {
                                        double regpart = 0.0;
                                        double otpart = 0.0;
                                        
                                        otpart   =    ( reghours + (PayPeriodRegHours + d.RegHours())) - 40.0;
                                        if (otpart < 0.0)
                                        {
                                            otpart = 0.0;
                                        }

                                        regpart = reghours - otpart;
                                        if (regpart < 0.0)
                                        {
                                            regpart = 0.0;
                                        }

                                        reghours = regpart;
                                        othours15x = othours15x + otpart;
                                    }
                                }
                                                                  
                            }
                            else        // no overtime
                            {
                                reghours = t.HoursWorked;
                                othours2x  = 0.0;
                                othours15x  = 0.0;
                            }
                            if (t.OutOfTownType == "N")
                            {
                                if (reghours > 0.0)
                                {
                                    t.PayList.Add(new PayItem(t, 1.0, reghours, reghours * t.BestPayRate(), t.BestPayRate()));
                                }
                                if (othours15x > 0.0)
                                {
                                    t.PayList.Add(new PayItem(t, 1.5, othours15x, (1.5 * othours15x * d.DAR()),  d.DAR()));
                                }
                                if (othours2x > 0.0)
                                {
                                    t.PayList.Add(new PayItem(t, 2.0, othours2x, (2.0 * othours2x * d.DAR()),  d.DAR()));
                                }
                            }
                            else // Out of  Town
                            {
                                double otr = OutOfTownHourlyRate;
                                if (e.JustHourly(connectString, facilityID, startDate, endDate))
                                {
                                    double HHR = e.HighestHourly(connectString, startDate, t.TaskID.ToString());
                                    if ( otr < HHR)
                                    {
                                        otr = HHR;
                                    }
                                }
 //                               if (t.OutOfTownType == "D")
 //                               {
                                    //t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, (t.HoursWorked * OutOfTownHourlyRate), OutOfTownHourlyRate));
                                    if (reghours > 0.0)
                                    {
                                        t.PayList.Add(new PayItem(t, 1.0, reghours, reghours * otr, otr));
                                    }
                                    if (othours15x > 0.0)
                                    {
                                        t.PayList.Add(new PayItem(t, 1.5, othours15x, (1.5 * othours15x * otr), otr));
                                    }
                                    if (othours2x > 0.0)
                                    {
                                        t.PayList.Add(new PayItem(t, 2.0, othours2x, (2.0 * othours2x * otr),  otr));
                                    }
//                                }
//                                else  // Out of town type == O
//                                {
//                                    double DayHoursWorked = t.Parent.HoursWorked();
//                                    if (DayHoursWorked > OutOfTownMinHours)
//                                    {
//                                        // t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, (t.HoursWorked * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                        if (reghours > 0.0)
//                                        {
//                                            t.PayList.Add(new PayItem(t, 1.0, reghours, reghours * otr, otr));
//                                        }
//                                        if (othours15x > 0.0)
//                                        {
//                                            t.PayList.Add(new PayItem(t, 1.5, othours15x, (1.5 * othours15x * otr), otr));
//                                        }
//                                        if (othours2x > 0.0)
//                                        {
//                                            t.PayList.Add(new PayItem(t, 2.0, othours2x, (2.0 * othours2x * otr),  otr));
//                                        }
//                                    }
//                                    else
//                                    {
//                                        //t.PayList.Add(new PayItem(t, 1.0, (OutOfTownMinHours / t.Parent.TaskWorkedList.Count), ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate)));
//                                        // to show actual hours worked but pay
//                                        //t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                        
//                                         if ((reghours + othours15x + othours2x) > 0.0)
//
//                                         {
//                                            double one = ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate);
//                                           //  double one = (OutOfTownMinHours  * OutOfTownHourlyRate);
//                                             double two = ((reghours + othours15x + othours2x) * otr);
//                                             if (one > two)
//                                             {
//                                                 t.PayList.Add(new PayItem(t, 1.0, (reghours + othours15x + othours2x), ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * OutOfTownHourlyRate), OutOfTownHourlyRate));
//                                             }
//                                             else
//                                             {
//                                                 t.PayList.Add(new PayItem(t, 1.0, (reghours + othours15x + othours2x), ((reghours + othours15x + othours2x) * otr), otr));
//                                             }
//                                         }
//                                        
////                                        if (reghours > 0.0)
////                                        {
////                                            t.PayList.Add(new PayItem(t, 1.0, reghours, ((OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * otr), otr));
////                                        }
////                                        if (othours15x > 0.0)
////                                        {
////                                            t.PayList.Add(new PayItem(t, 1.5, othours15x, (1.5 * (OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * otr), otr));
////                                        }
////                                        if (othours2x > 0.0)
////                                        {
////                                            t.PayList.Add(new PayItem(t, 2.0, othours2x, (2.0 * (OutOfTownMinHours / t.Parent.TaskWorkedList.Count) * otr),  otr));
////                                        }
//                                    }
//                                }
                            }
                            DayHours = DayHours + t.HoursWorked;

                        }
                        else // Other Task
                        {
							double FlatFee = 0.0;
							string sql =  "";
							sql = sql + " Select CalcMethod, isNull(FlatFeeAmount, 0.0) as FlatFeeAmount from OtherTasks where ID = " + t.OtherTaskID.ToString();     
            
							SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
							cmd.CommandTimeout = 360;
							cmd.Connection.Open(); 
							SqlDataReader reader = cmd.ExecuteReader(); 

							string OtherTaskCalcMethod = "";
							if(reader.Read()) 
							{
								OtherTaskCalcMethod = (string) reader["CalcMethod"];
								FlatFee = (double) System.Convert.ToDouble(reader["FlatFeeAmount"]);
							}

							reader.Close(); //close the reader. 
							if(cmd.Connection.State == ConnectionState.Open) 
							{ 
								cmd.Connection.Close(); 
							} 


							if ( OtherTaskCalcMethod.Trim().ToUpper() == "HOLIDAY")
							{
                                sql =  "";
                                sql = sql + " select isNull(max(hourspayrate), 0) as MaxHoursPayRate from employeerates o where o.effectivedate = ";
                                sql = sql + " ( ";
                                sql = sql + " SELECT      MAX(e.EffectiveDate) AS MaxEfDate ";
                                sql = sql + " FROM         dbo.EmployeeRates e ";
                                sql = sql + " WHERE  e.FacilityID = " + facilityID.ToString()  ;
                                sql = sql + "   AND  e.ShiftID = " + t.ShiftID.ToString();                  
                                sql = sql + "   AND  e.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  e.EffectiveDate <= '" + t.WorkDate.ToString() + "' ";
                                sql = sql + "     and e.taskid = o.taskid ";
                                sql = sql + " GROUP BY e.TaskID ";
                                sql = sql + " ) ";
                                sql = sql + "   AND  o.FacilityID = " + facilityID.ToString()  ;
                                sql = sql + "   AND  o.ShiftID = " + t.ShiftID.ToString();                  
                                sql = sql + "   AND  o.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  o.EffectiveDate <= '" + t.WorkDate.ToString() + "' ";


                                //                                sql = sql + " Select IsNull(Max(HoursPayRate), 0) as MaxHoursPayRate from EmployeeRates " ;
                                //                                sql = sql + " WHERE  FacilityID = " + facilityID.ToString()  ;
                                //                                sql = sql + "   AND  ShiftID = " + t.ShiftID.ToString();                  
                                //                                sql = sql + "   AND  EmployeeID = " + t.EmployeeId.ToString();
                                //                                sql = sql + "   AND  EffectiveDate <= '" + t.WorkDate.ToString() + "' ";

                                cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
                                cmd.CommandTimeout = 360;
                                cmd.Connection.Open(); 
                                reader = cmd.ExecuteReader(); 
                                double hr = 0.0;
                                if (reader.Read())
                                {
                                    hr = (double)  System.Convert.ToDouble(reader["MaxHoursPayRate"]);
                                }

                 
                                reader.Close(); //close the reader. 
                                if(cmd.Connection.State == ConnectionState.Open) 
                                { 
                                    cmd.Connection.Close(); 
                                } 
                 
                 


                                sql =  "";
                                sql = sql + "  SELECT     dbo.EmployeeRates.UnitsPayRate FROM dbo.Tasks ";
                                sql = sql + "     INNER JOIN dbo.Facility ON dbo.Tasks.Id = dbo.Facility.DefaultTaskID ";
                                sql = sql + "     INNER JOIN dbo.EmployeeRates ON dbo.Tasks.Id = dbo.EmployeeRates.TaskID ";
                                sql = sql + " WHERE  dbo.Facility.Id = " + facilityID.ToString();
                                sql = sql + "   AND  dbo.EmployeeRates.ShiftID = " + t.ShiftID.ToString();    
                                sql = sql + "   AND  dbo.EmployeeRates.EmployeeID = " + t.EmployeeId.ToString();
                                sql = sql + "   AND  EffectiveDate <= '" + t.WorkDate.ToString() + "' ";
                                sql = sql + "   Order By  EffectiveDate Desc ";


                                cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
                                cmd.CommandTimeout = 360;
                                cmd.Connection.Open();
                                reader = cmd.ExecuteReader(); 
                                double ur = 0.0;
                                if (reader.Read())
                                {
                                    ur = (double)  System.Convert.ToDouble(reader["UnitsPayRate"]);
                                }
                 
                                reader.Close(); //close the reader. 
                                if(cmd.Connection.State == ConnectionState.Open) 
                                { 
                                    cmd.Connection.Close(); 
                                } 

                                double HourPay = 8.0 * hr;
                                double UnitPay = 50.0 * ur;
                                double Pay = 0.0;
                                if ( HourPay > UnitPay )
                                {
                                    Pay = HourPay ;
                                }
                                else
                                {
                                    Pay = UnitPay ;
                                }

                                //         insert pay record with mult 1 
                                double PayDivHoursworked = 0.0;
                                if (t.HoursWorked > 0)
                                {
                                    PayDivHoursworked = Pay/t.HoursWorked;
                                }

                                t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, Pay, PayDivHoursworked));

                      
                            }   // Holiday


                            if ( OtherTaskCalcMethod.Trim().ToUpper() == "VACATION")
                            {
                                //                 '       if vacation then
                                //                 '         insert hours with 0 pay
                                t.PayList.Add(new PayItem(t, 1.0, t.HoursWorked, 0.0, 0.0));
                            }

							if ( OtherTaskCalcMethod.Trim().ToUpper() == "FLATFEE")
							{
								//                 '       if flat fee then
								//                 '         insert fee with 0 hours
								t.PayList.Add(new PayItem(t, 1.0, 0.0, FlatFee, 0.0));
							}

                             
                        }

                    }
                    PayPeriodHours = PayPeriodHours + d.HoursWorked();
                    PayPeriodRegHours =   PayPeriodRegHours + d.RegHours();
                }
                //check for min daily pay for Out Of Town
                double MinOutOfTownPay = OutOfTownHourlyRate *  OutOfTownMinHours;
                foreach (DayWorked d in e.DayWorkedList)
                {
                    if (d.TotalPay() < (MinOutOfTownPay))
                    {
                        foreach (TaskWorkedForCalc t in d.TaskWorkedList)
                        {
                            if (t.OutOfTownType == "O")
                            {
                                bool Regfound = false;
                                bool OTfound = false;
                                foreach (PayItem p in t.PayList)
                                {
                                    if (p.PayMultiplier == 1.0)
                                    {
                                        Regfound = true;
                                    }
                                    if (p.PayMultiplier > 1.0)
                                    {
                                        OTfound = true;
                                    }
                                    
                                }

                                if (Regfound)
                                {
                                    foreach (PayItem p in t.PayList)
                                    {
                                        if (p.PayMultiplier == 1.0)
                                        {
                                            p.PayAmount = MinOutOfTownPay/d.TaskWorkedList.Count;
                                            p.PayRate =    OutOfTownHourlyRate;
                                        }
                                        if (p.PayMultiplier > 1.0)
                                        {
                                            p.PayAmount = 0;
                                            p.PayRate = OutOfTownHourlyRate;
                                        }
                                    
                                    }
                                }
                                if ((!Regfound) && (OTfound))
                                {
                                    foreach (PayItem p in t.PayList)
                                    {
                                        if (p.PayMultiplier > 1.0)
                                        {
                                            p.PayAmount = MinOutOfTownPay/d.TaskWorkedList.Count;
                                            p.PayRate = OutOfTownHourlyRate;
                                        }
                                    
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return 0;
        }


        public int SavePay()
        {
            // remove task worked pay records
            string sql =  "";
            string sql2 =  "";

//            sql = sql + " Delete FROM EmployeeTaskWorkedPay  ";
//            sql = sql + " WHERE  ( EmployeeTaskWorkedID in ";
//            sql = sql + "    (Select id from EmployeeTaskWorked where ";
//            sql = sql + "          FacilityID =  "  + facilityID.ToString();
//            sql = sql + "       AND   ";
//            sql = sql + "        WorkDate Between '" + startDate + "' AND '" + endDate + "' ) )";
//
//            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
//            cmd.CommandTimeout = 360;
//            cmd.Connection.Open(); 
//            cmd.ExecuteNonQuery();



            sql =  "";
            sql = sql + " Delete FROM EmployeeTaskWorkedPay  ";
            sql = sql + " WHERE  ( EmployeeTaskWorkedID in ";
            sql = sql + "    (Select id from EmployeeTaskWorked where ";
            sql = sql + "        EmployeeID IN (Select ID from Employee where facilityID =  " + facilityID.ToString()  + " OR FacilityID IN  (" + AssFacilities +")   ) ";
            sql = sql + "       AND   ";
            sql = sql + "        WorkDate Between '" + startDate + "' AND '" + endDate + "' ) )";

            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd.CommandTimeout = 360;
            cmd.Connection.Open(); 
            cmd.ExecuteNonQuery();



            sql =  "";
            sql = sql + " Delete FROM EmployeeTaskWorkedPay  ";
            sql = sql + " WHERE EmployeeTaskWorkedID Not in ";
            sql = sql + "    (Select id from EmployeeTaskWorked )" ;
            
            cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
            cmd.CommandTimeout = 360;
            cmd.Connection.Open(); 
            cmd.ExecuteNonQuery();


            // insert task worked pay records


            sql =  "";
            sql2 =  "";

            foreach (Emp e in  EmpList)
            {
                foreach  (DayWorked d in e.DayWorkedList)
                {
                    foreach (TaskWorkedForCalc t in d.TaskWorkedList)
                    {
                        if (sql2 != "")
                            sql2 = sql2 + " \n   OR ";
                        sql2 = sql2 + " \n   id = " + System.Convert.ToString(t.Id);
                        foreach (PayItem p in t.PayList)
                        {
                            sql = sql +  " Insert into EmployeeTaskWorkedPay (EmployeeTaskWorkedID, PayMultiplier, PayRate, HoursPaid, PayAmount, PayrollStatus,  LastModifiedBy, LastModifiedOn) "  ;   
                            sql = sql + " Values ";
                            sql = sql + " (" + t.Id.ToString() + ", " + p.PayMultiplier.ToString() + ", ";
                            sql = sql + " " + p.PayRate.ToString() + " ";
//                            if (p.HoursPayed > 0)
//                            {
//                                sql = sql + System.Convert.ToString(p.PayAmount/p.HoursPayed);
//                            }
//                            else
//                            {
//                                sql = sql + "0";
//                            }
                            sql = sql + ", " + p.HoursPayed.ToString() + ", " + p.PayAmount.ToString() + ",  'PAYROLL', '" + UserName + "', '" + System.DateTime.Now.ToString() + "' ) ";
                            sql = sql + " \n ";
                        }
                    }
                }
            }
             int numofInserts = 0;
            if (sql != "")
            {
                cmd = new SqlCommand(sql, new SqlConnection(connectString)); 
                cmd.CommandTimeout = 360;
                cmd.Connection.Open(); 
                numofInserts = cmd.ExecuteNonQuery();
            }
            int numofInserts2 = 0;
			int numofInserts3 = 0;
            if (sql2 != "")
            {
                string sql3 = " update EmployeeTaskWorked set PayrollStatus = 'PAYROLL',  LastModifiedBy = '" + UserName + "', LastModifiedOn = '" + System.DateTime.Now.ToString() + "' ";
                sql3 = sql3 + " where  ( " + sql2;
                sql3 = sql3 + " ) AND (PayrollStatus = 'PAYROLL' OR PayrollStatus = 'OPEN') \n ";

                SqlCommand cmd2 = new SqlCommand(sql3, new SqlConnection(connectString)); 
                cmd2.CommandTimeout = 360;
                cmd2.Connection.Open(); 
                numofInserts2 = cmd2.ExecuteNonQuery();
			
				string sql4 = " update EmployeeTaskWorked set LastModifiedBy = '" + UserName + "', LastModifiedOn = '" + System.DateTime.Now.ToString() + "' ";
				sql4 = sql4 + " where  ( " + sql2;
				sql4 = sql4 + " ) AND (PayrollStatus <> 'PAYROLL' AND PayrollStatus <> 'OPEN') \n ";

				SqlCommand cmd3 = new SqlCommand(sql4, new SqlConnection(connectString)); 
				cmd3.CommandTimeout = 360;
				cmd3.Connection.Open(); 
				numofInserts3 = cmd3.ExecuteNonQuery();
			}


            Trace.WriteLine("Pay Records Inserted = " + numofInserts.ToString());


            return 0;
        }

        public override string ToString()
        {
            string s = "";

            foreach (Emp e in EmpList)
            {
                s = s + "--" + e.ToString() + "\n";
            }
            return s;
        }


	}
}
