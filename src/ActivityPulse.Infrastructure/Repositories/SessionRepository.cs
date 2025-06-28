using ActivityPulse.Application;
using ActivityPulse.Domain;
using Dapper;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IDbConnection _connection;
        public SessionRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<long> AddAsync(Session session)
        {
            const string sql = @"
                INSERT INTO Session
                (StartTime, EndTime, ProcessName, ProcessPath, WindowTitle, CreatedAt)
                VALUES (@StartTime, @EndTime, @ProcessName, @ProcessPath, @WindowTitle, @CreatedAt);
                SELECT last_insert_rowid();";
            return await _connection.ExecuteScalarAsync<long>(sql, session);
        }

        public async Task UpdateAsync(Session session)
        {
            const string sql = @"
                UPDATE Session
                SET EndTime = @EndTime, ProcessName = @ProcessName, ProcessPath = @ProcessPath, WindowTitle = @WindowTitle
                WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, session);
        }

        public async Task<Session?> GetByIdAsync(long id)
        {
            const string sql = "SELECT * FROM Session WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Session>(sql, new { Id = id });
        }

        public async Task<List<Session>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            const string sql = @"
                SELECT * FROM Session
                WHERE StartTime >= @From AND StartTime <= @To
                ORDER BY StartTime";
            return (await _connection.QueryAsync<Session>(sql, new { From = from, To = to })).ToList();
        }

        public async Task DeleteAsync(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return;
            var sql = "DELETE FROM Session WHERE Id IN @Ids";
            await _connection.ExecuteAsync(sql, new { Ids = ids });
        }
    }
}
