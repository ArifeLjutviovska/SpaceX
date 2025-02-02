namespace SpacexServer.Api.Commands.SpacexLaunches
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.SpacexLaunches.Responses;
    using SpacexServer.Api.Contracts.SpacexLaunches.Services;

    /// <summary>
    /// Query to retrieve paginated SpaceX launches from cache or external API.
    /// Allows fetching past, latest, or upcoming launches.
    /// </summary>
    /// <param name="pageNumber">The current page number for pagination.</param>
    /// <param name="pageSize">The number of launches per page.</param>
    /// <param name="getPastLaunches">Indicates if past launches should be retrieved.</param>
    /// <param name="getLatestLaunches">Indicates if the latest launch should be retrieved.</param>
    /// <param name="getUpcomingLaunches">Indicates if upcoming launches should be retrieved.</param>
    public class GetSpacexLaunchesQuery(int pageNumber, int pageSize, bool getPastLaunches, bool getLatestLaunches, bool getUpcomingLaunches) 
          : IQuery<PagedResult<SpaceXLaunchDto>>
    {
        public int PageNumber { get; set; } = pageNumber;

        public int PageSize { get; set; } = pageSize;

        public bool GetPastLaunches { get; set; } = getPastLaunches;

        public bool GetLatestLaunches { get; set; } = getLatestLaunches;

        public bool GetUpcomingLaunches { get; set; } = getUpcomingLaunches;
    }

    public class GetSpacexLaunchesQueryHandler(SpaceXLaunchService launchService) : IQueryHandler<GetSpacexLaunchesQuery, PagedResult<SpaceXLaunchDto>>
    {
        private readonly SpaceXLaunchService _launchService = launchService;

        public async Task<Result<PagedResult<SpaceXLaunchDto>>> ExecuteAsync(GetSpacexLaunchesQuery query)
        {
            List<SpaceXLaunchDto> launches = [];

            if (query.GetPastLaunches)
            {
                launches = await _launchService.GetCachedPastLaunchesAsync();
            }else if (query.GetLatestLaunches)
            {
                launches = await _launchService.GetCachedLatestLaunchesAsync();
            }
            else
            {
                launches = await _launchService.GetCachedUpcomingLaunchesAsync();
            }         

            int totalItems = launches?.Count ?? 0;
            if (totalItems == 0)
            {
                return Result.Ok(new PagedResult<SpaceXLaunchDto>
                {
                    Items = [],
                    TotalItems = 0,
                    TotalPages = 0,
                    CurrentPage = query.PageNumber
                });
            }

            var totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);
            var pagedLaunches = launches!
                .OrderByDescending(l => l.DateUtc)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var result = new PagedResult<SpaceXLaunchDto>
            {
                Items = pagedLaunches,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = query.PageNumber
            };

            return Result.Ok(result);
        }
    }
}