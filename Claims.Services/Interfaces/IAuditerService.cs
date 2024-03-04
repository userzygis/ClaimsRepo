namespace Claims.Services.Interfaces
{
    public interface IAuditerService
    {
        Task AuditClaimCreateAsync(string id);

        Task AuditClaimDeleteAsync(string id);

        Task AuditCoverCreateAsync(string id);

        Task AuditCoverDeleteAsync(string id);
    }
}
