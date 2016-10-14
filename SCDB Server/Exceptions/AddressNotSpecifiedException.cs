using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class AddressNotSpecifiedException : Exception
    {
        public AddressNotSpecifiedException(string message) : base(message) { }
    }
}
