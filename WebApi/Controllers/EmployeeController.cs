using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDirectory.Infrastructure;
using EmployeeDirectory.Models;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IDataAccessor dbAccessor;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("Properties/configs.json")
                .Build();

            dbAccessor = new DbAccessor(configuration.GetConnectionString("DefaultConnection"));
        }



        [HttpGet]
        [Route("get-all-employees/{from}-{count}")]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees(int from, int count)
        {
            IEnumerable<Employee> result = dbAccessor.GetAllEmployees(from, count, out ResultCode resultCode);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Employee>>(result);
        }



        [HttpGet]
        [Route("get-employees-by-name/{from}-{count}-{firstName}-{secondName}-{middleName}")]
        public ActionResult<IEnumerable<Employee>> GetEmployeesByName(
            string firstName, string secondName, string middleName,
            int from, int count)
        {
            if (firstName == "_")
                firstName = null;
            if (secondName == "_")
                secondName = null;
            if (middleName == "_")
                middleName = null;

            IEnumerable<Employee> result = dbAccessor.GetEmployeesByName(firstName, secondName, middleName, from, count, out ResultCode resultCode);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Employee>>(result);
        }



        [HttpGet]
        [Route("get-employee-by-id/{idEmployee}")]
        public ActionResult<Employee> GetEmployeeById(int idEmployee)
        {
            Employee result = dbAccessor.GetEmployeeById(idEmployee, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);
            else if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<Employee>(result);
        }



        [HttpGet]
        [Route("get-phones/{employeeId}")]
        public ActionResult<IEnumerable<Phone>> GetPhonesById(int employeeId)
        {
            IEnumerable<Phone> result = dbAccessor.GetPhonesById(employeeId, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);
            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Phone>>(result);
        }



        [HttpPut]
        [Route("add-employee")]
        public ActionResult<int> AddEmployee(Employee employee)
        {
            // Check employee validaty

            dbAccessor.AddEmployee(employee.Login, "", employee.FirstName, out int employeeId, out ResultCode resultCode);
            if (resultCode == ResultCode.AlreadyExist)
                return StatusCode(409);

            if (employee.SecondName != null || employee.MiddleName != null || employee.BirthDay != null)
            {
                dbAccessor.ChangeEmployee(employeeId, "", null, employee.SecondName, employee.MiddleName, employee.BirthDay, out resultCode);
            }

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<int>(employeeId);
        }



        [HttpPut]
        [Route("add-phone")]
        public ActionResult AddPhone(Phone phone)
        {
            // Check phone validaty

            dbAccessor.AddPhone(phone.IdEmployee, phone.PhoneNumber, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == ResultCode.AlreadyExist)
                return StatusCode(409);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }



        [HttpPut]
        [Route("remove-employee/{idEmployee}")]
        public ActionResult RemoveEmployee(int idEmployee)
        {
            dbAccessor.RemoveEmployee(idEmployee, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }



        [HttpPut]
        [Route("remove-phone/{phoneNumber}")]
        public ActionResult RemovePhone(string phoneNumber)
        {
            dbAccessor.RemovePhone(phoneNumber, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }



        [HttpPut]
        [Route("change-employee")]
        public ActionResult RemovePhone(Employee employee)
        {
            dbAccessor.ChangeEmployee(employee.Id, "", employee.FirstName, employee.SecondName, employee.MiddleName, employee.BirthDay, out ResultCode resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }
    }
}
