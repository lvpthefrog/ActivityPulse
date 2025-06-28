namespace ActivityPulse.Application
{
    public class UserStateDto
    {
        public double ActiveTime { get; set; }
        public double IdleTime { get; set; }

        public double TotalTime => ActiveTime + IdleTime;
    }

}
