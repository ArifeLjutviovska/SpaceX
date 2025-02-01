namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    public interface IQueryDispatcher
    {
        Task<Result<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}