using System;

namespace Exceptions
{
    public class PortNotSpecifiedException : Exception
    {
        public PortNotSpecifiedException(string message) : base(message){}
    }
}
