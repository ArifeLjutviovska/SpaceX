namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Defines a command handler that processes commands and returns a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of command handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the handler.</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> where TResult : ResultCommonLogic
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The result of executing the command.</returns>
        Task<TResult> ExecuteAsync(TCommand command);
    }

}