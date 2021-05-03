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


        private const string invalidSymbols = ".-~:/?#[]@!$#&'()*+,;=%";

        /// <summary>
        /// Check model on the validity
        /// </summary>
        public bool IsValid()
        {
            static bool isValid(string s)
            {
                foreach (var ch in s)
                {
                    if (invalidSymbols.Contains(ch))
                        return false;
                }
                return true;
            }


            if (FirstName.Length == 0 || isValid(Login) == false)
                return false;
            if (FirstName.Length == 0 || isValid(FirstName) == false)
                return false;

            if (SecondName != null && isValid(SecondName) == false)
                return false;
            if (MiddleName != null && isValid(MiddleName) == false)
                return false;

            return true;
        }
    }
}