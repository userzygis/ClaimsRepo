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
        private readonly IClaimsValidator _claimsValidator;
        private readonly ICoversRepository _coversRepository;

        public ClaimsService(IAuditerService auditerService, IClaimsRepository claimsRepository, IClaimsMapper claimsMapper, IClaimsValidator claimsValidator, ICoversRepository coversRepository)
        {
            _auditerService = auditerService;
            _claimsRepository = claimsRepository;
            _claimsMapper = claimsMapper;
            _claimsValidator = claimsValidator;
            _coversRepository = coversRepository;
        }

        public async Task<string> AddClaimAsync(Claim claim)
        {
            await PerformValidation(claim);
            ClaimModel claimModel = _claimsMapper.FromRequest(claim);
            await _claimsRepository.AddClaimAsync(claimModel);
            await _auditerService.AuditClaimCreateAsync(claimModel.Id);
            return claimModel.Id;
        }

        private async Task PerformValidation(Claim claim)
        {
            _claimsValidator.ValidateDamageCost(claim);
            var coverModel = await _coversRepository.GetCoverAsync(claim.CoverId);
            _claimsValidator.ValidateCreatedDate(claim, coverModel);
        }

        public async Task DeleteClaimAsync(string id)
        {
            await _claimsRepository.DeleteClaimAsync(id); 
            await _auditerService.AuditClaimDeleteAsync(id);
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
