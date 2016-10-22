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
            _logger = Cache.Instance.Logger;
            _db = db;

            Post["/state"] = _ =>
            {
                return "";
            };

            Post["/ask",true] = async (x, ct) =>
            {
                return "['Anna','Felix','Bob','Anthony']";
            };

            Get["/connect"] = _ =>
            {
                _logger.Info("User connected to the database");
                return Cache.Instance.Motd;
            };
        }
    }
}
