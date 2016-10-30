﻿using System;
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
                try
                {
                    return _db.State(Request.Form.statement, Request.UserHostAddress).ToString();
                }
                catch (Exception)
                {
                    return "false";
                }
            };

            Post["/ask"] = _ =>
            {
                try
                {
                    return _db.Ask(Request.Form.question, Request.UserHostAddress);
                }
                catch (Exception)
                {
                    return "n/a";
                }
            };

            Get["/connect"] = _ =>
            {
                _logger.Info($"{Request.UserHostAddress} connected to the database");
                return Cache.Instance.Motd;
            };
        }
    }
}
