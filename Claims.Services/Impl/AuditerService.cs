using Claims.Domain;
using Claims.Persistence.Interfaces;
using Claims.Services.Interfaces;

namespace Claims.Services.Impl
{
    //todo better idea would be to use service bus: await ServiceBus.SendQueueAsync(content, AuditQueueName, ttl, messageType: messageType)
    //and then consume this message and put in db
    public class AuditerService: IAuditerService
    {
        private readonly IAuditerRepository _auditerRepository;
        public AuditerService(IAuditerRepository auditerRepository)
        {
            _auditerRepository = auditerRepository;
        }

        public void AuditClaimCreate(string id)
        {
            //todo add logging?
            _auditerRepository.AuditClaim(id, Constants.AuditTypeCreate);
        }

        public void AuditClaimDelete(string id)
        {
            //todo add logging?
            _auditerRepository.AuditClaim(id, Constants.AuditTypeDelete);
        }

        public void AuditCoverCreate(string id)
        {
            //todo add logging?
            _auditerRepository.AuditCover(id, Constants.AuditTypeCreate);
        }

        public void AuditCoverDelete(string id)
        {
            //todo add logging?
            _auditerRepository.AuditCover(id, Constants.AuditTypeDelete);
        }
    }
}
