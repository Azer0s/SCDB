using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the program is not able to connect to the database.
    /// </summary>
    public class DatabaseConnectException : Exception
    {
        public DatabaseConnectException(string message) : base(message){ }
    }
}
