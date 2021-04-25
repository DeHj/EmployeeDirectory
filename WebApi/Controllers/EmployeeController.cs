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
        private DbAccessor dbAccessor = new DbAccessor("connection string");

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("get-all-employees")]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees(int from, int count)
        {
            StoredProcedureResultCode resultCode;
            IEnumerable<Employee> result = dbAccessor.GetAllEmployees(from, count, out resultCode);

            if (resultCode == StoredProcedureResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Employee>>(result);
        }

        [HttpGet]
        [Route("get-employees-by-name")]
        public ActionResult<IEnumerable<Employee>> GetEmployeesByName(
            string firstName, string secondName, string middleName,
            int from, int count)
        {
            StoredProcedureResultCode resultCode;
            IEnumerable<Employee> result = dbAccessor.GetEmployeesByName(firstName, secondName, middleName, from, count, out resultCode);

            if (result.Count() == 0)
                return new EmptyResult();
            else if (resultCode == StoredProcedureResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Employee>>(result);
        }

        [HttpGet]
        [Route("get-phones")]
        public ActionResult<IEnumerable<Phone>> GetPhonesById(int employeeId)
        {
            StoredProcedureResultCode resultCode;
            IEnumerable<Phone> result = dbAccessor.GetPhonesById(employeeId, out resultCode);

            if (resultCode == StoredProcedureResultCode.NotExist)
                return StatusCode(406);
            if (resultCode == StoredProcedureResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<IEnumerable<Phone>>(result);
        }

        [HttpPut]
        [Route("add-user")]
        public ActionResult<int> AddUser(Employee employee)
        {
            // Check employee validaty

            StoredProcedureResultCode resultCode;
            int employeeId;

            dbAccessor.AddUser(employee.Login, "", employee.FirstName, out employeeId, out resultCode);
            if (resultCode == StoredProcedureResultCode.AlreadyExist)
                return StatusCode(409);

            if (employee.SecondName != null || employee.MiddleName != null || employee.BirthDay != null)
            {
                dbAccessor.ChangeUser(employeeId, "", "", null, employee.SecondName, employee.MiddleName, employee.BirthDay, out resultCode);
            }

            if (resultCode == StoredProcedureResultCode.InternalError)
                return StatusCode(500);

            return new ActionResult<int>(employeeId);
        }

        [HttpPut]
        [Route("add-phone")]
        public ActionResult AddPhone(Phone phone)
        {
            // Check phone validaty

            StoredProcedureResultCode resultCode;

            dbAccessor.AddPhone(phone.IdEmployee, phone.PhoneValue, out resultCode);

            if (resultCode == StoredProcedureResultCode.NotExist)
                return StatusCode(406);

            if (resultCode == StoredProcedureResultCode.AlreadyExist)
                return StatusCode(409);

            if (resultCode == StoredProcedureResultCode.InternalError)
                return StatusCode(500);

            return new OkResult();
        }





    }
}
