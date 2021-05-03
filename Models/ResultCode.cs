using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Infrastructure
{
    public enum ResultCode
    {
        OK,
        InvalidLoginOrPassword,
        NotExist,
        AlreadyExist,
        InternalError,
        ConnectionError
    }
}
