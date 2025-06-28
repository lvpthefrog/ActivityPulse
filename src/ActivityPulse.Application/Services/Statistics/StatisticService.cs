using ActivityPulse.Domain;
using static ActivityPulse.Domain.Enums;

namespace ActivityPulse.Application
{
    public class StatisticService : IStatisticService
    {
        private readonly ISessionRepository _sessionRepo;
        private readonly IUserStateRepository _userStateRepo;
        private readonly ISessionAggregateRepository _sessionAggregateRepo;
        private readonly IUserStateAggregateRepository _userStateAggregateRepo;
        private readonly IAppMetaDataRepository _metaRepo;
        private readonly ISystemProvider _systemProvider;

        private Dictionary<string, AppMetaData> _metadataCache = new(StringComparer.OrdinalIgnoreCase);

        public StatisticService(
            ISessionRepository sessionRepo,
            IUserStateRepository userStateRepo,
            ISessionAggregateRepository aggregateRepo,
            IUserStateAggregateRepository userStateAggregateRepo,
            IAppMetaDataRepository metaRepo,
            ISystemProvider systemProvider
            )
        {
            _sessionRepo = sessionRepo;
            _userStateRepo = userStateRepo;
            _sessionAggregateRepo = aggregateRepo;
            _userStateAggregateRepo = userStateAggregateRepo;
            _metaRepo = metaRepo;
            _systemProvider = systemProvider;
        }

        public async Task<(List<AppUsageDto> AppUsage, UserStateDto UserState)> GetTodayDataAsync()
        {
            var from = DateTime.Now.StartOfDay();
            var to = DateTime.Now.EndOfDay();

            var appUsage = await GetTodayUserStatesAsync(from, to);

            var sessions = await _sessionRepo.GetByDateRangeAsync(from, to);
            var processNames = sessions.Select(s => s.ProcessName).Distinct().ToList();

            if (_metadataCache.Count == 0)
            {
                _metadataCache = (await _metaRepo.GetByProcessNamesAsync(processNames))
                    .ToDictionary(x => x.ProcessName, StringComparer.OrdinalIgnoreCase);
            }

            var aggregates = sessions
                .GroupBy(x => x.ProcessName)
                .Select(g => new AppUsageDto
                {
                    ProcessName = g.Key,
                    TotalDuration = g.Sum(s => ((s.EndTime.HasValue ? s.EndTime.Value : s.StartTime) - s.StartTime).TotalSeconds / 60),
                    IconPath = g.First().ProcessPath,
                })
                .ToList();

            foreach (var agg in aggregates)
            {
                if (!_metadataCache.TryGetValue(agg.ProcessName, out var meta))
                {
                    var iconPath = !string.IsNullOrEmpty(agg.IconPath) ? _systemProvider.ExtractIcon(agg.IconPath) : "";
                    meta = new AppMetaData
                    {
                        ProcessName = agg.ProcessName,
                        DisplayName = Helpers.GetDisplayName(agg.ProcessName),
                        IconPath = iconPath,
                    };
                    await _metaRepo.AddAsync(meta);
                    _metadataCache[agg.ProcessName] = meta;
                }
                agg.DisplayName = meta.DisplayName ?? Helpers.GetDisplayName(agg.ProcessName);
                agg.IconPath = meta.IconPath;
                agg.ProductivityPercentage = Math.Round(agg.TotalDuration / (appUsage.ActiveTime == 0 ? 1 : appUsage.ActiveTime) * 100);
            }

            return (aggregates.Where(x => x.TotalDuration > 0).OrderByDescending(aggregates => aggregates.TotalDuration).ToList(), appUsage);
        }

        public async Task<UserStateDto> GetTodayUserStatesAsync(DateTime from, DateTime to)
        {
            var userStates = await _userStateRepo.GetByDateRangeAsync(from, to);

            return new UserStateDto
            {
                ActiveTime = userStates
                    .Where(s => s.State == State.Active)
                    .Sum(s => ((s.EndTime.HasValue ? s.EndTime.Value : s.StartTime) - s.StartTime).TotalSeconds / 60),
                IdleTime = userStates.Where(s => s.State == State.Idle)
                    .Sum(s => ((s.EndTime.HasValue ? s.EndTime.Value : s.StartTime) - s.StartTime).TotalSeconds / 60),
            };
        }


        public async Task AggregateAsync(DateTime now)
        {
            var from = DateTime.MinValue;
            var to = now.StartOfDay().AddTicks(-1);

            await AggregateUserStateAsync(from, to, now);
            await AggregateSessionAsync(from, to, now);
        }

        private async Task AggregateSessionAsync(DateTime from, DateTime to, DateTime now)
        {
            var sessions = await _sessionRepo.GetByDateRangeAsync(from, to);
            var aggregates = sessions
                .GroupBy(s => s.ProcessName)
                .Select(g => new SessionAggregate
                {
                    Date = now.Date,
                    ProcessName = g.Key,
                    TotalDuration = g.Sum(s => ((s.EndTime.HasValue ? s.EndTime.Value : s.StartTime) - s.StartTime).TotalSeconds / 60),
                    SessionCount = g.Count()
                })
                .ToList();
            foreach (var item in aggregates)
            {
                await _sessionAggregateRepo.AddAsync(item);
            }
            if (sessions.Any())
            {
                await _sessionRepo.DeleteAsync(sessions.Select(s => s.Id).ToList());
            }
        }

        private async Task AggregateUserStateAsync(DateTime from, DateTime to, DateTime now)
        {
            var userStates = await _userStateRepo.GetByDateRangeAsync(from, to);
            var aggregates = userStates
                .GroupBy(s => s.State)
                .Select(g => new UserStateAggregate
                {
                    Date = now.Date,
                    State = g.Key,
                    Duration = g.Sum(s => ((s.EndTime.HasValue ? s.EndTime.Value : s.StartTime) - s.StartTime).TotalSeconds / 60)
                })
                .ToList();
            foreach (var item in aggregates)
            {
                await _userStateAggregateRepo.AddAsync(item);
            }
            if (userStates.Any())
            {
                await _userStateRepo.DeleteAsync(userStates.Select(s => s.Id).ToList());
            }
        }
    }
}
