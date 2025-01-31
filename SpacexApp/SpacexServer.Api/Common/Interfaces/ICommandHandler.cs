namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> where TResult : ResultCommonLogic
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }

}