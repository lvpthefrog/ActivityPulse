using static ActivityPulse.Domain.Enums;

namespace ActivityPulse.Domain
{
    public class UserState : AuditableEntity
    {
        public State State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
