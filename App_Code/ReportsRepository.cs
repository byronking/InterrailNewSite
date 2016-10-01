using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class ReportsRepository
    {
        public List<WeeklyCpuSummary> GetWeeklyCpuSummaryByDate(DateTime workDate)
        {
            var weeklyCpuSummaryList = new List<WeeklyCpuSummary>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("WeeklyCPUSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@WeekDate", SqlDbType.SmallDateTime).Value = workDate;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int? totalUnits = null;
                        if (reader["TotalUnits"] is DBNull)
                        {
                            totalUnits = 0;
                        }
                        else
                        {
                            totalUnits = Convert.ToInt32(reader["TotalUnits"]);
                        }

                        Decimal? totalPay = null;
                        if (reader["TotalPay"] is DBNull)
                        {
                            totalPay = 0;
                        }
                        else
                        {
                            totalPay = Convert.ToDecimal(reader["TotalPay"]);
                        }

                        Decimal? totalCostPerUnit = null;
                        if (reader["TotalCostPerUnit"] is DBNull)
                        {
                            totalCostPerUnit = 0;
                        }
                        else
                        {
                            totalCostPerUnit = Convert.ToDecimal(reader["TotalCostPerUnit"]);
                        }

                        Decimal? variance = null;
                        if (reader["Vairance"] is DBNull)
                        {
                            variance = 0;
                        }
                        else
                        {
                            variance = Convert.ToDecimal(reader["Vairance"]);
                        }

                        var weeklyCpuSummary = new WeeklyCpuSummary()
                        {
                            Facility = reader["Name"].ToString(),
                            TotalPay = Convert.ToDecimal(totalPay),
                            TotalUnits = Convert.ToInt32(totalUnits),
                            TotalCostPerUnit = Convert.ToDecimal(totalCostPerUnit),
                            Variance = Convert.ToDecimal(variance),
                            PossibleFactorsForVariance = reader["DataValue"].ToString()
                        };

                        weeklyCpuSummaryList.Add(weeklyCpuSummary);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return weeklyCpuSummaryList;
        }
    }
}