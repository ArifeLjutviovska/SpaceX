namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Common.Interfaces;
    using SpacexServer.Common.Models;
    using SpacexServer.Contracts.User.Requests;
    using SpacexServer.Services.User.Commands;

    [ApiController]
    [Route("spacex/api")]
    public class AuthController(ICommandDispatcher commandDispatcher) : ExtendedApiController
    {
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            Result result = await _commandDispatcher.ExecuteAsync(new AddNewUserCommand(request));

            return OkOrError(result);
        }
    }
}