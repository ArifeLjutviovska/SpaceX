namespace SpacexServer.Api.Contracts.Users.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}