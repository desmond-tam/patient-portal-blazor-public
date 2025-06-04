using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;

namespace PatientPortalApp.Data
{
    public static class QueryableExtension
    {
        public static IQueryable<T> OrderByString<T>(this IQueryable<T> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(T);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        private static Expression CompareEqual(Expression e1,Expression e2)
        {
            var p1 = Expression.Parameter(typeof(string));
            var c = Expression.Constant(true);
            var m1 = Expression.Call(typeof(string).GetMethod("Compare", new Type[] { typeof(string), typeof(string), typeof(bool) }),e1,e2,c);
            return m1;
            //    var methodinfo = typeof(string).GetMethod("Compare",new Type[] { typeof(string), typeof(string), typeof(bool) });
            //    var e3 = Expression.Constant(true);
            //    var compare = Expression.Call(methodinfo, e1, e2,e3);
            //    return Expression.Equal(compare, Expression.Constant(0));
        }

        public static IQueryable<T> WhereString<T,T1>(this IQueryable<T> source, T1 target, string propertyName)
        {
            var type = typeof(T);
            var type1 = typeof(T1);
            var property = type.GetProperty(propertyName);
            var left = Expression.Parameter(type, propertyName);
            var right = Expression.Parameter(type1, propertyName);
            var compare = CompareEqual(left,right);

            var resultExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { type, type1 },
                                          source.Expression,compare);
            return source.Provider.CreateQuery<T>(resultExpression);
        }


    }
}
