using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// Interface for the database manager.
    /// </summary>
    public interface IDbManager
    {
        /// <summary>
        /// Analyzes a question and gets the result from the database.
        /// </summary>
        /// <param name="question">The question you want to ask the database.</param>
        /// <param name="user">User who asked the question.</param>
        /// <returns>The result in form of a JSON string.</returns>
        string Ask(string question, string user);

        /// <summary>
        /// Analyzes a statement and puts it into the database.
        /// </summary>
        /// <param name="statement">Sentence(s) you want to put into the database.</param>
        /// <param name="user">User who stated the sentence(s).</param>
        /// <returns>Whether the opperation was succesful or not</returns>
        bool State(string statement, string user);
    }
}
