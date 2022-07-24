using System.Linq.Expressions;

// ReSharper disable MemberCanBeProtected.Global

// ReSharper disable ConvertIfStatementToReturnStatement

namespace HBD.EfCore.Abstractions.Specification;

/// <summary>
///     The Spec for Entity query
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Spec<T>
{
    public static readonly Spec<T> All = new TrueSpec<T>();

    private Func<T, bool>? _compiledFunc;
    

    /// <summary>
    ///     The specification of including navigation properties of query
    /// </summary>
    /// <returns></returns>
    public virtual IQueryable<T> Includes(IQueryable<T> query) => query;

    /// <summary>
    ///     Check whether the entity satisfy this spec or not. a Condition of Spec should be fully
    ///     defined and not allow to changes after Initialized.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool IsSatisfied(T entity)
    {
        _compiledFunc ??= ToExpression().Compile();
        return _compiledFunc.Invoke(entity);
    }

    /// <summary>
    ///     Provides condition expression for <see cref="IQueryable{T}" />
    /// </summary>
    /// <returns></returns>
    public abstract Expression<Func<T, bool>> ToExpression();
}