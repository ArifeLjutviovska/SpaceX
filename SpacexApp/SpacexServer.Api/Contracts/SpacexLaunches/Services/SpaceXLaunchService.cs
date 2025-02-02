namespace SpacexServer.Api.Contracts.SpacexLaunches.Services
{
    using Microsoft.Extensions.Caching.Memory;
    using SpacexServer.Api.Contracts.SpacexLaunches.Responses;

    public class SpaceXLaunchService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public SpaceXLaunchService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<SpaceXLaunchDto>> GetCachedPastLaunchesAsync()
        {
            return await _cache.GetOrCreateAsync("pastLaunches", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                return await FetchPastLaunchesAsync();
            }) ?? [];
        }

        public async Task<List<SpaceXLaunchDto>> GetCachedLatestLaunchesAsync()
        {
            return await _cache.GetOrCreateAsync("latestLaunches", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                return await FetchLatestLaunchesAsync();
            }) ?? [];
        }

        public async Task<List<SpaceXLaunchDto>> GetCachedUpcomingLaunchesAsync()
        {
            return await _cache.GetOrCreateAsync("upcomingLaunches", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                return await FetchUpcomingLaunchesAsync();
            }) ?? [];
        }

        private async Task<List<SpaceXLaunchDto>> FetchPastLaunchesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<SpaceXLaunchDto>>("https://api.spacexdata.com/v5/launches/past");
            return response ?? [];
        }

        private async Task<List<SpaceXLaunchDto>> FetchLatestLaunchesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<SpaceXLaunchDto>("https://api.spacexdata.com/v5/launches/latest");
            return response != null ? [response] : [];
        }


        private async Task<List<SpaceXLaunchDto>> FetchUpcomingLaunchesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<SpaceXLaunchDto>>("https://api.spacexdata.com/v5/launches/upcoming");
            return response ?? [];
        }
    }
}
