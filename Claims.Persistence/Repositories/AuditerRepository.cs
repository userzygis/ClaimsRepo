﻿using Claims.Persistence.DbModels.Auditing;
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

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditContext.Add(claimAudit);
            _auditContext.SaveChanges();
        }
        
        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditContext.Add(coverAudit);
            _auditContext.SaveChanges();
        }
    }
}
