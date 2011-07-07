using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linq2Wolfram
{
    public class WolframResults : IEnumerable<WolframResult>, IEnumerable
    {
        public Task<List<string>> results;

        public WolframResults(Task<List<string>> r)
        {
            this.results = r;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<WolframResult> GetEnumerator()
        {
            return new WolframEnumerator(this.results);
        }

        private class WolframEnumerator : IEnumerator<WolframResult>
        {
            private Task<List<string>> results;

            public WolframEnumerator(Task<List<string>> s)
            {
                this.results = s;
            }

            #region IEnumerator<WolframResult> Members

            public WolframResult Current
            {
                get
                {
                    return new WolframResult
                    {
                        url = results.Result.First()
                    };
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            private int i = 0;

            public bool MoveNext()
            {
                return i++ <= 0;
                //throw new NotImplementedException();
            }

            public void Reset()
            {
            }

            #endregion
        }

    }
}
