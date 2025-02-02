namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Dispatches queries to the appropriate query handlers.
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Executes a given query and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the executed query.</returns>
        Task<Result<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}