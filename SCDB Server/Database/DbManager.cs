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
using Newtonsoft.Json;

namespace Database
{
    /// <summary>
    /// Class which manages connections to the database and is responsible for processing statements and questions.
    /// </summary>
    public class DbManager : IDbManager
    {
        /// <summary>
        /// The initialized log4net logger.
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// Constructor. Called by the Nancy-Bootstrapper. Tries to connect to the databases, loads the verb exceptions and gets further required data from the cache.
        /// </summary>
        public DbManager()
        {
            _logger = Cache.Instance.Logger;
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

        /// <summary>
        /// Analyzes a question and gets the result from the database.
        /// </summary>
        /// <param name="question">The question you want to ask the database.</param>
        /// <param name="user">User who asked the question.</param>
        /// <returns>The result in form of a JSON string.</returns>
        public string Ask(string question, string user)
        {
            return AskServer(question, user);
        }

        /// <summary>
        /// Analyzes a statement and puts it into the database.
        /// </summary>
        /// <param name="statement">Sentence(s) you want to put into the database.</param>
        /// <param name="user">User who stated the sentence(s).</param>
        /// <returns>Whether the opperation was succesful or not</returns>
        public bool State(string statement, string user)
        {
            return StateOnServer(statement, user);
        }

        /// <summary>
        /// Analyzes a question and gets the result from the database.
        /// </summary>
        /// <param name="question">The question you want to ask the database.</param>
        /// <param name="user">User who asked the question.</param>
        /// <returns>The result in form of a JSON string.</returns>
        private string AskServer(string question, string user)
        {
            if (Cache.Instance.LogLevel > 1)
            {
                _logger.Info($"{user} sent a question");
            }

            if (question.Contains('.'))
            {
                return "n/a";
            }
            if (!question.Contains('?'))
            {
                return "n/a";
            }

            var questionList = question.Split('?');
            var questions = new List<SPO>();

            foreach (var v in questionList)
            {
                if (v != "" && v != " ")
                {
                    try
                    {
                        questions.Add(Structurizer.GetStructure(v,true));
                    }
                    catch (Exception)
                    {
                        if (Cache.Instance.LogLevel > 0)
                        {
                            _logger.Error("Something went wrong while processing data!");
                        }
                        return "n/a";
                    }
                }
            }

            List<string> resultData = new List<string>();

            foreach (var variable in questions)
            {
                if (!variable.IsExpression)
                {
                    var connection = new SqlConnection(Cache.Instance.DataConnectionString);
                    var command = new SqlCommand(Cache.Instance.Select, connection);

                    command.Parameters.AddWithValue("@Verb", variable.Predicate);
                    command.Parameters.AddWithValue("@Object", variable.Object);

                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.HasRows && reader != null)
                        {
                            while (reader.Read())
                            {
                                resultData.Add(reader.GetString(0));
                            }
                        }

                        if (questions.Count > 1 && questions.IndexOf(variable) != questions.Count-1)
                        {
                            resultData.Add("---");
                        }
                    }
                    catch (Exception)
                    {
                        if (Cache.Instance.LogLevel > 0)
                        {
                            _logger.Error("Something went wrong while selecting data!");
                        }
                        return "n/a";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    var connection = new SqlConnection(Cache.Instance.DataConnectionString);
                    var command = new SqlCommand(Cache.Instance.SelectState, connection);

                    command.Parameters.AddWithValue("@Subject", variable.Subject);
                    command.Parameters.AddWithValue("@Verb", variable.Predicate);
                    command.Parameters.AddWithValue("@Object", variable.Object);

                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            resultData.Add("true");
                            if (questions.Count > 1 && questions.IndexOf(variable) != questions.Count - 1)
                            {
                                resultData.Add("---");
                            }
                        }
                        else
                        {
                            resultData.Add("false");
                            if (questions.Count > 1 && questions.IndexOf(variable) != questions.Count - 1)
                            {
                                resultData.Add("---");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (Cache.Instance.LogLevel > 0)
                        {
                            _logger.Error("Something went wrong while inserting data!");
                        }
                        return "n/a";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }   
            }

            if (Cache.Instance.LogLevel > 2)
            {
                _logger.Info("Question succesful!");
            }

            return JsonConvert.SerializeObject(resultData);
        }

        /// <summary>
        /// Analyzes a statement and puts it into the database.
        /// </summary>
        /// <param name="statement">Sentence(s) you want to put into the database.</param>
        /// <param name="user">User who stated the sentence(s).</param>
        /// <returns>Whether the opperation was succesful or not</returns>
        private bool StateOnServer(string statement, string user)
        {
            if (Cache.Instance.LogLevel>1)
            {
                _logger.Info($"{user} stated information");
            }

            if (statement.Contains('?'))
            {
                return false;
            }
            if (!statement.Contains('.'))
            {
                return false;
            }

            var statementList = statement.Split('.');
            var statements = new List<SPO>();

            foreach (var v in statementList)
            {
                if (v != "" && v != " ")
                {
                    try
                    {
                        statements.Add(Structurizer.GetStructure(v,false));
                    }
                    catch (Exception)
                    {
                        if (Cache.Instance.LogLevel>0)
                        {
                            _logger.Error("Something went wrong while processing data!");
                        }
                        return false;
                    }
                }
            }

            foreach (var variable in statements)
            {
                var connection = new SqlConnection(Cache.Instance.DataConnectionString);
                var command = new SqlCommand(Cache.Instance.Insert,connection);

                command.Parameters.AddWithValue("@Subject", variable.Subject);
                command.Parameters.AddWithValue("@Verb", variable.Predicate);
                command.Parameters.AddWithValue("@Object", variable.Object);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    if (Cache.Instance.LogLevel > 0 )
                    {
                        _logger.Error("Something went wrong while inserting data!");
                    }
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (Cache.Instance.LogLevel > 2)
            {
                _logger.Info("Statement succesful!");
            }
            return true;
        }

        /// <summary>
        /// Loads the verb exceptions from the App DB and saves them in the Cache.
        /// </summary>
        private void LoadExceptions()
        {
            _logger.Info("Loading verb exeptions...");
            var connection = new SqlConnection(Cache.Instance.AppConnectionString);   
            connection.Open();

            Cache.Instance.VerbExceptions = new List<string>();

            var command = new SqlCommand("SELECT * FROM Exceptions", connection);

            SqlDataReader reader;
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
                    _logger.Warn("No rows found.");
                }
            }
        }
    }
}
