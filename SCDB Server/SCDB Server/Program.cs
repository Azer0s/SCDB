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
    class Program
    {
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
                    logger.Fatal("Couldnt start Nancy-Server", e);
                }
                Console.ReadKey();
            }
        }

        public static void Main()
        {
            Console.Clear();
            Cache.Instance.Logger = LogManager.GetLogger(typeof(Program));
            ILog logger = Cache.Instance.Logger;

            logger.Info("Loading objects into cache...");
            bool someErrors = false;

            try
            {
                Cache.Instance.Motd = System.Configuration.ConfigurationManager.AppSettings["motd"];
                if (Cache.Instance.Motd == null)
                {
                    logger.Warn("No motd specified, using default!", new NoMotdSpecifiedException(""));
                    someErrors = true;
                    Cache.Instance.Motd = "SCDB Server connected!";
                }
            }
            catch (Exception)
            {
                logger.Warn("No motd specified, using default!", new NoMotdSpecifiedException(""));
                someErrors = true;
                Cache.Instance.Motd = "SCDB Server connected!";
            }

            try
            {
                Cache.Instance.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"]);
                if (Cache.Instance.Port == 0)
                {
                    logger.Warn("No port specified, listening on port 8066!", new PortNotSpecifiedException(""));
                    someErrors = true;
                    Cache.Instance.Port = 8066;
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
                    logger.Warn("No address specified, listening on http://localhost", new AddressNotSpecifiedException(""));
                    someErrors = true;
                    Cache.Instance.Address = "http://localhost";
                }
            }
            catch (Exception)
            {
                logger.Warn("No address specified, listening on http://localhost",new AddressNotSpecifiedException(""));
                someErrors = true;
                Cache.Instance.Address = "http://localhost";
            }

            if (someErrors)
            {
                logger.Warn("Some errors ocurred while loading the configuration, using defaults!");
            }
            else
            {
                logger.Info("Loaded objects into cache succesfuly");
            }

            var p = new Program();
            p.Start();
        }
    }
}
