using Claims.Persistence.DbModels.Auditing;
using Claims.Persistence.Interfaces;

namespace Claims.Persistence.Repositories
{
    public class AuditerRepository: IAuditerRepository
    {
        private readonly AuditContext _auditContext;

        public AuditerRepository(AuditContext auditContext)
        {
            _auditContext = auditContext;
        }

        public async Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await AuditAsync(claimAudit);
        }
        
        public async Task AuditCoverAsync(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await AuditAsync(coverAudit);
        }

        private async Task AuditAsync(AuditingBase auditData)
        {
            await _auditContext.AddAsync(auditData);
            await _auditContext.SaveChangesAsync();
        }
    }
}
