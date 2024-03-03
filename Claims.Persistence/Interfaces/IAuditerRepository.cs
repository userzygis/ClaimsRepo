namespace Claims.Persistence.Interfaces
{
    public interface IAuditerRepository
    {
        void AuditClaim(string id, string httpRequestType);
        void AuditCover(string id, string httpRequestType);
    }
}
