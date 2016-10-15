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
        private void Start(int port, string url)
        {
            ILog logger = LogManager.GetLogger(typeof(Program));
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
            Cache.Instance.logger = LogManager.GetLogger(typeof(Program));
            ILog logger = Cache.Instance.logger;
            int port = 80;
            string url = "http://localhost";
            try
            {
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"]);
            }
            catch (Exception)
            {
                logger.Warn("No port specified, listening on port 80!",new PortNotSpecifiedException(""));
            }

            try
            {
                url = System.Configuration.ConfigurationManager.AppSettings["address"];
            }
            catch (Exception)
            {
                logger.Warn("No address specified, listening on http://localhost",new AddressNotSpecifiedException(""));
            }

            var p = new Program();
            p.Start(port,url);
        }
    }
}
