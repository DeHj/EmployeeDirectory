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
        public string PhoneValue { get; set; }


        private const string validSymbols = "1234567890";
        /// <summary>
        /// Checks the validity for PhoneValue symbols
        /// </summary>
        /// <param name="ch">Symbol for check</param>
        /// <returns></returns>
        public bool ValidityCheck(char ch) => validSymbols.Contains(ch);
    }
}
