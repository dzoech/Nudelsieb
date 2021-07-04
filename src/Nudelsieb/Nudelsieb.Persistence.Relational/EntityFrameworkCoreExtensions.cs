using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace Nudelsieb.Persistence.Relational
{
    public static class EntityFrameworkCoreExtensions
    {
        /// <summary>
        /// Returns the SQL query that will be executed by the <see cref="IQueryable{TEntity}"/>
        /// object.
        /// Source: https://stackoverflow.com/a/51583047/2549398
        /// </summary>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        public static IQueryable<TEntity> ToSql<TEntity>(
            this IQueryable<TEntity> query, out string sql) where TEntity : class
        {
            sql = query.ToSql();
            return query;
        }

        public static IQueryable<TEntity> ToSql<TEntity>(
            this IQueryable<TEntity> query, ILogger logger) where TEntity : class
        {
            var sql = query.ToSql();
            logger.LogDebug(sql);
            return query;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);

        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}
