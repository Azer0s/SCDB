﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Nancy;
using Nancy.TinyIoc;
using Database;
using Interfaces;

namespace SCDB_Server
{
    /// <summary>
    /// Custom bootstrapper for Nancy. Loads the database manager in the IoC Container.
    /// </summary>
    class ScdbBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            ILog logger = Cache.Instance.Logger;
            base.ConfigureApplicationContainer(container);

            //register classes
            container.Register<IDbManager, DbManager>();
            logger.Info("IoCContainer initialized!");
        }
    }
}
