namespace SpacexServer.Common.Interfaces
{
    using SpacexServer.Common.Models;

    public interface ICommand<TResult> where TResult : ResultCommonLogic
    {
    }

}