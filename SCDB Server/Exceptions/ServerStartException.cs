using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class ServerStartException : Exception
    {
        public ServerStartException(string message) : base(message) { }
    }
}
