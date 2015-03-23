using System;
using System.Linq.Expressions;

namespace Transit.Core.ExtensionMethods
{
    public static class PropertyChanged
    {

        #region public

        public static string GetPropertySymbol<TObject, TExpression>(this TObject tObject, Expression<Func<TObject, TExpression>> expression)
        {

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return ((MemberExpression)expression.Body).Member.Name;
        }

        #endregion

    }
}
