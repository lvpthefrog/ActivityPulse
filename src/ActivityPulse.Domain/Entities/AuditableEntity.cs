namespace ActivityPulse.Domain
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
