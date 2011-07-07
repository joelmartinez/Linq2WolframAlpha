using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using L2W = Linq2Wolfram.Expressions;

namespace Linq2Wolfram
{
    internal class WolframExpressionVisitor : L2W.ExpressionVisitor
    {
        StringBuilder sb;

        internal WolframExpressionVisitor()
        {
        }

        internal string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.sb.Append("http://api.wolframalpha.com/");

            this.Visit(expression);

            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.sb.Append("v2/query?");

                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;

            }
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    sb.Append("&");
                    break;
                case ExpressionType.Equal:
                    sb.Append("=");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value != null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.String:
                        sb.Append(Uri.EscapeDataString(c.Value.ToString()));
                        break;
                    default:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                }
            }
            return c;
        }
    }
}
