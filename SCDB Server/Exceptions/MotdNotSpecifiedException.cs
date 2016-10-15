using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class MotdNotSpecifiedException : Exception
    {
        public MotdNotSpecifiedException(string message) : base(message){ }
    }
}
