namespace ActivityPulse.Infrastructure
{
    public static class InfraHelper
    {
        public static string GetDatabasePath()
        {
            return Path.Combine(GetDatabaseDirectory(), Constants.DatabaseName);
        }

        public static string GetDatabaseDirectory()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dir = Path.Combine(appData, Constants.DatabaseFolder);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
