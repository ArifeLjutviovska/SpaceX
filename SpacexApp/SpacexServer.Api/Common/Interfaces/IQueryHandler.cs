namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<Result<TResult>> ExecuteAsync(TQuery query);
    }
}