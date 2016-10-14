using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDB_API
{
    class Command : IScdbCommand
    {
        public string QueryCommand => PlaceholderDefaultCommand;

        private string PlaceholderDefaultCommand { get; }

        public string QueryCommandWithParams => PlaceHolderCommandWithParams;
        private string PlaceHolderCommandWithParams { get; set; }

        public Command(string command)
        {
            PlaceholderDefaultCommand = command;
            PlaceHolderCommandWithParams = command;
        }
        public void ReplaceWithParameter(string placeholder, string value)
        {
            PlaceHolderCommandWithParams = PlaceHolderCommandWithParams.Replace("@" + placeholder, value);
        }

        public override string ToString()
        {
            return PlaceHolderCommandWithParams;
        }
    }
}
