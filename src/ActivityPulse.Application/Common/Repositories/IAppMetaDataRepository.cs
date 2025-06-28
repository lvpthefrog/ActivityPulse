using ActivityPulse.Domain;

namespace ActivityPulse.Application
{
    public interface IAppMetaDataRepository
    {
        Task<string> AddAsync(AppMetaData appMetaData);
        Task UpdateAsync(AppMetaData appMetaData);
        Task<List<AppMetaData>> GetByProcessNamesAsync(List<string> processNames);
    }
}
