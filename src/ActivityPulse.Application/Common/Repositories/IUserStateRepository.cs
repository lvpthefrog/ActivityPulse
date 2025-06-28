using ActivityPulse.Domain;

namespace ActivityPulse.Application
{
    public interface IUserStateRepository
    {
        Task<long> AddAsync(UserState userState);
        Task UpdateAsync(UserState userState);
        Task<UserState?> GetByIdAsync(long id);
        Task DeleteAsync(List<long> ids);
        Task<List<UserState>> GetByDateRangeAsync(DateTime from, DateTime to);
    }
}
