using ActivityPulse.Application;

namespace ActivityPulse.WPF
{
    public class AggregatorService
    {
        private readonly IStatisticService _statisticService;
        private DateTime? _lastAggregateTime;

        public AggregatorService(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public async Task<(List<AppUsageDto>, UserStateDto)> GetTodayDataAsync()
        {
            return await _statisticService.GetTodayDataAsync();
        }

        public async Task StartAggregation()
        {
            var today = DateTime.Now.Date;
            if (_lastAggregateTime?.Date == today)
            {
                return;
            }

            await _statisticService.AggregateAsync(today);
            _lastAggregateTime = today;
        }
    }
}
