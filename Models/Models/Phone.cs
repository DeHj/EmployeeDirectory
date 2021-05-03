using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Models
{
    /// <summary>
    /// Represents a phone
    /// </summary>
    public class Phone
    {
        public int IdEmployee { get; set; }
        public string PhoneNumber { get; set; }


        private const string validSymbols = "1234567890";

        /// <summary>
        /// Check model on the validity
        /// </summary>
        public bool IsValid()
        {
            static bool isValid(string s)
            {
                foreach (var ch in s)
                {
                    if (validSymbols.Contains(ch) == false)
                        return false;
                }
                return true;
            }

            return (PhoneNumber != null && PhoneNumber.Length == 11 && isValid(PhoneNumber));
        }
    }
}
