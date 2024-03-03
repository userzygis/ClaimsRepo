using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;
using Claims.Persistence.Interfaces;
using Claims.Services.Interfaces;

namespace Claims.Services.Impl
{
    public class ClaimsService : IClaimsService
    {
        private readonly IAuditerService _auditerService;
        private readonly IClaimsRepository _claimsRepository;
        private readonly IClaimsMapper _claimsMapper;
        
        public ClaimsService(IAuditerService auditerService, IClaimsRepository claimsRepository, IClaimsMapper claimsMapper)
        {
            _auditerService = auditerService;
            _claimsRepository = claimsRepository;
            _claimsMapper = claimsMapper;
        }

        public async Task<string> AddClaimAsync(Claim claim)
        {            
            ClaimModel claimModel = _claimsMapper.FromRequest(claim);
             await _claimsRepository.AddClaimAsync(claimModel);
            _auditerService.AuditClaimCreate(claimModel.Id);
            return claimModel.Id;
        }

        public async Task DeleteClaimAsync(string id)
        {
            await _claimsRepository.DeleteClaimAsync(id); 
            _auditerService.AuditClaimDelete(id);
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            return _claimsMapper.FromDbModel(await _claimsRepository.GetClaimAsync(id));
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {            
            return (await _claimsRepository.GetClaimsAsync()).Select(_claimsMapper.FromDbModel);
        }
    }
}
