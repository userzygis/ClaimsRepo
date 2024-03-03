namespace Claims.Services.Interfaces
{
    public interface IAuditerService
    {
        void AuditClaimCreate(string id);

        void AuditClaimDelete(string id);

        void AuditCoverCreate(string id);

        void AuditCoverDelete(string id);
    }
}
