using System.Collections.Generic;

namespace Interfaces
{
    public interface IScdbClient
    {
        bool Connect();
        string Connection { get; set; }
        bool State(string statement);
        List<string> Ask(string question);
        bool State(IScdbCommand statement);
        List<string> Ask(IScdbCommand question);
    }
}
