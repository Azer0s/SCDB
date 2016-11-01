using System.Collections.Generic;

namespace Interfaces
{
    /// <summary>
    /// Interface for the Client connection.
    /// </summary>
    public interface IScdbClient
    {
        /// <summary>
        /// Shows if the client is connected to the server.
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Connection string for the server.
        /// </summary>
        string Connection { get; set; }
        /// <summary>
        /// Tries to connect to the SCDB server. Has to be called before a statement or a question is sent.
        /// </summary>
        /// <returns>Whether the connection was succesful.</returns>
        bool Connect();
        /// <summary>
        /// Sends a statement to the SCDB server.
        /// </summary>
        /// <param name="statement">The statement you want to send.</param>
        /// <returns>Whether the statement was sent/processed succesful.</returns>
        bool State(string statement);
        /// <summary>
        /// Sends a question to the SCDB server.
        /// </summary>
        /// <param name="question">The question you want to send.</param>
        /// <returns>The result of the question.</returns>
        List<string> Ask(string question);
        /// <summary>
        /// Sends a statement to the SCDB server.
        /// </summary>
        /// <param name="statement">The statement you want to send.</param>
        /// <returns>Whether the statement was sent/processed succesful.</returns>
        bool State(IScdbCommand statement);
        /// <summary>
        /// Sends a question to the SCDB server.
        /// </summary>
        /// <param name="question">The question you want to send.</param>
        /// <returns>The result of the question.</returns>
        List<string> Ask(IScdbCommand question);
    }
}
