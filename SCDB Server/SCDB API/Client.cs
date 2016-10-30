﻿using System;
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
    public class Client : IScdbClient
    {
        private string _connection;

        public bool IsConnected { get; private set; } = false;

        public string Connection
        {
            get { return _connection; }
            set { _connection = "http://" + value; }
        }

        public Client(string connection)
        {
            Connection = connection;
        }

        public Client() { }

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
