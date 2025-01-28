namespace SpacexServer.Common.Interfaces
{
    using SpacexServer.Common.Models;

    public interface ICommandDispatcher
    {
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command) where TResult : ResultCommonLogic;
    }
}