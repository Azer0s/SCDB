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
    public class TestManager : IDbManager
    {
        public string ConnectionString { get; set; }
        private readonly ILog _logger;

        public TestManager()
        {
            _logger = Cache.Instance.Logger;
        }
        public string Ask(string question, string user)
        {
            _logger.Info(question);
            return "['Anna','Felix','Bob','Anthony']";
        }

        public bool State(string statement, string user)
        {
            _logger.Info(statement);
            return true;
        }
    }
}
