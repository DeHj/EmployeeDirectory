using System;
using System.Collections.Generic;
using EmployeeDirectory.Infrastructure;
using EmployeeDirectory.Models;
using System.Linq;
using System.Net.Http;
using System.Net;

namespace ClientApp.Infrastructure
{
    class ServerAccessor : IDataAccessor
    {
        HttpClient client = new HttpClient();
        string urlPrefix;


        public ServerAccessor(string urlPrefix)
        {
            this.urlPrefix = urlPrefix;
        }



        public void AddEmployee(string login, string hashsum, string firstName, out int idEmployee, out ResultCode resultCode)
        {
            Employee employee = new Employee
            {
                Login = login,
                FirstName = firstName,
            };

            string url = urlPrefix + "employees/add-employee";
            var requestResult = HttpHelper.RequestPutAsync<int>(client, url, employee).Result;

            idEmployee = requestResult.Result;
            resultCode = HttpStatusToResultCode(requestResult.StatusCode);
        }

        public void AddPhone(int idEmployee, string phoneNumber, out ResultCode resultCode)
        {
            Phone employee = new Phone
            {
                IdEmployee = idEmployee,
                PhoneNumber = phoneNumber,
            };

            string url = urlPrefix + "employees/add-phone";
            var requestResult = HttpHelper.RequestPutAsync(client, url, employee).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);
        }

        public void ChangeEmployee(int idEmployee, string newHashsum, string firstName, string secondName, string middleName, DateTime? birthday, out ResultCode resultCode)
        {
            Employee employee = new Employee
            {
                Login = "login",
                FirstName = firstName,
                SecondName = secondName,
                MiddleName = middleName,
                BirthDay = birthday,
            };

            string url = urlPrefix + "employees/change-employee";
            var requestResult = HttpHelper.RequestPutAsync(client, url, employee).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);
        }

        public IEnumerable<Employee> GetAllEmployees(int from, int count, out ResultCode resultCode)
        {
            string url = urlPrefix + $"employees/get-all-employees/{from}-{count}";

            WebServiceResponse<Employee[]> requestResult;
            try
            {
                requestResult = HttpHelper.RequestGetAsync<Employee[]>(client, url).Result;
                resultCode = HttpStatusToResultCode(requestResult.StatusCode);
                return requestResult.Result.ToList();
            }
            catch
            {
                resultCode = ResultCode.ConnectionError;
                return null;
            }
        }

        public Employee GetEmployeeById(int idEmployee, out ResultCode resultCode)
        {
            string url = urlPrefix + $"employees/get-employee-by-id/{idEmployee}";
            var requestResult = HttpHelper.RequestGetAsync<Employee>(client, url).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);

            return requestResult.Result;
        }

        public IEnumerable<Employee> GetEmployeesByName(string firstName, string secondName, string middleName, int from, int count, out ResultCode resultCode)
        {
            firstName ??= "_";
            secondName ??= "_";
            middleName ??= "_";

            string url = urlPrefix + $"employees/get-employees-by-name/{from}-{count}-{firstName}-{secondName}-{middleName}";
            var requestResult = HttpHelper.RequestGetAsync<IEnumerable<Employee>>(client, url).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);

            return requestResult.Result;
        }

        public IEnumerable<Phone> GetPhonesById(int idEmployee, out ResultCode resultCode)
        {
            string url = urlPrefix + $"employees/get-phones/{idEmployee}";
            var requestResult = HttpHelper.RequestGetAsync<IEnumerable<Phone>>(client, url).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);

            return requestResult.Result;
        }

        public void RemoveEmployee(int idEmployee, out ResultCode resultCode)
        {
            string url = urlPrefix + $"employees/remove-employee/{idEmployee}";
            var requestResult = HttpHelper.RequestPutAsync(client, url).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);
        }

        public void RemovePhone(string phoneNumber, out ResultCode resultCode)
        {
            string url = urlPrefix + $"employees/remove-phone/{phoneNumber}";
            var requestResult = HttpHelper.RequestPutAsync(client, url).Result;

            resultCode = HttpStatusToResultCode(requestResult.StatusCode);
        }



        private ResultCode HttpStatusToResultCode(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
                return ResultCode.OK;

            if (statusCode == HttpStatusCode.NotAcceptable)
                return ResultCode.NotExist;

            if (statusCode == HttpStatusCode.Conflict)
                return ResultCode.AlreadyExist;

            if (statusCode == HttpStatusCode.Unauthorized)
                return ResultCode.InvalidLoginOrPassword;

            return ResultCode.InternalError;
        }
    }
}