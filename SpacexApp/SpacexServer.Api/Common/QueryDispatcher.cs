namespace SpacexServer.Api.Common
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;

    public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

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