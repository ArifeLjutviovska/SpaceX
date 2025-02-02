namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Commands.RefreshTokens;
    using SpacexServer.Api.Commands.Users;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.RefreshTokens.Requests;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Api.Contracts.Users.Responses;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Net;
    using System.Security.Claims;

    [ApiController]
    [Route("api/auth")]
    public class AuthController(ICommandDispatcher commandDispatcher) : ExtendedApiController
    {
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("signup")]
        [SwaggerOperation(Summary = "Registers a new user.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "User registered successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "The email is already registered.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request data.")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpUserRequest request)
        {
            Result result = await _commandDispatcher.ExecuteAsync(new SignUpUserCommand(request));
            return OkOrError(result);
        }

        /// <summary>
        /// Logs in a user and returns JWT access & refresh tokens.
        /// </summary>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logs in a user and returns JWT tokens.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Login successful.", typeof(Result<LoginUserResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Invalid email or password.")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest request)
        {
            Result<LoginUserResponse> result = await _commandDispatcher.ExecuteAsync(new LoginUserCommand(request));
            return OkOrError(result);
        }

        /// <summary>
        /// Refreshes an expired JWT access token.
        /// </summary>
        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Refreshes an expired JWT access token.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Token refreshed successfully.", typeof(Result<LoginUserResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Invalid or expired refresh token.")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            Result<LoginUserResponse> result = await _commandDispatcher.ExecuteAsync(new RefreshTokenCommand(request));
            return OkOrError(result);
        }

        /// <summary>
        /// Updates the user's password. Requires authentication.
        /// </summary>
        [HttpPut("update-password")]
        [Authorize]
        [SwaggerOperation(Summary = "Updates the user's password.", Description = "Requires current password verification.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password updated successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(Result.Failed("Unauthorized request."));
            }

            request.Email = email;
            var result = await _commandDispatcher.ExecuteAsync(new UpdatePasswordCommand(request));

            return OkOrError(result);
        }

        /// <summary>
        /// Verifies the email of the user that forgotted the password. Just sends request as Ok or Failed.
        /// </summary>
        [HttpPut("forgot-password")]
        [SwaggerOperation(Summary = "Validates the email.", Description = "Requires email verification.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Email verified successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            var result = await _commandDispatcher.ExecuteAsync(new ForgotPasswordCommand(request));

            return OkOrError(result);
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        [HttpPut("reset-password")]
        [SwaggerOperation(Summary = "Resets the user's password.", Description = "Requires email verification.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password updated successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var result = await _commandDispatcher.ExecuteAsync(new ResetPasswordCommand(request));

            return OkOrError(result);
        }
    }
}