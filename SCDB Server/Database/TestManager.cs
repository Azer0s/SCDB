using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using log4net;

namespace Database
{
    /// <summary>
    /// Class which is used for testing. Returns fixed values.
    /// </summary>
    public class TestManager : IDbManager
    {
        /// <summary>
        /// Connection string is never used. Required by the interface.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Logger to show all entered statements and questions.
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// Constructor. Gets the logger from the cache.
        /// </summary>
        public TestManager()
        {
            _logger = Cache.Instance.Logger;
        }

        /// <summary>
        /// Returns a fixed JSON string.
        /// </summary>
        /// <param name="question">Never used in processing. Is printed out by the logger.</param>
        /// <param name="user">Never used.</param>
        /// <returns></returns>
        public string Ask(string question, string user)
        {
            _logger.Info(question);
            return "['Anna','Felix','Bob','Anthony']";
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <param name="statement">Never used in processing. Is printed out by the logger.</param>
        /// <param name="user">Never used.</param>
        /// <returns></returns>
        public bool State(string statement, string user)
        {
            _logger.Info(statement);
            return true;
        }
    }
}
