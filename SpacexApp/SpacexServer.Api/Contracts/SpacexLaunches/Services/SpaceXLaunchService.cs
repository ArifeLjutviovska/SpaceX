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

        private async Task<List<SpaceXLaunchDto>> FetchPastLaunchesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<SpaceXLaunchDto>>("https://api.spacexdata.com/v5/launches/past");
            return response ?? [];
        }
    }
}
