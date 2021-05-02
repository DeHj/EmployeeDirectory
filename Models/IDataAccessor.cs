using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Infrastructure
{
    public interface IDataAccessor
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
            out ResultCode resultCode
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
            out ResultCode resultCode
            );

        /// <summary>
        /// Get employee with specified id
        /// </summary>
        /// <param name="idEmployee">Id of the employee</param>
        /// <param name="resultCode">Result code of request</param>
        Employee GetEmployeeById(
            int idEmployee,
            out ResultCode resultCode
            );

        /// <summary>
        /// Get collection of phone numbers of the employee
        /// </summary>
        /// <param name="idEmployee">Id of the employee</param>
        /// <param name="resultCode">Result code of request</param>
        IEnumerable<Phone> GetPhonesById(
            int idEmployee,
            out ResultCode resultCode
            );

        /// <summary>
        /// Add new employee to the system
        /// </summary>
        /// <param name="login">New employee login</param>
        /// <param name="hashsum">New employee password hashsum</param>
        /// <param name="firstName">New employee first name</param>
        /// <param name="idEmployee">Id of the new employee</param>
        /// <param name="resultCode">Result code of the operation</param>
        void AddEmployee(
            string login,
            string hashsum,
            string firstName,
            out int idEmployee,
            out ResultCode resultCode
            );

        /// <summary>
        /// Remove employee from the system
        /// </summary>
        /// <param name="idEmployee">Id of the employee to delete</param>
        /// <param name="resultCode">Result code of the operation</param>
        void RemoveEmployee(
            int idEmployee,
            out ResultCode resultCode
            );

        /// <summary>
        /// Change employee data
        /// </summary>
        /// <param name="idEmployee">Id of the employee</param>
        /// <param name="newHashsum">Employee new password hashsum</param>
        /// <param name="firstName">New employee firstname</param>
        /// <param name="secondName">New employee secondname</param>
        /// <param name="middleName">New employee middlename</param>
        /// <param name="birthday">New employee birthday</param>
        /// <param name="resultCode">Result code of the operation</param>
        void ChangeEmployee(
            int idEmployee,
            string newHashsum,
            string firstName,
            string secondName,
            string middleName,
            DateTime? birthday,
            out ResultCode resultCode
            );

        /// <summary>
        /// Adds a phone number
        /// </summary>
        /// <param name="idEmployee">Id of the employee who owns the phone number</param>
        /// <param name="phoneNumber">A phone number</param>
        /// <param name="resultCode">Result code of the operation</param>
        void AddPhone(
            int idEmployee,
            string phoneNumber,
            out ResultCode resultCode
            );

        /// <summary>
        /// Removes the phone number
        /// </summary>
        /// <param name="idEmployee">Id of the employee who remove the phone number</param>
        /// <param name="phoneNumber">A phone number</param>
        /// <param name="resultCode">Result code of the operation</param>
        void RemovePhone(
            string phoneNumber,
            out ResultCode resultCode
            );
    }
}
