using Claims.Domain;
using Claims.Persistence.Interfaces;
using Claims.Services.Interfaces;

namespace Claims.Services.Impl
{
    //todo better idea would be to use service bus: await ServiceBus.SendQueueAsync(content, AuditQueueName, ttl, messageType: messageType)
    //and then create sb consumer (azure function app) to process queue messages and put in db
    public class AuditerService: IAuditerService
    {
        private readonly IAuditerRepository _auditerRepository;
        public AuditerService(IAuditerRepository auditerRepository)
        {
            _auditerRepository = auditerRepository;
        }

        public async Task AuditClaimCreateAsync(string id)
        {
            await _auditerRepository.AuditClaimAsync(id, Constants.AuditTypeCreate);
        }

        public async Task AuditClaimDeleteAsync(string id)
        {
            await _auditerRepository.AuditClaimAsync(id, Constants.AuditTypeDelete);
        }

        public async Task AuditCoverCreateAsync(string id)
        {
            await _auditerRepository.AuditCoverAsync(id, Constants.AuditTypeCreate);
        }

        public async Task AuditCoverDeleteAsync(string id)
        {
            await _auditerRepository.AuditCoverAsync(id, Constants.AuditTypeDelete);
        }
    }
}
