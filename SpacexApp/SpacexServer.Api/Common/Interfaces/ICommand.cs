namespace SpacexServer.Api.Common.Interfaces
{
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Represents a command that returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
    public interface ICommand<TResult> where TResult : ResultCommonLogic
    {
    }
}