namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Commands.Users;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.Users.Requests;
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
        /// <param name="request">User signup details.</param>
        /// <returns>Success or failure result.</returns>
        [HttpPost("signup")]
        [SwaggerOperation(Summary = "Registers a new user.", Description = "Creates a new user account if the email is not already registered.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "User registered successfully.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "The email is already registered.", typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request data.")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpUserRequest request)
        {
            Result result = await _commandDispatcher.ExecuteAsync(new SignUpUserCommand(request));
            return OkOrError(result);
        }
    }
}