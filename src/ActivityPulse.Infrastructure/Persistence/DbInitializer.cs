using Dapper;
using Microsoft.Data.Sqlite;

namespace ActivityPulse.Infrastructure
{
    public static class DbInitializer
    {
        public static void EnsureCreated()
        {
            var dbPath = InfraHelper.GetDatabasePath();
            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Dispose();
            }
            string sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts", "init_db.sql");
            string sqlScript = File.ReadAllText(sqlFilePath);

            if (!File.Exists(sqlFilePath))
                throw new FileNotFoundException("SQL migration script not found: " + sqlFilePath);

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            connection.Execute(sqlScript);
        }
    }
}
