namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    public interface ICommand<TResult> where TResult : ResultCommonLogic
    {
    }
}