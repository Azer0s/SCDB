using System.Collections.Generic;

namespace Interfaces
{
    public interface IScdbClient
    {
        bool IsConnected { get; }
        string Connection { get; set; }
        bool Connect();
        bool State(string statement);
        List<string> Ask(string question);
        bool State(IScdbCommand statement);
        List<string> Ask(IScdbCommand question);
    }
}
