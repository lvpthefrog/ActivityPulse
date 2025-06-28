using static ActivityPulse.Domain.Enums;

namespace ActivityPulse.Domain
{
    public class UserStateAggregate : AuditableEntity
    {
        public DateTime Date { get; set; }
        public State State { get; set; }
        public double Duration { get; set; }
    }
}
