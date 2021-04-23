using System;

namespace EmployeeDirectory.Models
{
    /// <summary>
    /// Represents an employee
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}