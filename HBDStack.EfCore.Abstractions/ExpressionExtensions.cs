using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

public static class ExpressionExtensions
{
    #region Methods

    /// <summary>
    /// Combine two expressions with And
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var and = Expression.AndAlso(left.Body, Expression.Invoke(right, left.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(and, left.Parameters);
    }

    public static IEnumerable<PropertyInfo?> ExtractProperties<T, TKey>(this Expression<Func<T, TKey>> @this)
        where T : class
    {
        if (@this == null) yield break;

        var queue = new Queue<Expression>();
        queue.Enqueue(@this.Body);

        while (queue.Count > 0)
        {
            var ex = queue.Dequeue();

            switch (ex)
            {
                case MemberExpression expression:
                {
                    dynamic tmp = expression;
                    yield return tmp.Member;
                    break;
                }
                case UnaryExpression expression:
                {
                    dynamic? tmp = expression.Operand as MemberExpression;
                    yield return tmp?.Member;
                    break;
                }
                case BinaryExpression expression:
                {
                    var tmp = expression;
                    queue.Enqueue(tmp.Left);
                    queue.Enqueue(tmp.Right);
                    break;
                }
                case MethodCallExpression expression:
                {
                    dynamic tmp = expression;
                    yield return tmp.Object.Member;
                    break;
                }
            }
        }
    }

    public static PropertyInfo? ExtractProperty<T, TKey>(this Expression<Func<T, TKey>> @this)
        where T : class =>
        @this.ExtractProperties().SingleOrDefault();

    /// <summary>
    /// Get Not of the expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> NotMe<T>(this Expression<Func<T, bool>> @this)
    {
        var not = Expression.Not(@this.Body);
        return Expression.Lambda<Func<T, bool>>(not, @this.Parameters);
    }

    /// <summary>
    /// Combine two expressions with Or
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right) =>
        Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, right.Body), left.Parameters.Single());

    #endregion Methods
}