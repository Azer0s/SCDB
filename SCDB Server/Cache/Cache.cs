using System;
using System.Collections.Generic;
using log4net;

/// <summary>
/// Class for storing important program data.
/// </summary>
public sealed class Cache
{
    private static readonly Lazy<Cache> lazy =
        new Lazy<Cache>(() => new Cache());
    /// <summary>
    /// The initialized log4net logger. Always use this logger instead of creating a new one!
    /// </summary>
    public ILog Logger { get; set; }
    /// <summary>
    /// The connection string for the app DB. Used for log and verb exceptions.
    /// </summary>
    public string AppConnectionString { get; set; }
    /// <summary>
    /// The connection string for the data DB. Used for incoming statements and questions.
    /// </summary>
    public string DataConnectionString { get; set; }
    /// <summary>
    /// Port on which the program listens.
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// Address on which the program listens.
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Message of the Day. Used for connection verification.
    /// </summary>
    public string Motd { get; set; }
    /// <summary>
    /// List for storing the verb exceptions which are loaded from the App DB.
    /// </summary>
    public List<string> VerbExceptions { get; set; }
    /// <summary>
    /// Insert query for statements.
    /// </summary>
    public string Insert { get; set; }
    /// <summary>
    /// Select statement for questions.
    /// </summary>
    public string Select { get; set; }
    /// <summary>
    /// Determines which messages are logged.
    /// </summary>
    /// -1...None;
    ///  1...Only errors;
    ///  2...Errors and user connect/statement info;
    ///  3...All messages
    public int LogLevel { get; set; }
    /// <summary>
    /// Acces variable for the cache.
    /// </summary>
    public static Cache Instance { get { return lazy.Value; } }

    private Cache()
    {
    }
}
