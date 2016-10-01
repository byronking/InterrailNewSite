using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailFacilityRepository
    {
        public List<InterrailFacility> GetAllFacilities()
        {
            var facilitiesList = new List<InterrailFacility>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllFacilities", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var facility = new InterrailFacility()
                    {
                        FacilityId = Convert.ToInt32(reader["Id"]),
                        FacilityNumber = Convert.ToInt32(reader["FacilityNumber"]),
                        AlphaCode = reader["AlphaCode"].ToString(),
                        FacilityName = reader["Name"].ToString(),
                        Address1 = reader["Address1"].ToString(),
                        Address2 = reader["Address2"].ToString(),
                        Address3 = reader["Address3"].ToString(),
                        DefaultTaskId = Convert.ToInt32(reader["DefaultTaskId"]),
                        DefaultShiftId = reader["DefaultShiftId"].ToString(),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        OvertimeCalcBasis = Convert.ToInt32(reader["OvertimeCalcBasis"]),
                        GlCostCenter = reader["GlCostCenter"].ToString(),
                        IrgCompanyId = Convert.ToInt32(reader["IrgCompanyId"]),
                        LastModifiedBy = reader["LastModifiedBy"].ToString(),
                        LastModifiedOn = Convert.ToDateTime(reader["LastModifiedOn"]),
                        Active = Convert.ToBoolean(reader["Active"]),
                        BudgetedCpu = Convert.ToDecimal(reader["BudgetedCpu"])
                    };

                    facilitiesList.Add(facility);
                }
            }

            return facilitiesList;
        }

        public List<InterrailFacility> SearchFacilities(string searchCriteria)
        {
            var facilitiesList = new List<InterrailFacility>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("SearchFacilities", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@searchCriteria", SqlDbType.VarChar, 50).Value = searchCriteria;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var facility = new InterrailFacility()
                    {
                        FacilityId = Convert.ToInt32(reader["Id"]),
                        FacilityNumber = Convert.ToInt32(reader["FacilityNumber"]),
                        AlphaCode = reader["AlphaCode"].ToString(),
                        FacilityName = reader["Name"].ToString(),
                        Address1 = reader["Address1"].ToString(),
                        Address2 = reader["Address2"].ToString(),
                        Address3 = reader["Address3"].ToString(),
                        DefaultTaskId = Convert.ToInt32(reader["DefaultTaskId"]),
                        DefaultShiftId = reader["DefaultShiftId"].ToString(),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                        OvertimeCalcBasis = Convert.ToInt32(reader["OvertimeCalcBasis"]),
                        GlCostCenter = reader["GlCostCenter"].ToString(),
                        IrgCompanyId = Convert.ToInt32(reader["IrgCompanyId"]),
                        LastModifiedBy = reader["LastModifiedBy"].ToString(),
                        LastModifiedOn = Convert.ToDateTime(reader["LastModifiedOn"]),
                        Active = Convert.ToBoolean(reader["Active"]),
                        BudgetedCpu = Convert.ToDecimal(reader["BudgetedCpu"])
                    };

                    facilitiesList.Add(facility);
                }
            }

            return facilitiesList;
        }
    }
}