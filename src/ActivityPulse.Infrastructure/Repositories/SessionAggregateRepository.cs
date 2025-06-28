using ActivityPulse.Application;
using ActivityPulse.Domain;
using Dapper;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public class SessionAggregateRepository : ISessionAggregateRepository
    {
        private readonly IDbConnection _connection;

        public SessionAggregateRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddAsync(SessionAggregate session)
        {
            const string sql = @"
                INSERT INTO SessionAggregate
                (Date, ProcessName, ProcessPath, TotalDuration, SessionCount, CreatedAt)
                VALUES (@Date, @ProcessName, @ProcessPath, @TotalDuration, @SessionCount, @CreatedAt)";
            await _connection.ExecuteAsync(sql, session);
        }

        public async Task<List<SessionAggregate>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            const string sql = @"
                SELECT * FROM SessionAggregate
                WHERE Date >= @From AND Date <= @To
                ORDER BY Date";
            return (await _connection.QueryAsync<SessionAggregate>(sql, new { From = from, To = to })).ToList();
        }
    }
}
