using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDB_API
{
    interface IScdbClient
    {
        string Connection { get; set; }
        bool State(string statement);
        List<string> Ask(string question);
        bool State(IScdbCommand statement);
        List<string> Ask(IScdbCommand question);
    }
}
