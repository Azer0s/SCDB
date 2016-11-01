using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if the listening address is not specified in the App.config.
    /// </summary>
    public class AddressNotSpecifiedException : Exception
    {
        public AddressNotSpecifiedException(string message) : base(message) { }
    }
}
