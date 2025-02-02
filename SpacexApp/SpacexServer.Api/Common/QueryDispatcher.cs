namespace SpacexServer.Api.Common
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Dispatches queries to their respective handlers using dependency injection.
    /// </summary>
    public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;


        /// <summary>
        /// Executes a query by resolving its corresponding handler.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of executing the query.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no handler is registered for the query.</exception>
        public async Task<Result<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType)!;

            return handler == null
                ? throw new InvalidOperationException($"No handler registered for {query.GetType().Name}")
                : (Result<TResult>)await handler.ExecuteAsync((dynamic)query);
        }
    }
}