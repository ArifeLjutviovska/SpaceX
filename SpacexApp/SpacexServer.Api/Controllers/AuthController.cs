namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Commands.RefreshTokens;
    using SpacexServer.Api.Commands.Users;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Api.Contracts.Users.Responses;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Net;
    using System.Security.Claims;

    /// <summary>
    /// Controller for handling authentication and user account management.
    /// Provides endpoints for user registration, login, password updates, token refresh, and password recovery.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ICommandDispatcher commandDispatcher) : ExtendedApiController
    {
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The user registration details.</param>
        /// <returns>A response indicating success or failure of the registration process.</returns>
        [HttpPost("signup")]
        [SwaggerOperation(Summary = "Registers a new user.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "User registered successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "The email is already registered.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request data.")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpUserRequest request)
        {
            return OkOrError(await _commandDispatcher.ExecuteAsync(new SignUpUserCommand(request)));
        }

        /// <summary>
        /// Logs in a user and returns JWT access & refresh tokens.
        /// </summary>
        /// <param name="request">User login credentials.</param>
        /// <returns>JWT access and refresh tokens if authentication is successful.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logs in a user and returns JWT tokens.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Login successful.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Invalid email or password.")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest request)
        {
            return OkOrError(await _commandDispatcher.ExecuteAsync(new LoginUserCommand(request)));
        }

        /// <summary>
        /// Refreshes an expired JWT access token using a valid refresh token.
        /// </summary>
        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Refreshes an expired JWT access token.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Token refreshed successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Invalid or expired refresh token.")]
        public async Task<IActionResult> RefreshToken()
        {
            return OkOrError(await _commandDispatcher.ExecuteAsync(new RefreshTokenCommand()));
        }

        /// <summary>
        /// Updates the user's password. Requires authentication.
        /// </summary>
        /// <param name="request">Request containing the current and new password.</param>
        /// <returns>A response indicating success or failure of the password update.</returns>
        [HttpPut("update-password")]
        [Authorize]
        [SwaggerOperation(Summary = "Updates the user's password.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password updated successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            request.Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            return OkOrError(await _commandDispatcher.ExecuteAsync(new UpdatePasswordCommand(request)));
        }

        /// <summary>
        /// Verifies if the provided email exists in the system for password reset.
        /// </summary>
        /// <param name="request">The request containing the user's email.</param>
        /// <returns>A response indicating whether the email exists and if the user can proceed with password reset.</returns>
        [HttpPut("forgot-password")]
        [SwaggerOperation(Summary = "Validates the email for password reset.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Email verified successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            return OkOrError(await _commandDispatcher.ExecuteAsync(new ForgotPasswordCommand(request)));
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="request">Request containing the new password and user email verification.</param>
        /// <returns>A response indicating whether the password reset was successful.</returns>
        [HttpPut("reset-password")]
        [SwaggerOperation(Summary = "Resets the user's password.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Password updated successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request.")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            return OkOrError(await _commandDispatcher.ExecuteAsync(new ResetPasswordCommand(request)));
        }

        /// <summary>
        /// Retrieves the currently logged-in user.
        /// </summary>
        [HttpGet("current-user")]
        [Authorize]
        [SwaggerOperation(Summary = "Retrieves the currently logged-in user.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "User data retrieved successfully.", typeof(Result<CurrentUserResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        public IActionResult GetCurrentUser()
        {
            CurrentUserResponse user = new()
            {
                FirstName = User.FindFirst("firstName")?.Value ?? "",
                LastName = User.FindFirst("lastName")?.Value ?? "",
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? ""
            };

            return OkOrError(Result.Ok(user));
        }

        /// <summary>
        /// Verifies if the user's session is still valid.
        /// </summary>
        [HttpGet("verify-session")]
        [Authorize]
        [SwaggerOperation(Summary = "Verifies if the user's session is still valid.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Session is valid.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Session expired.")]
        public IActionResult VerifySession()
        {
            if (string.IsNullOrEmpty(Request.Cookies["accessToken"]))
            {
                return OkOrError(Result.Failed("Session expired."));
            }

            return OkOrError(Result.Ok("Session is valid."));
        }

        /// <summary>
        /// Logs out a user by clearing JWT cookies.
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        [SwaggerOperation(Summary = "Logs out a user.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "User logged out successfully.")]
        public IActionResult Logout()
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            Response.Cookies.Append("accessToken", "", cookieOptions);
            Response.Cookies.Append("refreshToken", "", cookieOptions);

            return OkOrError(Result.Ok("Logged out successfully."));
        }
    }
}