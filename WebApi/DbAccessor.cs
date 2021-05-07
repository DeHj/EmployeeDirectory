using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EmployeeDirectory;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Infrastructure
{
    public delegate T ReadFromSDR<T>(in SqlDataReader sqlDataReader);

    public class DbAccessor : IDataAccessor
    {
        private readonly string connectionString;

        public DbAccessor (string connectionString)
        {
            this.connectionString = connectionString;
        }

        private static T DBNullCastToClass<T>(object dbObject) where T : class
            => dbObject is DBNull ? null : (T)dbObject;
        private static T? DBNullCastToStruct<T>(object dbObject) where T : struct
            => dbObject is DBNull ? null : (T)dbObject;

        private Employee ReadEmployee(in SqlDataReader sqlDataReader)
        {
            return new Employee
            {
                Login = (string)sqlDataReader.GetValue(0),
                FirstName = (string)sqlDataReader.GetValue(1),
                SecondName = DBNullCastToClass<string>(sqlDataReader.GetValue(2)),
                MiddleName = DBNullCastToClass<string>(sqlDataReader.GetValue(3)),
                BirthDay = DBNullCastToStruct<DateTime>(sqlDataReader.GetValue(4)),
                Id = (int)sqlDataReader.GetValue(5)
            };
        }
        private Phone ReadPhone(in SqlDataReader sqlDataReader)
        {
            return new Phone
            {
                PhoneNumber = (string)sqlDataReader.GetValue(0),
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
            int from, int count,
            out ResultCode resultCode)
        {
            try
            {
                resultCode = ResultCode.OK;
                return GetCollectionByProcedure(
                "get_all_employees",
                new SqlParameter[] {
                    new SqlParameter("@from", from),
                    new SqlParameter("@page_size", count)
                },
                ReadEmployee
                );
            }
            catch
            {
                resultCode = ResultCode.InternalError;
                return null;
            }
        }



        public IEnumerable<Employee> GetEmployeesByName(
            string firstName, string secondName, string middleName,
            int from, int count,
            out ResultCode resultCode)
        {
            try
            {
                resultCode = ResultCode.OK;
                return GetCollectionByProcedure(
                "get_employees_by_name",
                new SqlParameter[] {
                    new SqlParameter("@first_name", firstName),
                    new SqlParameter("@second_name", secondName),
                    new SqlParameter("@middle_name", middleName),
                    new SqlParameter("@from", from),
                    new SqlParameter("@page_size", count)
                },
                ReadEmployee
                );
            }
            catch
            {
                resultCode = ResultCode.InternalError;
                return null;
            }
        }

        public Employee GetEmployeeById(int idEmployee, out ResultCode resultCode)
        {
            try
            {
                resultCode = ResultCode.OK;
                var employees = GetCollectionByProcedure(
                "get_employee_by_id",
                new SqlParameter[] {
                    new SqlParameter("@id_employee", idEmployee),
                },
                ReadEmployee
                );

                if (employees.Any() == false)
                {
                    resultCode = ResultCode.NotExist;
                    return null;
                }
                else if (employees.Count() == 1)
                {
                    resultCode = ResultCode.OK;
                    return employees.First();
                }
                else
                    throw new Exception("");
            }
            catch
            {
                resultCode = ResultCode.InternalError;
                return null;
            }
        }

        public IEnumerable<Phone> GetPhonesById(
            int idEmployee,
            out ResultCode resultCode)
        {
            try
            {
                resultCode = ResultCode.OK;
                return GetCollectionByProcedure(
                "get_phones_by_id",
                new SqlParameter[] {
                    new SqlParameter("id_employee", idEmployee)
                },
                ReadPhone
                );
            }
            catch
            {
                resultCode = ResultCode.InternalError;
                return null;
            }
        }



        public void AddEmployee(
            string login, string hashsum, string firstName,
            out int userId, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("add_user", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sqlParams = new List<SqlParameter>(5)
                {
                    new SqlParameter("@login", login),
                    new SqlParameter("@first_name", firstName),
                    new SqlParameter("@hashsum", hashsum),
                    new SqlParameter
                    {
                        ParameterName = "@employee_id",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    },
                    new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    }
                };
                command.Parameters.AddRange(sqlParams.ToArray());

                command.ExecuteNonQuery();

                userId = 0;

                int result = (int)command.Parameters["@result"].Value;
                if (result == 0)
                {
                    resultCode = ResultCode.OK;
                    userId = (int)command.Parameters["@employee_id"].Value;
                }
                else if (result == 1)
                    resultCode = ResultCode.AlreadyExist;
                else
                    resultCode = ResultCode.InternalError;
            }
        }



        public void AddPhone(int userId, string phoneNumber, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("add_phone", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sqlParams = new List<SqlParameter>(3)
                {
                    new SqlParameter("@employee_id", userId),
                    new SqlParameter("@phone_number", phoneNumber),
                    new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    }
                };
                command.Parameters.AddRange(sqlParams.ToArray());

                command.ExecuteNonQuery();

                int result = (int)command.Parameters["@result"].Value;
                if (result == 0)
                    resultCode = ResultCode.OK;
                else if (result == 2627)
                    resultCode = ResultCode.AlreadyExist;
                else if (result == 547)
                    resultCode = ResultCode.NotExist;
                else
                    resultCode = ResultCode.InternalError;
            }
        }



        public void RemoveEmployee(int userId, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete_user", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sqlParams = new List<SqlParameter>(2)
                {
                    new SqlParameter("@employee_id", userId),
                    new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    }
                };
                command.Parameters.AddRange(sqlParams.ToArray());

                command.ExecuteNonQuery();

                int result = (int)command.Parameters["@result"].Value;
                if (result == 0)
                    resultCode = ResultCode.OK;
                else if (result == 1)
                    resultCode = ResultCode.NotExist;
                else
                    resultCode = ResultCode.InternalError;
            }
        }



        public void ChangeEmployee(int userId, string newHashsum, string firstName, string secondName, string middleName, DateTime? birthday, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("change_user", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sqlParams = new List<SqlParameter>(7)
                {
                    new SqlParameter("@employee_id", userId),
                    new SqlParameter("@new_hashsum", newHashsum),
                    new SqlParameter("@first_name", firstName),
                    new SqlParameter("@second_name", secondName),
                    new SqlParameter("@middle_name", middleName),
                    new SqlParameter("@birthday", birthday),
                    new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    }
                };
                command.Parameters.AddRange(sqlParams.ToArray());

                command.ExecuteNonQuery();

                int result = (int)command.Parameters["@result"].Value;
                if (result == 0)
                    resultCode = ResultCode.OK;
                else if (result == 1)
                    resultCode = ResultCode.NotExist;
                else
                    resultCode = ResultCode.InternalError;
            }
        }



        public void RemovePhone(string phoneNumber, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete_phone", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sqlParams = new List<SqlParameter>(2)
                {
                    new SqlParameter("@phone_number", phoneNumber),
                    new SqlParameter
                    {
                        ParameterName = "@result",
                        Direction = System.Data.ParameterDirection.Output,
                        SqlDbType = System.Data.SqlDbType.Int
                    }
                };
                command.Parameters.AddRange(sqlParams.ToArray());

                command.ExecuteNonQuery();

                int result = (int)command.Parameters["@result"].Value;
                if (result == 0)
                    resultCode = ResultCode.OK;
                else if (result == 1)
                    resultCode = ResultCode.NotExist;
                else
                    resultCode = ResultCode.InternalError;
            }
        }
    }
}
