using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailEmployeeRepository
    {
        /// <summary>
        /// This gets a the Interrail employee by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InterrailEmployee GetEmployeeById(int id)
        {
            var employee = new InterrailEmployee();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetEmployeeById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime? hireDate = null;
                    if (reader["hiredate"] is DBNull)
                    {
                        hireDate = null;
                    }
                    else
                    {
                        hireDate = Convert.ToDateTime(reader["hiredate"]);
                    }

                    DateTime? inactiveDate = null;
                    if (reader["inactivedate"] is DBNull)
                    {
                        inactiveDate = null;
                    }
                    else
                    {
                        inactiveDate = Convert.ToDateTime(reader["inactivedate"]);
                    }

                    DateTime? tempStartDate = null;
                    if (reader["tempstartdate"] is DBNull)
                    {
                        tempStartDate = null;
                    }
                    else
                    {
                        tempStartDate = Convert.ToDateTime(reader["tempstartdate"]);
                    }

                    DateTime? terminationDate = null;
                    if (reader["terminationdate"] is DBNull)
                    {
                        terminationDate = null;
                    }
                    else
                    {
                        terminationDate = Convert.ToDateTime(reader["terminationdate"]);
                    }

                    DateTime? birthDate = null;
                    if (reader["birthdate"] is DBNull)
                    {
                        birthDate = null;
                    }
                    else
                    {
                        birthDate = Convert.ToDateTime(reader["birthdate"]);
                    }

                    DateTime? lastModifiedOn = null;
                    if (reader["lastmodifiedon"] is DBNull)
                    {
                        lastModifiedOn = null;
                    }
                    else
                    {
                        lastModifiedOn = Convert.ToDateTime(reader["lastmodifiedon"]);
                    }

                    int? employmentSourceId = null;
                    if (reader["employmentsourceid"] is DBNull)
                    {
                        employmentSourceId = null;
                    }
                    else
                    {
                        employmentSourceId = Convert.ToInt32(reader["employmentsourceid"]);
                    }

                    employee = new InterrailEmployee()
                    {
                        EmployeeId = Convert.ToInt32(reader["id"]),
                        EmployeeNumber = reader["employeenumber"].ToString(),
                        TempNumber = reader["tempnumber"].ToString().Trim(),
                        LastName = reader["lastname"].ToString(),
                        FirstName = reader["firstname"].ToString(),
                        MiddleInitial = reader["middleinitial"].ToString(),
                        HireDate = hireDate,
                        InactiveDate = inactiveDate,
                        TempStartDate = tempStartDate,
                        TerminationDate = terminationDate,
                        Address1 = reader["address1"].ToString(),
                        Address2 = reader["address2"].ToString(),
                        City = reader["city"].ToString(),
                        State = reader["state"].ToString(),
                        ZipCode = reader["zip"].ToString(),
                        SSN = reader["ssn"].ToString(),
                        BirthDate = birthDate,
                        EmployeePhone = reader["employeephone"].ToString(),
                        EmergencyContact = reader["emergencycontact"].ToString(),
                        EmergencyContactPhone = reader["contactphone"].ToString(),
                        TempEmployee = Convert.ToBoolean(reader["tempemployee"]),
                        Salaried = Convert.ToBoolean(reader["salaried"]),
                        EmploymentSourceId = employmentSourceId,
                        FacilityId = Convert.ToInt32(reader["facilityid"]),
                        LastModifiedBy = reader["lastmodifiedby"].ToString(),
                        LastModifiedOn = lastModifiedOn,
                        Active = Convert.ToBoolean(reader["active"])
                    };
                }
            }

            return employee;
        }

        /// <summary>
        /// This gets a the employees hired this week.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<InterrailEmployee> GetEmployeesHiredThisWeek(string startDate, string endDate)
        {
            List<InterrailEmployee> employeesList = new List<InterrailEmployee>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetEmployeesHiredThisWeek", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startDate", SqlDbType.Char).Value = startDate;
                cmd.Parameters.Add("@endDate", SqlDbType.Char).Value = endDate;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime? hireDate = null;
                    if (reader["hiredate"] is DBNull)
                    {
                        hireDate = null;
                    }
                    else
                    {
                        hireDate = Convert.ToDateTime(reader["hiredate"]);
                    }

                    DateTime? inactiveDate = null;
                    if (reader["inactivedate"] is DBNull)
                    {
                        inactiveDate = null;
                    }
                    else
                    {
                        inactiveDate = Convert.ToDateTime(reader["inactivedate"]);
                    }

                    DateTime? tempStartDate = null;
                    if (reader["tempstartdate"] is DBNull)
                    {
                        tempStartDate = null;
                    }
                    else
                    {
                        tempStartDate = Convert.ToDateTime(reader["tempstartdate"]);
                    }
                    
                    DateTime? terminationDate = null;
                    if (reader["terminationdate"] is DBNull)
                    {
                        terminationDate = null;
                    }
                    else
                    {
                        terminationDate = Convert.ToDateTime(reader["terminationdate"]);
                    }

                    DateTime? birthDate = null;
                    if (reader["birthdate"] is DBNull)
                    {
                        birthDate = null;
                    }
                    else
                    {
                        birthDate = Convert.ToDateTime(reader["birthdate"]);
                    }

                    DateTime? lastModifiedOn = null;
                    if (reader["lastmodifiedon"] is DBNull)
                    {
                        lastModifiedOn = null;
                    }
                    else
                    {
                        lastModifiedOn = Convert.ToDateTime(reader["lastmodifiedon"]);
                    }

                    int? employmentSourceId = null;
                    if (reader["employmentsourceid"] is DBNull)
                    {
                        employmentSourceId = null;
                    }
                    else
                    {
                        employmentSourceId = Convert.ToInt32(reader["employmentsourceid"]);
                    }

                    var employee = new InterrailEmployee()
                    {
                        EmployeeId = Convert.ToInt32(reader["id"]),
                        EmployeeNumber = reader["employeenumber"].ToString(),
                        TempNumber = reader["tempnumber"].ToString(),
                        LastName = reader["lastname"].ToString(),
                        FirstName = reader["firstname"].ToString(),
                        MiddleInitial = reader["middleinitial"].ToString(),
                        HireDate = hireDate,
                        InactiveDate = inactiveDate,
                        TempStartDate = tempStartDate,
                        TerminationDate = terminationDate,
                        Address1 = reader["firstname"].ToString(),
                        Address2 = reader["firstname"].ToString(),
                        City = reader["firstname"].ToString(),
                        State = reader["firstname"].ToString(),
                        ZipCode = reader["firstname"].ToString(),
                        SSN = reader["firstname"].ToString(),
                        BirthDate = birthDate,
                        EmployeePhone = reader["employeephone"].ToString(),
                        EmergencyContact = reader["emergencycontact"].ToString(),
                        EmergencyContactPhone = reader["contactphone"].ToString(),
                        TempEmployee = Convert.ToBoolean(reader["tempemployee"]),
                        Salaried = Convert.ToBoolean(reader["salaried"]),
                        EmploymentSourceId = employmentSourceId,
                        FacilityId = Convert.ToInt32(reader["facilityid"]),
                        LastModifiedBy = reader["lastmodifiedby"].ToString(),
                        LastModifiedOn = lastModifiedOn,
                        Active = Convert.ToBoolean(reader["active"])
                    };

                    employeesList.Add(employee);
                }
            }

            return employeesList;
        }

        /// <summary>
        /// This gets the employment sources by facility id.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public List<EmploymentSource> GetEmploymentSourcesByFacilityId(int facilityId)
        {
            var employmentSourceList = new List<EmploymentSource>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetEmploymentSourcesByFacilityId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime? lastModifiedOn = null;
                    if (reader["lastmodifiedon"] is DBNull)
                    {
                        lastModifiedOn = null;
                    }
                    else
                    {
                        lastModifiedOn = Convert.ToDateTime(reader["lastmodifiedon"]);
                    }

                    var employmentSource = new EmploymentSource()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        FacilityId = Convert.ToInt32(reader["facilityid"]),
                        SourceCode = reader["sourcecode"].ToString(),
                        SourceName = reader["sourcename"].ToString(),
                        LastModifiedBy = reader["lastmodifiedby"].ToString(),
                        LastModifiedOn = lastModifiedOn,
                        Active = Convert.ToBoolean(reader["active"])
                    };

                    employmentSourceList.Add(employmentSource);
                }
            }

            return employmentSourceList;
        }

        /// <summary>
        /// This saves an employee.
        /// </summary>
        /// <param name="employee"></param>
        public int SaveInterrailEmployee(InterrailEmployee employee)
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"];
            var newId = 0;

            if (employee.EmployeeId == 0)
            {
                // Create a new employee.
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SaveInterrailEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@employeenumber", SqlDbType.Char).Value = employee.EmployeeNumber ?? string.Empty;
                    cmd.Parameters.Add("@tempnumber", SqlDbType.Char).Value = employee.TempNumber ?? string.Empty; ;
                    cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = employee.LastName;
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = employee.FirstName;
                    cmd.Parameters.Add("@middleinitial", SqlDbType.VarChar).Value = employee.MiddleInitial;

                    if (employee.HireDate != null)
                    {
                        cmd.Parameters.Add("@hiredate", SqlDbType.SmallDateTime).Value = employee.HireDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@hiredate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.InactiveDate != null)
                    {
                        cmd.Parameters.Add("@inactivedate", SqlDbType.SmallDateTime).Value = employee.InactiveDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@inactivedate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.TempStartDate != null)
                    {
                        cmd.Parameters.Add("@tempstartdate", SqlDbType.SmallDateTime).Value = employee.TempStartDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@tempstartdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.TerminationDate != null)
                    {
                        cmd.Parameters.Add("@terminationdate", SqlDbType.SmallDateTime).Value = employee.TerminationDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@terminationdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = employee.Address1;
                    cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = employee.Address2;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = employee.City;
                    cmd.Parameters.Add("@state", SqlDbType.Char).Value = employee.State;
                    cmd.Parameters.Add("@zip", SqlDbType.VarChar).Value = employee.ZipCode;
                    cmd.Parameters.Add("@ssn", SqlDbType.VarChar).Value = employee.SSN;

                    if (employee.BirthDate != null)
                    {
                        cmd.Parameters.Add("@birthdate", SqlDbType.SmallDateTime).Value = employee.BirthDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@birthdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@employeephone", SqlDbType.VarChar).Value = employee.EmployeePhone;
                    cmd.Parameters.Add("@emergencycontact", SqlDbType.VarChar).Value = employee.EmergencyContact;
                    cmd.Parameters.Add("@contactphone", SqlDbType.VarChar).Value = employee.EmergencyContactPhone;
                    cmd.Parameters.Add("@tempemployee", SqlDbType.Bit).Value = employee.TempEmployee;
                    cmd.Parameters.Add("@salaried", SqlDbType.Bit).Value = employee.Salaried;
                    cmd.Parameters.Add("@employmentsourceid", SqlDbType.Int).Value = employee.EmploymentSourceId; 
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = employee.FacilityId;
                    cmd.Parameters.Add("@lastmodifiedby", SqlDbType.VarChar).Value = employee.LastModifiedBy;

                    if (employee.LastModifiedOn != null)
                    {
                        cmd.Parameters.Add("@lastmodifiedon", SqlDbType.SmallDateTime).Value = employee.LastModifiedOn;
                    }
                    else
                    {
                        cmd.Parameters.Add("@lastmodifiedon", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = employee.Active;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    newId = (int)returnParameter.Value;
                }

                return newId;
            }
            else
            {
                // Update an existing employee.
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("UpdateInterrailEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = employee.EmployeeId;
                    cmd.Parameters.Add("@employeenumber", SqlDbType.Char).Value = employee.EmployeeNumber ?? string.Empty;
                    cmd.Parameters.Add("@tempnumber", SqlDbType.Char).Value = employee.TempNumber ?? string.Empty;
                    cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = employee.LastName;
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = employee.FirstName;
                    cmd.Parameters.Add("@middleinitial", SqlDbType.VarChar).Value = employee.MiddleInitial;

                    if (employee.HireDate != null)
                    {
                        cmd.Parameters.Add("@hiredate", SqlDbType.SmallDateTime).Value = employee.HireDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@hiredate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.InactiveDate != null)
                    {
                        cmd.Parameters.Add("@inactivedate", SqlDbType.SmallDateTime).Value = employee.InactiveDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@inactivedate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.TempStartDate != null)
                    {
                        cmd.Parameters.Add("@tempstartdate", SqlDbType.SmallDateTime).Value = employee.TempStartDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@tempstartdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    if (employee.TerminationDate != null)
                    {
                        cmd.Parameters.Add("@terminationdate", SqlDbType.SmallDateTime).Value = employee.TerminationDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@terminationdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = employee.Address1;
                    cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = employee.Address2;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = employee.City;
                    cmd.Parameters.Add("@state", SqlDbType.Char).Value = employee.State;
                    cmd.Parameters.Add("@zip", SqlDbType.VarChar).Value = employee.ZipCode;
                    cmd.Parameters.Add("@ssn", SqlDbType.VarChar).Value = employee.SSN;

                    if (employee.BirthDate != null)
                    {
                        cmd.Parameters.Add("@birthdate", SqlDbType.SmallDateTime).Value = employee.BirthDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@birthdate", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@employeephone", SqlDbType.VarChar).Value = employee.EmployeePhone;
                    cmd.Parameters.Add("@emergencycontact", SqlDbType.VarChar).Value = employee.EmergencyContact;
                    cmd.Parameters.Add("@contactphone", SqlDbType.VarChar).Value = employee.EmergencyContactPhone;
                    cmd.Parameters.Add("@tempemployee", SqlDbType.Bit).Value = employee.TempEmployee;
                    cmd.Parameters.Add("@salaried", SqlDbType.Bit).Value = employee.Salaried;
                    cmd.Parameters.Add("@employmentsourceid", SqlDbType.Int).Value = employee.EmploymentSourceId; 
                    cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = employee.FacilityId;
                    cmd.Parameters.Add("@lastmodifiedby", SqlDbType.VarChar).Value = employee.LastModifiedBy;

                    if (employee.LastModifiedOn != null)
                    {
                        cmd.Parameters.Add("@lastmodifiedon", SqlDbType.SmallDateTime).Value = employee.LastModifiedOn;
                    }
                    else
                    {
                        cmd.Parameters.Add("@lastmodifiedon", SqlDbType.SmallDateTime).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = employee.Active;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }

                return employee.EmployeeId;
            }           
        }

        public List<EmployeeNumber> GetAllEmployeeNumbers()
        {
            var employeeNumbersList = new List<EmployeeNumber>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllEmployeeNumbers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var employeeNumber = new EmployeeNumber()
                    {
                        Number = reader["employeenumber"].ToString()
                    };

                    employeeNumbersList.Add(employeeNumber);
                }
            }

            return employeeNumbersList;
        }

        public List<EmployeesForTasks> GetEmployeesForTasks(int facilityId)
        {
            var employeesForTasksList = new List<EmployeesForTasks>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetEmployeesByFacilityId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityid", SqlDbType.Int).Value = facilityId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var employeeForTasks = new EmployeesForTasks()
                    {
                        EmployeeId = Convert.ToInt32(reader["Id"].ToString()),
                        EmployeeNumber = reader["EmployeeNumber"].ToString(),
                        FacilityId = Convert.ToInt32(reader["FacilityId"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        Lastname = reader["Lastname"].ToString(),
                        MiddleInitial = reader["MiddleInitial"].ToString(),
                        TempNumber = reader["TempNumber"].ToString()
                    };

                    employeesForTasksList.Add(employeeForTasks);
                }
            }

            return employeesForTasksList;
        }

        public int GetNewTempNumber()
        {
            var tempNumber = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("GetNewTempNumber", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tempNumber = Convert.ToInt32(reader["tempNumber"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return tempNumber;
        }

        public bool ValidateEmployeeNumber(int employeeNumber)
        {
            var validEmployeeNumber = false;
            var existingEmployeeNumber = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("ValidateEmployeeNumber", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@employeeNumber", SqlDbType.VarChar).Value = employeeNumber;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        existingEmployeeNumber = Convert.ToInt32(reader["employeeNumber"]);
                    }

                    if (employeeNumber != existingEmployeeNumber)
                    {
                        validEmployeeNumber = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return validEmployeeNumber;
        }
    }
}