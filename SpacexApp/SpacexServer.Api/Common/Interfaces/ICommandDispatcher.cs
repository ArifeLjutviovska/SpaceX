namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    public interface ICommandDispatcher
    {
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command) where TResult : ResultCommonLogic;
    }
}