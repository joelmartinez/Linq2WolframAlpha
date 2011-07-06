using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .Where(k => k.input == "population of florida");

            foreach (var r in query) 
            {
                Console.WriteLine(r.url);
            }
                    }
    }
}
