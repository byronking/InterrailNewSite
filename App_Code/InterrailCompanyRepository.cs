using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailCompanyRepository
    {
        public List<InterrailCompany> GetAllInterrailCompanies()
        {
            var companiesList = new List<InterrailCompany>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllInterrailCompanies", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var company = new InterrailCompany()
                    {
                        ID = reader.GetInt32(0),
                        CompanyId = reader.GetString(1),
                        CompanyName = reader.GetString(2),
                        LogoPath = reader.GetString(3),
                        PayPeriodId = reader.GetInt32(4),
                        PayrollCompanyCode = reader.GetString(5),
                        OutOfTownRate = reader.GetDecimal(6),
                        OutOfTownHoursPerDay = reader.GetDecimal(7),
                        LastModifiedBy = reader.GetString(8),
                        LastModifiedOn = reader.GetDateTime(9),
                        Active = reader.GetBoolean(10)
                    };

                    companiesList.Add(company);
                }
            }

            return companiesList;
        }

        public List<InterrailCompany> SearchInterrailCompanies(string searchCriteria)
        {
            var companiesList = new List<InterrailCompany>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("SearchInterrailCompanies", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@searchCriteria", SqlDbType.VarChar, 50).Value = searchCriteria;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var company = new InterrailCompany()
                    {
                        ID = reader.GetInt32(0),
                        CompanyId = reader.GetString(1),
                        CompanyName = reader.GetString(2),
                        LogoPath = reader.GetString(3),
                        PayPeriodId = reader.GetInt32(4),
                        PayrollCompanyCode = reader.GetString(5),
                        OutOfTownRate = reader.GetDecimal(6),
                        OutOfTownHoursPerDay = reader.GetDecimal(7),
                        LastModifiedBy = reader.GetString(8),
                        LastModifiedOn = reader.GetDateTime(9),
                        Active = reader.GetBoolean(10)
                    };

                    companiesList.Add(company);
                }
            }

            return companiesList;
        }
    }
}