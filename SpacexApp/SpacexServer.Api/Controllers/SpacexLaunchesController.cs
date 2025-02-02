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


    /// <summary>
    /// Controller for retrieving SpaceX launch data.
    /// Provides endpoints for past, latest, and upcoming launches with pagination.
    /// Requires authentication.
    /// </summary>
    [Authorize] 
    [ApiController]
    [Route("api/spacex")] 
    public class SpacexLaunchesController(IQueryDispatcher queryDispatcher) : ExtendedApiController
    {
        private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;

        /// <summary>
        /// Retrieves a paginated list of past SpaceX launches.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Default is 1.</param>
        /// <param name="pageSize">The number of launches per page. Default is 20.</param>
        /// <returns>A paginated list of past SpaceX launches.</returns>
        [HttpGet("past-launches")]
        [SwaggerOperation(Summary = "Gets past SpaceX launches with pagination.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful response.", typeof(Result<PagedResult<SpaceXLaunchDto>>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        public async Task<IActionResult> GetPastLaunchesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _queryDispatcher.ExecuteAsync(new GetSpacexLaunchesQuery(pageNumber: pageNumber, 
                                                                                        pageSize: pageSize,
                                                                                        getPastLaunches: true,
                                                                                        getLatestLaunches: false,
                                                                                        getUpcomingLaunches: false));
            return OkOrError(result);
        }

        /// <summary>
        /// Retrieves a paginated list of the latest SpaceX launches.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Default is 1.</param>
        /// <param name="pageSize">The number of launches per page. Default is 20.</param>
        /// <returns>A paginated list of the latest SpaceX launches.</returns>
        [HttpGet("latest-launches")]
        [SwaggerOperation(Summary = "Gets latest SpaceX launches with pagination.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful response.", typeof(Result<PagedResult<SpaceXLaunchDto>>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        public async Task<IActionResult> GetLatestLaunchesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _queryDispatcher.ExecuteAsync(new GetSpacexLaunchesQuery(pageNumber: pageNumber,
                                                                                        pageSize: pageSize,
                                                                                        getPastLaunches: false,
                                                                                        getLatestLaunches: true,
                                                                                        getUpcomingLaunches: false));
            return OkOrError(result);
        }

        /// <summary>
        /// Retrieves a paginated list of upcoming SpaceX launches.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Default is 1.</param>
        /// <param name="pageSize">The number of launches per page. Default is 20.</param>
        /// <returns>A paginated list of upcoming SpaceX launches.</returns>
        [HttpGet("upcoming-launches")]
        [SwaggerOperation(Summary = "Gets upcoming SpaceX launches with pagination.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful response.", typeof(Result<PagedResult<SpaceXLaunchDto>>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized request.")]
        public async Task<IActionResult> GetUpcomingLaunchesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _queryDispatcher.ExecuteAsync(new GetSpacexLaunchesQuery(pageNumber: pageNumber,
                                                                                        pageSize: pageSize,
                                                                                        getPastLaunches: false,
                                                                                        getLatestLaunches: false,
                                                                                        getUpcomingLaunches: true));
            return OkOrError(result);
        }
    }
}