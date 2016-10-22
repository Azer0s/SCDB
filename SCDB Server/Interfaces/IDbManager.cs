using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDbManager
    {
        Task<string> Ask(string question);
        Task<bool> State(string statement);
    }
}
