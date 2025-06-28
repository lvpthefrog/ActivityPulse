using ActivityPulse.Application;
using ActivityPulse.Domain;
using Dapper;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public class AppMetaDataRepository : IAppMetaDataRepository
    {
        private readonly IDbConnection _connection;
        public AppMetaDataRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<string> AddAsync(AppMetaData appMetaData)
        {
            const string sql = @"
                INSERT INTO AppMetaData
                (ProcessName, DisplayName, IconPath, CreatedAt)
                VALUES (@ProcessName, @DisplayName, @IconPath, @CreatedAt)";
            await _connection.ExecuteAsync(sql, appMetaData);
            return appMetaData.ProcessName;
        }

        public async Task UpdateAsync(AppMetaData appMetaData)
        {
            const string sql = @"
                UPDATE AppMetaData
                SET ProcessPath = @ProcessPath, DisplayName = @DisplayName, Category = @Category, IconPath = @IconPath
                WHERE ProcessName = @ProcessName";
            await _connection.ExecuteAsync(sql, appMetaData);
        }

        public async Task<List<AppMetaData>> GetByProcessNamesAsync(List<string> processNames)
        {
            if (processNames == null || processNames.Count == 0)
                return new();
            var sql = @"
                SELECT * FROM AppMetaData
                WHERE ProcessName IN @ProcessNames";
            return (await _connection.QueryAsync<AppMetaData>(sql, new { ProcessNames = processNames })).ToList();
        }
    }
}
