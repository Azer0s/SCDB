using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interfaces;
using Newtonsoft.Json;

namespace SCDB_API
{
    public class Client : IScdbClient
    {
        private string _connection;

        public bool IsConnected { get; private set; } = false;

        public string Connection
        {
            get { return _connection; }
            set { _connection = "http://" + value; }
        }

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
            if (!IsConnected)
            {
                return false;
            }
            try
            {
                using (var client = new SCDBWebClient())
                {
                    var url = Connection + "/state";
                    var content = new NameValueCollection()
                    {
                        {"statement", statement }
                    };
                    byte[] responsBytes = client.UploadValues(url, "POST", content);
                    string responsebody = System.Text.Encoding.UTF8.GetString(responsBytes);
                    //TODO Get response
                    return responsebody != "false";
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private class SCDBWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest w = base.GetWebRequest(address);
                w.Timeout = 5*1000;
                return w;
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
