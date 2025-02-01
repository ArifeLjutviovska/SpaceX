namespace SpacexServer.Api.Contracts.Users.Responses
{
    public record LoginUserResponse(string AccessToken, string RefreshToken);
}
