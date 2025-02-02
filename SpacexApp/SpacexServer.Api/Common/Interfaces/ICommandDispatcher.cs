namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Dispatches commands to the appropriate command handlers.
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Executes a given command and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>The result of the executed command.</returns>
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command) where TResult : ResultCommonLogic;
    }
}