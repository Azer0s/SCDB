using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SCDB_Server
{
    public sealed class Cache
    {
        private static readonly Lazy<Cache> lazy =
            new Lazy<Cache>(() => new Cache());

        public ILog Logger { get; set; }
        public string AppConnectionString { get; set; }
        public string DataConnectionString { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public string Motd { get; set; }
        public static Cache Instance { get { return lazy.Value; } }

        private Cache()
        {
        }
    }
}
