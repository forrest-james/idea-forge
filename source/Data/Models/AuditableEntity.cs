namespace Data.Models
{
    public abstract class AuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
    }
}