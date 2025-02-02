namespace SpacexServer.Api.Common
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Dispatches commands to their respective handlers using dependency injection.
    /// </summary>
    public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;


        /// <summary>
        /// Executes a command by resolving its corresponding handler.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>The result of executing the command.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no handler is registered for the command.</exception>
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