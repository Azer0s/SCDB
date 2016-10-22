using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Database
{
    class TestManager : IDbManager
    {
        public string ConnectionString { get; set; }
        public Task<string> Ask(string question)
        {
            throw new NotImplementedException();
        }

        public Task<bool> State(string statement)
        {
            throw new NotImplementedException();
        }
    }
}
