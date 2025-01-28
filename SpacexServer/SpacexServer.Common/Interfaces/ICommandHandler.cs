namespace SpacexServer.Common.Interfaces
{
    using SpacexServer.Common.Models;

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> where TResult : ResultCommonLogic
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }

}