using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exceptions;
using log4net;
using Nancy.Hosting.Self;

namespace SCDB_Server
{
    /// <summary>
    /// Entry class for the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Starts the Nancy server and initiates all components required by it (including the bootsptrapper and the database manager).
        /// </summary>
        private void Start()
        {
            var port = Cache.Instance.Port;
            var url = Cache.Instance.Address;
            ILog logger = Cache.Instance.Logger;
            Console.Title = "SCDB Server Console";
            var uri = new Uri($"{url}:{port}/");
            using (var nancy = new NancyHost(uri, new ScdbBootstrapper()))
            {
                try
                {
                    nancy.Start();
                    logger.Info($"Started listennig port on {port}");
                    logger.Info($"Started listening address on {url}");
                }
                catch (Exception e)
                {
                    logger.Fatal("Couldnt start Nancy-Server! Exiting programm!", new ServerStartException(e.Message));
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Main method. Loads the configuration into the cache, initializes the logger, calls the start method.
        /// </summary>
        public static void Main()
        {
            Console.Clear();
            Cache.Instance.Logger = LogManager.GetLogger(typeof(Program));
            ILog logger = Cache.Instance.Logger;

            logger.Info("Loading objects into cache...");
            bool someErrors = false;

            try
            {
                Cache.Instance.LogLevel =
                    Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["loglevel"]);
                if (Cache.Instance.LogLevel == 0)
                {
                    throw new LogLevelNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("No log level specified, using default!", new LogLevelNotSpecifiedException(""));
                someErrors = true;
                Cache.Instance.LogLevel = 2;
            }
            try
            {
                Cache.Instance.Insert = System.Configuration.ConfigurationManager.AppSettings["insert"];
                if (Cache.Instance.Insert == null)
                {
                    throw new StatementNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("Insert statement not specified, using default!",
                    new StatementNotSpecifiedException("Insert statement not specified!"));
                someErrors = true;
                Cache.Instance.Insert =
                    "DECLARE @Subject_Id AS uniqueidentifier=NEWID();" +
                    "DECLARE @Verb_Id AS uniqueidentifier=NEWID();" +
                    "DECLARE @Object_Id AS uniqueidentifier=NEWID();" +
                    "INSERT INTO Subject (Id, Name) VALUES (@Subject_Id, @Subject); " +
                    "INSERT INTO Verb (Id, Name) VALUES (@Verb_Id,@Verb); " +
                    "INSERT INTO Object (Id, Name) VALUES (@Object_Id, @Object);";
            }
            try
            {
                Cache.Instance.Select = System.Configuration.ConfigurationManager.AppSettings["select"];
                if (Cache.Instance.Select == null)
                {
                    throw new StatementNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("Select statement not specified, using default!",
                    new StatementNotSpecifiedException("Select statement not specified!"));
                someErrors = true;
                Cache.Instance.Select = "" /*TODO Add default select statement*/;
            }
            try
            {
                Cache.Instance.Motd = System.Configuration.ConfigurationManager.AppSettings["motd"];
                if (Cache.Instance.Motd == null)
                {
                    throw new MotdNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("No motd specified, using default!", new MotdNotSpecifiedException(""));
                someErrors = true;
                Cache.Instance.Motd = "SCDB Server connected!";
            }

            try
            {
                Cache.Instance.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"]);
                if (Cache.Instance.Port == 0)
                {
                    throw new PortNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("No port specified, listening on port 8066!",new PortNotSpecifiedException(""));
                someErrors = true;
                Cache.Instance.Port = 8066;
            }

            try
            {
                Cache.Instance.Address = System.Configuration.ConfigurationManager.AppSettings["address"];
                if (Cache.Instance.Address == null)
                {
                    throw new AddressNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Warn("No address specified, listening on http://localhost",new AddressNotSpecifiedException(""));
                someErrors = true;
                Cache.Instance.Address = "http://localhost";
            }

            try
            {
                Cache.Instance.AppConnectionString =
                    System.Configuration.ConfigurationManager.ConnectionStrings["app"].ConnectionString;
                Cache.Instance.DataConnectionString =
                    System.Configuration.ConfigurationManager.ConnectionStrings["data"].ConnectionString;
                if (Cache.Instance.AppConnectionString == null && Cache.Instance.AppConnectionString == "")
                {
                    throw new ConnectionStringNotSpecifiedException("");
                }
                if (Cache.Instance.DataConnectionString == null && Cache.Instance.DataConnectionString == "")
                {
                    throw new ConnectionStringNotSpecifiedException("");
                }
            }
            catch (Exception)
            {
                logger.Fatal("Could not load connectionStrings!", new ConnectionStringNotSpecifiedException(""));
                logger.Warn("Exiting programm!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (someErrors)
            {
                logger.Warn("Some errors ocurred while loading the configuration, using defaults!");
            }
            else
            {
                logger.Info("Loaded objects into cache succesfuly!");
            }

            var p = new Program();
            p.Start();
        }
    }
}
