using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class RebillRepository
    {
        public List<RebillData> GetRebillDetailByFacilityId(int facilityId)
        {
            var rebillDetailList = new List<RebillData>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetRebillDetailByFacility", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var rebillDetail = new RebillData()
                    {
                        RebillDetailId = Convert.ToInt32(reader["Id"]),
                        WorkDate = Convert.ToDateTime(reader["WorkDate"]),
                        RebillStatus = reader["RebillStatus"].ToString(),
                        TaskCode = reader["TaskCode"].ToString(),
                        Description = reader["Description"].ToString(),
                        TotalHours = Convert.ToDecimal(reader["TotalHours"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        CustomerCode = reader["CustomerCode"].ToString(),
                    };

                    rebillDetailList.Add(rebillDetail);
                }
            }

            return rebillDetailList;
        }

        public List<RebillDetail> GetRebillDetail(int rebillDetailId)
        {
            var rebillDetailList = new List<RebillDetail>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetRebillDetail", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rebillDetailId", SqlDbType.Int).Value = rebillDetailId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var rebillDetail = new RebillDetail()
                    {
                        Description = reader["Description"].ToString(),
                        TaskId = Convert.ToInt32(reader["TaskId"].ToString()),
                        RebillSubTasksId = Convert.ToInt32(reader["RebillSubTasksId"].ToString())
                    };

                    rebillDetailList.Add(rebillDetail);
                }
            }

            return rebillDetailList;
        }

        public List<RebillData> GetRebillDataForApproval(int facilitId)
        {
            var rebillDataList = new List<RebillData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("GetRebillDataForApproval", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilitId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var rebillData = new RebillData()
                        {
                            RebillDetailId = Convert.ToInt32(reader["Id"].ToString()),
                            WorkDate = Convert.ToDateTime(reader["WorkDate"]),
                            RebillStatus = reader["RebillStatus"].ToString().Trim(),
                            Description = reader["Description"].ToString(),
                            TaskCode = reader["TaskCode"].ToString().Trim(),
                            CustomerName = reader["CustomerName"].ToString(),
                            CustomerCode = reader["CustomerCode"].ToString(),
                            TotalHours = Convert.ToDecimal(reader["TotalHours"].ToString()),
                            TotalUnits = Convert.ToInt32(reader["TotalUnits"].ToString()),
                            HoursOrUnits = reader["HoursOrUnits"].ToString()
                        };

                        rebillDataList.Add(rebillData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return rebillDataList;
        }

        public SaveResult ApproveRebillData(List<int> rebillData, string rebillStatus, string lastModifiedBy)
        {
            var saveResult = new SaveResult();

            try
            {
                foreach (var data in rebillData)
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                    using (SqlCommand cmd = new SqlCommand("ApproveRebillData", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = data;
                        cmd.Parameters.Add("@rebillStatus", SqlDbType.VarChar).Value = rebillStatus;
                        cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = lastModifiedBy;
                        cmd.Parameters.Add("lastModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                saveResult.SaveSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                saveResult.SaveMessage = ex.ToString();
            }

            return saveResult;
        }

        public SaveResult ApproveRebillData(DateTime startDate, DateTime endDate, int facilityId, string rebillStatus, string lastModifiedBy)
        {
            var saveResult = new SaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("ApproveRebillDataDateRange", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                    cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;
                    cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                    cmd.Parameters.Add("@rebillStatus", SqlDbType.VarChar).Value = rebillStatus;
                    cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = lastModifiedBy;
                    cmd.Parameters.Add("lastModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }

                saveResult.SaveSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                saveResult.SaveMessage = ex.ToString();
            }

            return saveResult;
        }
    }
}