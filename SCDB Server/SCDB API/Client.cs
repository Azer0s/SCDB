using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCDB_API
{
    public class Client : IScdbClient
    {
        public string Connection { get; set; }

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

                return true;

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
                return new List<string>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
