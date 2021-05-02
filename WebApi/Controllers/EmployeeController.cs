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

namespace WebApi.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private IDataAccessor dbAccessor = new DbAccessor("Server = WIN-NA5S2RO1BDR; Database=EmployeesDirectory; Trusted_Connection=True;");

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        [Route("get-all-employees/{from}-{count}")]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees(int from, int count)
        {
            ResultCode resultCode;
            IEnumerable<Employee> result = dbAccessor.GetAllEmployees(from, count, out resultCode);

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
            ResultCode resultCode;
            IEnumerable<Employee> result = dbAccessor.GetEmployeesByName(firstName, secondName, middleName, from, count, out resultCode);

            if (result.Count() == 0)
                return new EmptyResult();
            else if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Employee>>(result);
        }



        [HttpGet]
        [Route("get-phones/{employeeId}")]
        public ActionResult<IEnumerable<Phone>> GetPhonesById(int employeeId)
        {
            ResultCode resultCode;
            IEnumerable<Phone> result = dbAccessor.GetPhonesById(employeeId, out resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);
            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Phone>>(result);
        }



        [HttpPut]
        [Route("add-employee")]
        public ActionResult<int> AddUser(Employee employee)
        {
            // Check employee validaty

            ResultCode resultCode;
            int employeeId;

            dbAccessor.AddUser(employee.Login, "", employee.FirstName, out employeeId, out resultCode);
            if (resultCode == ResultCode.AlreadyExist)
                return StatusCode(409);

            if (employee.SecondName != null || employee.MiddleName != null || employee.BirthDay != null)
            {
                dbAccessor.ChangeUser(employeeId, "", null, employee.SecondName, employee.MiddleName, employee.BirthDay, out resultCode);
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

            ResultCode resultCode;

            dbAccessor.AddPhone(phone.IdEmployee, phone.PhoneValue, out resultCode);

            if (resultCode == ResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == ResultCode.AlreadyExist)
                return StatusCode(409);

            if (resultCode == ResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }
    }
}
