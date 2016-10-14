namespace Interfaces
{
    public interface IScdbCommand
    {
        string QueryCommand { get; }
        string QueryCommandWithParams { get; }
        void ReplaceWithParameter(string placeholder, string value);
        string ToString();
    }
}
