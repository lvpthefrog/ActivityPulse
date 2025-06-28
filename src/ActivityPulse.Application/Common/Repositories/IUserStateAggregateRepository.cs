using ActivityPulse.Domain;

namespace ActivityPulse.Application
{
    public interface IUserStateAggregateRepository
    {
        Task<long> AddAsync(UserStateAggregate userStates);
        Task<List<UserStateAggregate>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
