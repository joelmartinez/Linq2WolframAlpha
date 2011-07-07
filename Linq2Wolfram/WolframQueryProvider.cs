using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Linq2Wolfram.Expressions;

namespace Linq2Wolfram
{
    public class WolframQueryProvider : QueryProvider
    {
        Task<List<string>> task = null;

        public override string GetQueryText(Expression expression)
        {
            return new WolframExpressionVisitor().Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            if (task == null)
            {
                this.task = BeginTask(expression);
            }

            return new WolframResults(this.task);
        }

        public void BeginExecution(Expression ex)
        {
            this.task = BeginTask(ex);
        }

        private Task<List<string>> BeginTask(Expression expression)
        {
            var t = Task<List<string>>.Factory.StartNew(() =>
            {
                string url = this.GetQueryText(expression);

                List<String> s = new List<string>();
                s.Add(url);
                return s;
            });
            return t;
        }
    }
}
