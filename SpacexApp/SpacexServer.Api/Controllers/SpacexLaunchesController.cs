namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Commands.SpacexLaunches;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.SpacexLaunches.Responses;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Net;

    [Authorize] 
    [ApiController]
    [Route("api/spacex")] 
    public class SpacexLaunchesController(IQueryDispatcher queryDispatcher) : ExtendedApiController
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;

        /// <summary>
        /// Retrieves a paginated list of past SpaceX launches.
        /// </summary>
        [HttpGet("past-launches")]
        [SwaggerOperation(Summary = "Gets past SpaceX launches with pagination.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful response.", typeof(Result<PagedResult<SpaceXLaunchDto>>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        public async Task<IActionResult> GetPastLaunchesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _queryDispatcher.ExecuteAsync(new GetPastLaunchesQuery(pageNumber, pageSize));
            return OkOrError(result);
        }
    }
}