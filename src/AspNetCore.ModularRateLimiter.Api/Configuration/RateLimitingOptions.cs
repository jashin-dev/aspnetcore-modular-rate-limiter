namespace AspNetCore.ModularRateLimiter.Api.Configuration
{
    public sealed class RateLimitingOptions
    {
        public const string SectionName = "RateLimiting";
        public string? Algorithm { get; set; }
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
        public int SegmentsPerWindow { get; set; }
        public int TokenLimit { get; set; }
        public int TokensPerPeriod { get; set; }
        public int ReplenishmentPeriodSeconds { get; set; }
        public int QueueLimit { get; set; }
    }
}
