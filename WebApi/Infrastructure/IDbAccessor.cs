using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Infrastructure
{
    public interface IDbAccessor
    {
        /// <summary>
        /// Get part of the collection of all employees
        /// </summary>
        /// <param name="from">First result of a part of requested data</param>
        /// <param name="count">Result count of a part of requested data</param>
        /// <param name="resultCode">Result code of request</param>
        IEnumerable<Employee> GetAllEmployees(
            int from,
            int count,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Get part of the employee collection by name template
        /// </summary>
        /// <param name="firstName">Template of the first name</param>
        /// <param name="secondName">Template of the second name</param>
        /// <param name="middleName">Template of the middle name</param>
        /// <param name="from">First result of a part of requested data</param>
        /// <param name="count">Result count of a part of requested data</param>
        /// <param name="resultCode">Result code of request</param>
        IEnumerable<Employee> GetEmployeesByName(
            string firstName,
            string secondName,
            string middleName,
            int from,
            int count,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Get collection of phone numbers of the employee
        /// </summary>
        /// <param name="idEmployee">Id of the employee</param>
        /// <param name="resultCode">Result code of request</param>
        IEnumerable<Phone> GetPhonesById(
            int idEmployee,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Add new user to the system
        /// </summary>
        /// <param name="login">New user login</param>
        /// <param name="hashsum">New user password hashsum</param>
        /// <param name="firstName">New user first name</param>
        /// <param name="userId">Id of the new user</param>
        /// <param name="resultCode">Result code of the operation</param>
        void AddUser(
            string login,
            string hashsum,
            string firstName,
            out int userId,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Remove user from the system
        /// </summary>
        /// <param name="userId">Id of the user to delete</param>
        /// <param name="login">Login of the user to delete</param>
        /// <param name="hashsum">Hashsum of the password of the user to delete</param>
        /// <param name="resultCode">Result code of the operation</param>
        void RemoveUser(
            string userId,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Change user data
        /// </summary>
        /// <param name="login">Login of the user</param>
        /// <param name="newHashsum">User new password hashsum</param>
        /// <param name="firstName">New user firstname</param>
        /// <param name="secondName">New user secondname</param>
        /// <param name="middleName">New user middlename</param>
        /// <param name="birthday">New user birthday</param>
        /// <param name="resultCode">Result code of the operation</param>
        void ChangeUser(
            int userId,
            string newHashsum,
            string firstName,
            string secondName,
            string middleName,
            DateTime? birthday,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Adds a phone number
        /// </summary>
        /// <param name="userId">Id of the user who adds the phone number</param>
        /// <param name="login">Login of the user</param>
        /// <param name="hashsum">User password hashsum</param>
        /// <param name="phoneNumber">A phone number</param>
        /// <param name="resultCode">Result code of the operation</param>
        void AddPhone(
            int userId,
            string phoneNumber,
            out StoredProcedureResultCode resultCode
            );

        /// <summary>
        /// Removes the phone number
        /// </summary>
        /// <param name="userId">Id of the user who remove the phone number</param>
        /// <param name="login">Login of the user</param>
        /// <param name="hashsum">User password hashsum</param>
        /// <param name="phoneNumber">A phone number</param>
        /// <param name="resultCode">Result code of the operation</param>
        void RemovePhone(
            string phoneNumber,
            out StoredProcedureResultCode resultCode
            );
    }
}
