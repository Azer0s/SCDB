using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class LogLevelNotSpecifiedException : Exception
    {
        public LogLevelNotSpecifiedException(string message) : base(message){ }
    }
}
