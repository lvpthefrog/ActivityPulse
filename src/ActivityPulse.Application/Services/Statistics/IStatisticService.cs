namespace ActivityPulse.Application
{
    public interface IStatisticService
    {
        Task AggregateAsync(DateTime now);
        Task<(List<AppUsageDto> AppUsage, UserStateDto UserState)> GetTodayDataAsync();
    }
}
