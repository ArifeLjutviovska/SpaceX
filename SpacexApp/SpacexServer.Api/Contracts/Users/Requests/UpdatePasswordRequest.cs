namespace SpacexServer.Api.Contracts.Users.Requests
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}