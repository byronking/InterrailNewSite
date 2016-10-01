using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class EmployeesRepository
    {
        public List<EmployeesForLists> GetEmployeesForLists(int facilityId)
        {
            var employeesList = new List<EmployeesForLists>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("GetEmployeesByFacilityIdForLists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var employeesForLists = new EmployeesForLists()
                        {
                            Id = Convert.ToInt32(reader["Id"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmployeeNumber = reader["EmployeeNumber"].ToString()
                        };

                        employeesList.Add(employeesForLists);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return employeesList;
        }

        public EmployeesForLists GetEmployeesById(int employeeId)
        {
            var employee = new EmployeesForLists();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("GetEmployeesById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = employeeId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        employee = new EmployeesForLists()
                        {
                            Id = Convert.ToInt32(reader["Id"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmployeeNumber = reader["EmployeeNumber"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return employee;
        }
    }
}