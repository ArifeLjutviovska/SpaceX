namespace SpacexServer.Api.Controllers
{
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
    }
}
