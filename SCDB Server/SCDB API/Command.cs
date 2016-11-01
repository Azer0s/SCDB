using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace SCDB_API
{
    /// <summary>
    /// Custom command. Can be used instead of a string when sending a statement or question.
    /// </summary>
    public class Command : IScdbCommand
    {
        /// <summary>
        /// The statement/question with placeholders.
        /// </summary>
        public string QueryCommand => PlaceholderDefaultCommand;

        private string PlaceholderDefaultCommand { get; }

        /// <summary>
        /// The statement/question with parameters.
        /// </summary>
        public string QueryCommandWithParams => PlaceHolderCommandWithParams;
        private string PlaceHolderCommandWithParams { get; set; }

        public Command(string command)
        {
            PlaceholderDefaultCommand = command;
            PlaceHolderCommandWithParams = command;
        }

        /// <summary>
        /// Replaces a placeholder in a command with a value.
        /// </summary>
        /// <param name="placeholder">The placeholder you are addressing.</param>
        /// <param name="value">The value you want to enter.</param>
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
