using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SCDB_API
{
    public class Client : IScdbClient
    {
        private string _connection;
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Connection
        {
            get { return _connection; }
            set { _connection = "http://" + value; }
        }

        public bool State(string statement)
        {
            return SendStatement(statement);
        }

        public List<string> Ask(string question)
        {
            return SendQuestion(question);
        }

        private bool SendStatement(string statement)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Connection + "/ask");
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("statement", statement)
                    });
                    var result = client.PostAsync("/state", content).Result;
                    var resultContent = result.Content.ReadAsStringAsync().Result;

                    if (resultContent == "n/a")
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool State(IScdbCommand statement)
        {
            return SendStatement(statement.ToString());
        }

        public List<string> Ask(IScdbCommand question)
        {
            return SendQuestion(question.ToString());
        }

        private List<string> SendQuestion(string question)
        {
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
