using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class DatabaseConnectException : Exception
    {
        public DatabaseConnectException(string message) : base(message){ }
    }
}
