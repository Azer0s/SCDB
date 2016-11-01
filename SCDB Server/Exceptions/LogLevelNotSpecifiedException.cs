using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the loglevel is not specified in the App.config.
    /// </summary>
    public class LogLevelNotSpecifiedException : Exception
    {
        public LogLevelNotSpecifiedException(string message) : base(message){ }
    }
}
