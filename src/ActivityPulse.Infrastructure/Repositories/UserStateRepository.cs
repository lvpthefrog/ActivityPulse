using ActivityPulse.Application;
using ActivityPulse.Domain;
using Dapper;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public class UserStateRepository : IUserStateRepository
    {
        private readonly IDbConnection _connection;

        public UserStateRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<long> AddAsync(UserState userState)
        {
            const string sql = @"
                INSERT INTO UserState
                (State, StartTime, EndTime, CreatedAt)
                VALUES (@State, @StartTime, @EndTime, @CreatedAt);
                SELECT last_insert_rowid();";
            return await _connection.ExecuteScalarAsync<long>(sql, userState);
        }

        public async Task UpdateAsync(UserState userState)
        {
            const string sql = @"
                UPDATE UserState
                SET State = @State, StartTime = @StartTime, EndTime = @EndTime
                WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, userState);
        }

        public async Task<UserState?> GetByIdAsync(long id)
        {
            const string sql = "SELECT * FROM UserState WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<UserState>(sql, new { Id = id });
        }


        public async Task DeleteAsync(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return;
            var sql = "DELETE FROM Session WHERE Id IN @Ids";
            await _connection.ExecuteAsync(sql, new { Ids = ids });
        }

        public async Task<List<UserState>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            const string sql = @"
                SELECT * FROM UserState
                WHERE StartTime >= @From AND StartTime <= @To
                ORDER BY StartTime";
            return (await _connection.QueryAsync<UserState>(sql, new { From = from, To = to })).ToList();
        }
    }
}
