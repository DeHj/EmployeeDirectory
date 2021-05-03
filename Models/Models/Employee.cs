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
        public DateTime? BirthDay { get; set; }


        public const string InvalidSymbols = ".-~:/?#[]@!$#&'()*+,;=%";

        public static bool StringIsValid(string s)
        {
            foreach (var ch in s)
            {
                if (InvalidSymbols.Contains(ch))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check model on the validity
        /// </summary>
        public bool IsValid()
        {
            if (FirstName.Length == 0 || StringIsValid(Login) == false)
                return false;
            if (FirstName.Length == 0 || StringIsValid(FirstName) == false)
                return false;

            if (SecondName != null && StringIsValid(SecondName) == false)
                return false;
            if (MiddleName != null && StringIsValid(MiddleName) == false)
                return false;

            return true;
        }
    }
}