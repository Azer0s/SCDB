using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using log4net;
using Nancy;

namespace SCDB_Server
{
    public class ScdbModule : NancyModule
    {
        private readonly IDbManager _db;
        private readonly ILog _logger;

        public ScdbModule(IDbManager db)
        {
            _logger = LogManager.GetLogger(typeof(Program));
            _db = db;

            Post["/state"] = _ =>
            {
                return "";
            };

            Post["/ask"] = _ =>
            {
                return "";
            };

            Get["/connect"] = _ => System.Configuration.ConfigurationManager.AppSettings["motd"];
        }
    }
}
