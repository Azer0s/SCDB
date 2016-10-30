using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDbManager
    {
        string Ask(string question, string user);
        bool State(string statement, string user);
    }
}
