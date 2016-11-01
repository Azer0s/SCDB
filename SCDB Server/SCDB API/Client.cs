using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interfaces;
using Newtonsoft.Json;

namespace SCDB_API
{
    /// <summary>
    /// Client for the SCDB Server.
    /// </summary>
    public class Client : IScdbClient
    {
        private string _connection;

        /// <summary>
        /// Shows if the client is connected to the server.
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        /// <summary>
        /// Connection string for the server.
        /// </summary>
        public string Connection
        {
            get { return _connection; }
            set { _connection = "http://" + value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection">Connection string for the client.</param>
        public Client(string connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Client() { }

        /// <summary>
        /// Tries to connect to the SCDB server. Has to be called before a statement or a question is sent.
        /// </summary>
        /// <returns>Whether the connection was succesful.</returns>
        public bool Connect()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_connection);
                    var result = client.GetAsync("/connect");
                    string resultAsString = result.Result.Content.ReadAsStringAsync().ToString();
                }
                IsConnected = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends a statement to the SCDB server.
        /// </summary>
        /// <param name="statement">The statement you want to send.</param>
        /// <returns>Whether the statement was sent/processed succesful.</returns>
        public bool State(string statement)
        {
            return SendStatement(statement);
        }

        /// <summary>
        /// Sends a question to the SCDB server.
        /// </summary>
        /// <param name="question">The question you want to send.</param>
        /// <returns>The result of the question.</returns>
        public List<string> Ask(string question)
        {
            return SendQuestion(question);
        }

        /// <summary>
        /// Sends a statement to the SCDB server.
        /// </summary>
        /// <param name="statement">The statement you want to send.</param>
        /// <returns>Whether the statement was sent/processed succesful.</returns>
        private bool SendStatement(string statement)
        {
            if (!IsConnected)
            {
                return false;
            }

            bool succes = false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Connection);
                    var content = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("statement", statement)
                        });
                    var result = client.PostAsync("/state", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    succes = Boolean.Parse(resultContent.ToLower());
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return succes;
        }

        /// <summary>
        /// Sends a statement to the SCDB server.
        /// </summary>
        /// <param name="statement">The statement you want to send.</param>
        /// <returns>Whether the statement was sent/processed succesful.</returns>
        public bool State(IScdbCommand statement)
        {
            return SendStatement(statement.ToString());
        }

        /// <summary>
        /// Sends a question to the SCDB server.
        /// </summary>
        /// <param name="question">The question you want to send.</param>
        /// <returns>The result of the question.</returns>
        public List<string> Ask(IScdbCommand question)
        {
            return SendQuestion(question.ToString());
        }

        /// <summary>
        /// Sends a question to the SCDB server.
        /// </summary>
        /// <param name="question">The question you want to send.</param>
        /// <returns>The result of the question.</returns>
        private List<string> SendQuestion(string question)
        {
            if (!IsConnected)
            {
                return null;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Connection);
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("question", question)
                    });
                    var result = client.PostAsync("/ask", content).Result;
                    var resultContent = result.Content.ReadAsStringAsync().Result;

                    return resultContent == "n/a" ? null : JsonConvert.DeserializeObject<List<string>>(resultContent);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
