namespace SpacexServer.Common
{
    using SpacexServer.Common.Interfaces;
    using SpacexServer.Common.Models;

    public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command) where TResult : ResultCommonLogic
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType)!;

            return handler == null
                ? throw new InvalidOperationException($"No handler registered for {command.GetType().Name}")
                : (TResult)await handler.ExecuteAsync((dynamic)command);
        }
    }
}