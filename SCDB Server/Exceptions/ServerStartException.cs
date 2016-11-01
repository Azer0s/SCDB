using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the program fails to start the Nancy Server.
    /// </summary>
    public class ServerStartException : Exception
    {
        public ServerStartException(string message) : base(message) { }
    }
}
