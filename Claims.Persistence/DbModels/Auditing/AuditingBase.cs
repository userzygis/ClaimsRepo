namespace Claims.Persistence.DbModels.Auditing
{
    public abstract class AuditingBase
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string? HttpRequestType { get; set; }
    }
}
