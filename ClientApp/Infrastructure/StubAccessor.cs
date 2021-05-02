using System;
using System.Collections.Generic;
using EmployeeDirectory.Infrastructure;
using EmployeeDirectory.Models;
using System.Linq;

namespace ClientApp.Infrastructure
{
    class StubAccessor : IDataAccessor
    {
        IList<Phone> Phones = new List<Phone>()
        {
            new Phone
            {
                IdEmployee = 0,
                PhoneNumber = "89123340840"
            },
            new Phone
            {
                IdEmployee = 0,
                PhoneNumber = "89120000000"
            },
            new Phone
            {
                IdEmployee = 1,
                PhoneNumber = "89122327720"
            }
        };

        IList<Employee> Employees = new List<Employee>()
        {
            new Employee
            {
                Id = 0,
                Login = "dehabs",
                FirstName = "Denis"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            },
            new Employee
            {
                Id = 1,
                Login = "mymom",
                BirthDay = new DateTime(1965, 12, 9),
                FirstName = "Natasha"
            }
        };




        public void AddPhone(int userId, string phoneNumber, out ResultCode resultCode)
        {
            Phones.Add(new Phone { IdEmployee = userId, PhoneNumber = phoneNumber });

            resultCode = ResultCode.OK;
        }

        public void AddEmployee(string login, string hashsum, string firstName, out int userId, out ResultCode resultCode)
        {
            userId = new Random().Next();

            Employees.Add(new Employee { FirstName = firstName, Id = userId, Login = login });

            resultCode = ResultCode.OK;
        }

        public void ChangeEmployee(int userId, string newHashsum, string firstName, string secondName, string middleName, DateTime? birthday, out ResultCode resultCode)
        {
            Employee employee = Employees.Where((Employee e) => e.Id == userId).First();
            
            employee.FirstName = firstName ?? employee.FirstName;
            employee.SecondName = secondName ?? employee.SecondName;
            employee.MiddleName = middleName ?? employee.MiddleName;
            employee.BirthDay = birthday ?? employee.BirthDay;

            resultCode = ResultCode.OK;
            return;
        }

        public IEnumerable<Employee> GetAllEmployees(int from, int count, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;
            return Employees;
        }

        public IEnumerable<Employee> GetEmployeesByName(string firstName, string secondName, string middleName, int from, int count, out ResultCode resultCode)
        {
            if (firstName == null) firstName = "";
            if (secondName == null) secondName = "";
            if (middleName == null) middleName = "";

            resultCode = ResultCode.OK;
            return Employees.Where((Employee e)
                =>
            {
                string fn = e.FirstName ?? "";
                string sn = e.SecondName ?? "";
                string mn = e.MiddleName ?? "";

                return fn.Contains(firstName) && sn.Contains(secondName) && mn.Contains(middleName);
            }
                );
        }

        public Employee GetEmployeeById(int idEmployee, out ResultCode resultCode)
        {
            var employees = Employees.Where((Employee e) => e.Id == idEmployee);
            if (employees.Any())
            {
                resultCode = ResultCode.OK;
                return employees.First();
            }
            else
            {
                resultCode = ResultCode.NotExist;
                return null;
            }
        }

        public IEnumerable<Phone> GetPhonesById(int idEmployee, out ResultCode resultCode)
        {
            resultCode = ResultCode.OK;
            return Phones.Where((Phone p) => p.IdEmployee == idEmployee);
        }

        public void RemovePhone(string phoneNumber, out ResultCode resultCode)
        {
            var phones = Phones.Where((Phone p) => { return p.PhoneNumber == phoneNumber; });
            if (phones.Any() == false)
            {
                resultCode = ResultCode.NotExist;
                return;
            }

            Phones.Remove(phones.First());
            resultCode = ResultCode.OK;
        }

        public void RemoveEmployee(int userId, out ResultCode resultCode)
        {
            var employees = Employees.Where((Employee e) => e.Id == userId);

            if (employees.Any())
            {
                Employees.Remove(employees.First());
                resultCode = ResultCode.OK;
            }
            else
            {
                resultCode = ResultCode.NotExist;
            }
        }
    }
}
