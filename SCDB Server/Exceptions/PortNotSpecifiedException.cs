using System;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the listening port is not specified in the App.config.
    /// </summary>
    public class PortNotSpecifiedException : Exception
    {
        public PortNotSpecifiedException(string message) : base(message){}
    }
}
