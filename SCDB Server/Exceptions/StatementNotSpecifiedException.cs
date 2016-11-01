using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if one of the SQL statements is not specified in the App.config.
    /// </summary>
    public class StatementNotSpecifiedException : Exception
    {
        public StatementNotSpecifiedException(string message) : base(message){ }
    }
}
