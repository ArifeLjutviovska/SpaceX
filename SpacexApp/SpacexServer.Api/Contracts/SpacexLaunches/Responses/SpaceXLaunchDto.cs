namespace SpacexServer.Api.Contracts.SpacexLaunches.Responses
{
    public class SpaceXLaunchDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateUtc { get; set; }
        public string? RocketId { get; set; }
        public bool? Success { get; set; }
        public string? Details { get; set; }
        public string? PatchImage { get; set; }
        public string? WebcastUrl { get; set; }
        public string? WikipediaUrl { get; set; }
        public string? ArticleUrl { get; set; }
        public string? Launchpad { get; set; }
    }
}
