namespace SpacexServer.Api.Common.Enums
{
    public enum ResultType : short
    {
        InternalError,
        Ok,
        NotFound,
        Forbidden,
        Conflicted,
        Invalid,
        Unauthorized
    }
}