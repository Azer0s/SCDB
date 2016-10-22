using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linguistics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Structurizer.GetStructure("Felix has a dog");
            Console.WriteLine(result.Subject);
            Console.WriteLine(result.Predicate);
            Console.WriteLine(result.Object);
            Console.ReadKey();
        }
    }
}
