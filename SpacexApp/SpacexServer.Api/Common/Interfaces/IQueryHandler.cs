namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Defines a query handler that processes queries and returns a result.
    /// </summary>
    /// <typeparam name="TQuery">The type of query handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the handler.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of executing the query.</returns>
        Task<Result<TResult>> ExecuteAsync(TQuery query);
    }
}