using Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Exceptions;
using log4net;
using Linguistics;

namespace Database
{
    public class DbManager : IDbManager
    {
        private readonly ILog _logger;
        private readonly string _insert;

        public DbManager()
        {
            _logger = Cache.Instance.Logger;
            _insert = Cache.Instance.Insert;

            var connection = new SqlConnection(Cache.Instance.AppConnectionString);

            _logger.Info("Trying to connect to the database 'scdb_app'...");
            try
            {
                connection.Open();
                connection.Close();
                _logger.Info("Connection to 'scdb_app' succesful!");
            }
            catch (Exception e)
            {
                _logger.Fatal("Can not open connection to SQL Server!", new DatabaseConnectException(e.Message));
                _logger.Warn("Exiting programm!");
                Console.ReadLine();
                Environment.Exit(0);
            }

            connection = new SqlConnection(Cache.Instance.DataConnectionString);
            _logger.Info("Trying to connect to the database 'scdb_data'...");
            try
            {
                connection.Open();
                connection.Close();
                _logger.Info("Connection to 'scdb_data' succesful!");
            }
            catch (Exception e)
            {
                _logger.Fatal("Can not open connection to SQL Server!", new DatabaseConnectException(e.Message));
                _logger.Warn("Exiting programm!");
                Console.ReadLine();
                Environment.Exit(0);
            }

            LoadExceptions();
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
            _logger.Info("User sent a question");
            return "";
        }

        public bool StateOnServer(string statement)
        {
            _logger.Info("User stated information");

            var statementList = statement.Split('.');
            var statements = new List<SPO>();

            foreach (var v in statementList)
            {
                if (v != "" && v != " ")
                {
                    statements.Add(Structurizer.GetStructure(v));
                }
            }

            foreach (var variable in statements)
            {
                var connection = new SqlConnection(Cache.Instance.DataConnectionString);
                var command = new SqlCommand(_insert,connection);

                command.Parameters.AddWithValue("@Subject_Name", variable.Subject);
                command.Parameters.AddWithValue("@Verb_Name", variable.Predicate);
                command.Parameters.AddWithValue("@Object_Name", variable.Object);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    _logger.Error("Something went wrong while inserting data!",new DatabaseException(e.Message));
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }

        public void LoadExceptions()
        {
            _logger.Info("Loading verb exeptions...");
            var connection = new SqlConnection(Cache.Instance.AppConnectionString);   
            connection.Open();

            Cache.Instance.VerbExceptions = new List<string>();

            var command = new SqlCommand("SELECT * FROM Exceptions", connection);

            SqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception)
            {
                _logger.Error("Something went wrong while loading the exceptions!");
                return;
            }

            if (reader != null)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Cache.Instance.VerbExceptions.Add(reader.GetString(0));
                    }
                    _logger.Info("Loaded exceptions succesfully!");
                }
                else
                {
                    _logger.Error("No rows found.");
                }
            }
        }
    }
}
