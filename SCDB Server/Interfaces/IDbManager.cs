using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDbManager
    {
        string ConnectionString { get; set; }
        Task<string> Ask(string question);
        Task<bool> State(string statement);
    }
}
