namespace SpacexServer.Api.Commands.SpacexLaunches
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.SpacexLaunches.Responses;
    using SpacexServer.Api.Contracts.SpacexLaunches.Services;

    public class GetPastLaunchesQuery(int pageNumber, int pageSize) : IQuery<PagedResult<SpaceXLaunchDto>>
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public class GetPastLaunchesQueryHandler(SpaceXLaunchService launchService) : IQueryHandler<GetPastLaunchesQuery, PagedResult<SpaceXLaunchDto>>
    {
        private readonly SpaceXLaunchService _launchService = launchService;

        public async Task<Result<PagedResult<SpaceXLaunchDto>>> ExecuteAsync(GetPastLaunchesQuery query)
        {
            var launches = await _launchService.GetCachedPastLaunchesAsync();

            var totalItems = launches.Count;
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
            var pagedLaunches = launches
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