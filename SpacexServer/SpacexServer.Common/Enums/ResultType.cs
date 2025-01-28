namespace SpacexServer.Common.Enums
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