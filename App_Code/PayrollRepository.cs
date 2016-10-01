using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class PayrollRepository
    {
        public List<InterrailTask> GetFacilityTasks(int facilityId)
        {
            var facilityTasksList = new List<InterrailTask>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetTasksByFacilityId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var facilityTask = new InterrailTask()
                    {
                        TaskId = Convert.ToInt32(reader["TaskId"].ToString()),
                        OtherTaskId = Convert.ToInt32(reader["OtherTaskId"].ToString()),
                        TaskCode = reader["TaskCode"].ToString(),
                        TaskDescription = reader["TaskDescription"].ToString()
                    };

                    facilityTasksList.Add(facilityTask);
                }
            }

            return facilityTasksList;
        }

        public double GetUpmForCurrentTask(int rebillDetailId, int facilityId)
        {
            double upm = new Double();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetUpmForCurrentTask", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rebillDetailId", SqlDbType.Int).Value = rebillDetailId;
                cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    upm = Convert.ToDouble(reader["upm"]);
                }
            }

            return upm;
        }

        public InterrailTaskWorked GetTaskWorked(int taskWorkedId)
        {
            var taskWorked = new InterrailTaskWorked();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetTaskWorkedById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@taskWorkedId", SqlDbType.Int).Value = taskWorkedId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    taskWorked.TaskWorkedId = Convert.ToInt32(reader["Id"]);
                    taskWorked.TaskId = Convert.ToInt32(reader["TaskId"]);
                    taskWorked.OtherTaskId = Convert.ToInt32(reader["OtherTaskId"]);
                    taskWorked.FacilityId = Convert.ToInt32(reader["FacilityId"]);
                    taskWorked.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    taskWorked.RebillDetailId = Convert.ToInt32(reader["RebillDetailId"]);
                    taskWorked.WorkDate = reader["WorkDate"].ToString();
                    taskWorked.ShiftId = Convert.ToInt32(reader["ShiftId"]);
                    taskWorked.Upm = Convert.ToDouble(reader["Upm"]);
                    taskWorked.HoursWorked = Convert.ToDouble(reader["HoursWorked"]);
                    taskWorked.PayrollStatus = reader["PayrollStatus"].ToString();
                    taskWorked.OutOfTownType = reader["OutOfTownType"].ToString();
                    taskWorked.LastModifiedBy = reader["LastModifiedBy"].ToString();
                    taskWorked.LastModifiedOn = reader["LastModifiedOn"].ToString();
                    taskWorked.Notes = reader["Notes"].ToString();
                }
            }

            return taskWorked;
        }

        public int SaveTaskWorked(InterrailTaskWorked taskWorked)
        {
            var newId = 0;            

            try
            {
                if (taskWorked.TaskWorkedId == 0)
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                    using (SqlCommand cmd = new SqlCommand("SaveTaskWorked", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@taskId", SqlDbType.Int).Value = taskWorked.TaskId;
                        cmd.Parameters.Add("@otherTaskID", SqlDbType.Int).Value = taskWorked.OtherTaskId;
                        cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = taskWorked.FacilityId;
                        cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = taskWorked.EmployeeId;
                        cmd.Parameters.Add("@rebillDetailID", SqlDbType.Int).Value = taskWorked.RebillDetailId;
                        cmd.Parameters.Add("@workDate", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(taskWorked.WorkDate);
                        cmd.Parameters.Add("@shiftId", SqlDbType.Char).Value = taskWorked.ShiftId.ToString();
                        cmd.Parameters.Add("@upm", SqlDbType.Decimal).Value = taskWorked.Upm;
                        cmd.Parameters.Add("@hoursWorked", SqlDbType.Decimal).Value = taskWorked.HoursWorked;
                        cmd.Parameters.Add("@payrollStatus", SqlDbType.Char).Value = taskWorked.PayrollStatus;
                        cmd.Parameters.Add("@outOfTownType", SqlDbType.Char).Value = taskWorked.OutOfTownType;
                        cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = taskWorked.LastModifiedBy;
                        cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = taskWorked.LastModifiedOn;
                        cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = taskWorked.Notes;

                        var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        newId = (int)returnParameter.Value;
                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                    using (SqlCommand cmd = new SqlCommand("UpdateTaskWorked", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@taskWorkedId", SqlDbType.Int).Value = taskWorked.TaskWorkedId;
                        cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = taskWorked.EmployeeId;
                        cmd.Parameters.Add("@hoursWorked", SqlDbType.Decimal).Value = taskWorked.HoursWorked;
                        cmd.Parameters.Add("@outOfTownType", SqlDbType.Char).Value = taskWorked.OutOfTownType;
                        cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = taskWorked.LastModifiedBy;
                        cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = taskWorked.LastModifiedOn;
                        cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = taskWorked.Notes;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        newId = taskWorked.TaskWorkedId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return newId;
        }

        public void DeleteTaskWorked(int taskWorkedId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("DeleteTaskWorked", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@taskWorkedId", SqlDbType.Int).Value = taskWorkedId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}