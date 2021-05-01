using System;
using System.Collections.Generic;
using EmployeeDirectory.Infrastructure;
using EmployeeDirectory.Models;

namespace ClientApp.Core
{
    class ServerAccessor : IDataAccessor
    {
        public void AddPhone(int userId, string phoneNumber, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void AddUser(string login, string hashsum, string firstName, out int userId, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void ChangeUser(int userId, string newHashsum, string firstName, string secondName, string middleName, DateTime? birthday, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetAllEmployees(int from, int count, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetEmployeesByName(string firstName, string secondName, string middleName, int from, int count, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Phone> GetPhonesById(int idEmployee, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void RemovePhone(string phoneNumber, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int userId, out ResultCode resultCode)
        {
            throw new NotImplementedException();
        }
    }
}
