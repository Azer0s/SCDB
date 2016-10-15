using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class ConnectionStringNotSpecifiedException : Exception
    {
        public ConnectionStringNotSpecifiedException(string message) : base(message){ }
    }
}
