using System.Linq;
using Linq2Wolfram.Expressions;

namespace Linq2Wolfram
{
    public class WolframAlphaContext
    {
        private WolframQueryProvider provider = new WolframQueryProvider();

        public WolframAlphaContext(string appid)
        {

        }

        public string AppId { get; private set; }

        public IQueryable<WolframResult> Knowledge
        {
            get
            {
                var query = new Query<WolframResult>(provider);
                return query;
            }
        }
    }
}
