﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    /// <summary>
    /// Exception. Thrown if one of the required connection strings is not specified in the App.config.
    /// </summary>
    public class ConnectionStringNotSpecifiedException : Exception
    {
        public ConnectionStringNotSpecifiedException(string message) : base(message){ }
    }
}
