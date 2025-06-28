using ActivityPulse.Domain;

namespace ActivityPulse.Application
{
    public interface ISessionAggregateRepository
    {
        Task AddAsync(SessionAggregate session);
        Task<List<SessionAggregate>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
