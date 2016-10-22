using Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Database
{
    public class DbManager : IDbManager
    {
        public string ConnectionString { get; set; }
        private ILog logger;

        public DbManager()
        {
            logger = Cache.Instance.Logger;
            ConnectionString = Cache.Instance.DataConnectionString;

            var connection = new SqlConnection(ConnectionString);

            logger.Info("Trying to connect to the database...");
            try
            {
                connection.Open();
                connection.Close();
                logger.Info("Connection succesful!");
            }
            catch (Exception e)
            {
                logger.Fatal("Can not open connection to SQL Server!", e);
                logger.Warn("Exiting programm!");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        public Task<string> Ask(string question)
        {
            return Task.Run(() => AskServer(question));
        }

        public Task<bool> State(string statement)
        {
            return Task.Run(() => StateOnServer(statement));
        }

        public string AskServer(string question)
        {
            //TODO Implement
            return "";
        }

        public bool StateOnServer(string statement)
        {
            //TODO Implement
            return false;
        }
    }
}
