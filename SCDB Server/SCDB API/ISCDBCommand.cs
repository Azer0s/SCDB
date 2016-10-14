using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDB_API
{
    public interface IScdbCommand
    {
        string QueryCommand { get; }
        string QueryCommandWithParams { get; }
        void ReplaceWithParameter(string placeholder, string value);
        string ToString();
    }
}
