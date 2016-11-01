using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the message of the day is not specified in the App.config.
    /// </summary>
    public class MotdNotSpecifiedException : Exception
    {
        public MotdNotSpecifiedException(string message) : base(message){ }
    }
}
