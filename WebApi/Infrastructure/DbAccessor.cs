using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EmployeeDirectory;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Infrastructure
{
    public delegate T ReadFromSDR<T>(in SqlDataReader sqlDataReader);

    public class DbAccessor : IDbAccessor
    {
        private string connectionString { get; }

        private IList<Employee> employees = new List<Employee>();
        private IList<Phone> phones = new List<Phone>();
        public IList<string> ints = new List<string>() { "1", "2", "3" };

        public DbAccessor (string connectionString)
        {
            this.connectionString = connectionString;
        }

        
        private Employee readEmployee(in SqlDataReader sqlDataReader)
        {
            return new Employee
            {
                Login = (string)sqlDataReader.GetValue(0),
                FirstName = (string)sqlDataReader.GetValue(1),
                SecondName = (string)sqlDataReader.GetValue(2),
                MiddleName = (string)sqlDataReader.GetValue(3),
                BirthDay = (DateTime)sqlDataReader.GetValue(4),
                Id = (int)sqlDataReader.GetValue(5)
            };
        }
        private Phone readPhone(in SqlDataReader sqlDataReader)
        {
            return new Phone
            {
                PhoneValue = (string)sqlDataReader.GetValue(0),
                IdEmployee = (int)sqlDataReader.GetValue(1)
            };
        }



        private IEnumerable<T> GetCollectionByProcedure<T>(
            string procedureName,
            SqlParameter[] parameters,
            ReadFromSDR<T> readFromSDR
            )
        {
            var result = new List<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                var sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    result.Add(readFromSDR(sdr));
                }
            }

            return result;
        }



        public IEnumerable<Employee> GetAllEmployees(
            int from,
            int count,
            out StoredProcedureResultCode resultCode)
        {
            resultCode = StoredProcedureResultCode.OK;
            return employees;

            try
            {
                resultCode = StoredProcedureResultCode.OK;
                return GetCollectionByProcedure(
                "GetAllEmployees",
                new SqlParameter[] {
                    new SqlParameter("from", from),
                    new SqlParameter("page_size", count)
                },
                readEmployee
                );
            }
            catch
            {
                resultCode = StoredProcedureResultCode.InternalError;
                return null;
            }
        }

        public IEnumerable<Employee> GetEmployeesByName(
            string firstName,
            string secondName,
            string middleName,
            int from,
            int count,
            out StoredProcedureResultCode resultCode)
        {
            resultCode = StoredProcedureResultCode.OK;
            return employees.Where(employee => (employee.FirstName == firstName));

            try
            {
                resultCode = StoredProcedureResultCode.OK;
                return GetCollectionByProcedure(
                "GetEmployeesByName",
                new SqlParameter[] {
                    new SqlParameter("first_name", firstName),
                    new SqlParameter("second_name", secondName),
                    new SqlParameter("middle_name", middleName),
                    new SqlParameter("from", from),
                    new SqlParameter("page_size", count)
                },
                readEmployee
                );
            }
            catch
            {
                resultCode = StoredProcedureResultCode.InternalError;
                return null;
            }
        }

        public IEnumerable<Phone> GetPhonesById(
            int idEmployee,
            out StoredProcedureResultCode resultCode)
        {
            resultCode = StoredProcedureResultCode.OK;
            return phones.Where(phone => (phone.IdEmployee == idEmployee));

            try
            {
                resultCode = StoredProcedureResultCode.OK;
                return GetCollectionByProcedure(
                "GetEmployeesByName",
                new SqlParameter[] {
                    new SqlParameter("id employee", idEmployee)
                },
                readPhone
                );
            }
            catch
            {
                resultCode = StoredProcedureResultCode.InternalError;
                return null;
            }
        }

        public void AddUser(
            string login, string hashsum, string firstName, out int userId, out StoredProcedureResultCode resultCode)
        {
            Random R = new Random();
            userId = R.Next();
            employees.Add(new Employee { Login = login, FirstName = firstName, Id = userId });
            resultCode = StoredProcedureResultCode.OK;
        }

        public void AddPhone(int userId, string phoneNumber, out StoredProcedureResultCode resultCode)
        {
            phones.Add(new Phone { IdEmployee = userId, PhoneValue = phoneNumber });
            resultCode = StoredProcedureResultCode.OK;
        }

        public void RemoveUser(string userId, out StoredProcedureResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void ChangeUser(int userId, string hashsum, string newHashsum, string firstName, string secondName, string middleName, DateTime? birthday, out StoredProcedureResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void RemovePhone(string phoneNumber, out StoredProcedureResultCode resultCode)
        {
            throw new NotImplementedException();
        }
    }
}
