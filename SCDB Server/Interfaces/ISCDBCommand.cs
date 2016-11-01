namespace Interfaces
{
    /// <summary>
    /// Interface for the ScdbCommand.
    /// </summary>
    public interface IScdbCommand
    {
        /// <summary>
        /// The statement/question with placeholders.
        /// </summary>
        string QueryCommand { get; }
        
        /// <summary>
        /// The statement/question with parameters.
        /// </summary>
        string QueryCommandWithParams { get; }

        /// <summary>
        /// Replaces a placeholder in a command with a value.
        /// </summary>
        /// <param name="placeholder">The placeholder you are addressing.</param>
        /// <param name="value">The value you want to enter.</param>
        void ReplaceWithParameter(string placeholder, string value);
        string ToString();
    }
}
