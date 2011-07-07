using System;
using System.Linq;
using Linq2Wolfram;

namespace SampleUsage
{

    class Program
    {
        static void Main(string[] args)
        {
            string appid = "1234";
            var query = new WolframAlphaContext(appid)
                .Knowledge
                .Where(k => k.input == "population of florida" && k.appid == "1234");

            foreach (var r in query)
            {
                Console.WriteLine(r.url);
            }
        }
    }
}
