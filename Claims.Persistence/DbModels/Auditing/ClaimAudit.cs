namespace Claims.Persistence.DbModels.Auditing
{
    public class ClaimAudit: AuditingBase
    {
        public string? ClaimId { get; set; }
    }
}
