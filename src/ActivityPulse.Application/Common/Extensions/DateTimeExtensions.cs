namespace ActivityPulse.Application
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            var diff = dateTime.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            return dateTime.AddDays(-diff).Date;
        }
        public static DateTime EndOfWeek(this DateTime dateTime)
        {
            var diff = DayOfWeek.Sunday - dateTime.DayOfWeek;
            if (diff < 0) diff += 7;
            return dateTime.AddDays(diff).Date.AddDays(1).AddTicks(-1);
        }
    }
}
