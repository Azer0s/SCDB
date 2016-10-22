using Interfaces;
using System;
using System.Collections.Generic;
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
        private ILog logger;

        public DbManager()
        {
            logger = Cache.Instance.Logger;

            var connection = new SqlConnection(Cache.Instance.AppConnectionString);

            logger.Info("Trying to connect to the database 'scdb_app'...");
            try
            {
                connection.Open();
                connection.Close();
                logger.Info("Connection to 'scdb_app' succesful!");
            }
            catch (Exception e)
            {
                logger.Fatal("Can not open connection to SQL Server!", new DatabaseConnectException(e.Message));
                logger.Warn("Exiting programm!");
                Console.ReadLine();
                Environment.Exit(0);
            }

            connection = new SqlConnection(Cache.Instance.DataConnectionString);
            logger.Info("Trying to connect to the database 'scdb_data'...");
            try
            {
                connection.Open();
                connection.Close();
                logger.Info("Connection to 'scdb_data' succesful!");
            }
            catch (Exception e)
            {
                logger.Fatal("Can not open connection to SQL Server!", new DatabaseConnectException(e.Message));
                logger.Warn("Exiting programm!");
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
            logger.Info("User sent a question");
            return "";
        }

        public bool StateOnServer(string statement)
        {
            logger.Info("User stated information");

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
                Console.WriteLine(variable.ToString());
                string insertStatement = "DECLARE @Subject_Id AS uniqueidentifier=NEWID();DECLARE @Verb_Id AS uniqueidentifier=NEWID();DECLARE @Object_Id AS uniqueidentifier=NEWID();INSERT INTO Subject (Id, Name) VALUES (@Subject_Id, @Subject_Name); INSERT INTO Verb (Id, Subject, Name) VALUES (@Verb_Id, @Subject_Id, @Verb_Name); INSERT INTO Object (Id, Verb, Name) VALUES (@Object_Id, @Verb_Id, @Object_Name);";

                var connection = new SqlConnection(Cache.Instance.DataConnectionString);
                var command = new SqlCommand(insertStatement,connection);

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
                    logger.Error("Something went wrong while inserting data!");
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
            logger.Info("Loading verb exeptions...");
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
                logger.Error("Something went wrong while loading the exceptions!");
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
                    logger.Info("Loaded exceptions succesfully!");
                }
                else
                {
                    logger.Error("No rows found.");
                }
            }
        }
    }
}
