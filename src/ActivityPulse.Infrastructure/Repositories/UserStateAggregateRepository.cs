using ActivityPulse.Application;
using ActivityPulse.Domain;
using Dapper;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public class UserStateAggregateRepository : IUserStateAggregateRepository
    {
        private readonly IDbConnection _connection;

        public UserStateAggregateRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<long> AddAsync(UserStateAggregate userStateAggregate)
        {
            const string sql = @"
                INSERT INTO UserStateAggregate
                (Date, State, Duration, CreatedAt)
                VALUES (@Date, @State, @Duration, @CreatedAt);
                SELECT last_insert_rowid();";
            return await _connection.ExecuteScalarAsync<long>(sql, userStateAggregate);
        }

        public async Task<List<UserStateAggregate>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            const string sql = @"
                SELECT * FROM UserStateAggregate
                WHERE Date >= @From AND Date <= @To
                ORDER BY Date";
            return (await _connection.QueryAsync<UserStateAggregate>(sql, new { From = from, To = to })).ToList();
        }

    }
}
