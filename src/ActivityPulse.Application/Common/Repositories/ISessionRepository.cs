using ActivityPulse.Domain;

namespace ActivityPulse.Application
{
    public interface ISessionRepository
    {
        Task<long> AddAsync(Session session);
        Task UpdateAsync(Session session);
        Task<Session?> GetByIdAsync(long id);
        Task DeleteAsync(List<long> ids);
        Task<List<Session>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
