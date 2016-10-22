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
        public ILog logger;

        public TestManager()
        {
            logger = Cache.Instance.Logger;
        }
        public Task<string> Ask(string question)
        {
            logger.Info(question);
            return Task.Run(() => "['Anna','Felix','Bob','Anthony']");
        }

        public Task<bool> State(string statement)
        {
            logger.Info(statement);
            return Task.Run(() => true);
        }
    }
}
