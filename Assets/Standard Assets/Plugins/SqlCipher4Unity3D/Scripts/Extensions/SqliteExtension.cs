using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlCipher4Unity3D.Extensions {

public static class SqliteExtension {

    public static int Delete<T>(this TableQuery<T> tableQuery, Expression<Func<T, bool>> predExpr)
        where T : class, new()
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var type = tableQuery.GetType();
        var method = type.GetMethod("CompileExpr", flags);

        if(predExpr.NodeType == ExpressionType.Lambda) {
            var lambda = (LambdaExpression)predExpr;
            var pred = lambda.Body;
            var args = new List<object>();
            var w = method.Invoke(tableQuery, new object[] { pred, args });
            var compileResultType = w.GetType();
            var prop = compileResultType.GetProperty("CommandText");
            var commandText = prop.GetValue(w, null).ToString();
            var cmdText = $"delete from \"{tableQuery.Table.TableName}\"";
            cmdText += " where " + commandText;
            var command = tableQuery.Connection.CreateCommand(cmdText, args.ToArray());
            var result = command.ExecuteNonQuery();

            return result;
        }

        throw new NotSupportedException("Must be a predicate");
    }

}

}
