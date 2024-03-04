using Claims.Domain.ActionModels;
using Claims.Persistence.Interfaces;
using Claims.Services.Interfaces;

namespace Claims.Services.Impl
{
    public class CoversService : ICoversService
    {
        private readonly IPremiumCalcService _premiumCalcService;
        private readonly IAuditerService _auditerService;
        private readonly ICoversRepository _coversRepository;
        private readonly ICoversMapper _coversMapper;
        private readonly ICoversValidator _coversValidator;
        
        public CoversService(IPremiumCalcService premiumCalcService, IAuditerService auditerService, ICoversRepository coversRepository, ICoversMapper coversMapper, ICoversValidator coversValidator)
        {
            _premiumCalcService = premiumCalcService;
            _auditerService = auditerService;
            _coversRepository = coversRepository;
            _coversMapper = coversMapper;
            _coversValidator = coversValidator;
        }

        public async Task<string> AddCoverAsync(Cover cover)
        {
            PerformValidation(cover);
            cover.Premium = await _premiumCalcService.ComputePremiumAsync(cover);
            var coverModel = _coversMapper.FromRequest(cover);
            await _coversRepository.AddCoverAsync(coverModel);
            await _auditerService.AuditCoverCreateAsync(coverModel.Id);
            return coverModel.Id;
        }

        private void PerformValidation(Cover cover)
        {
            _coversValidator.ValidateStartDate(cover);
            _coversValidator.ValidateTotalInsurancePeriod(cover);
        }

        public async Task DeleteCoverAsync(string id)
        {
            await _coversRepository.DeleteCoverAsync(id);
            await _auditerService.AuditCoverDeleteAsync(id);
        }

        public async Task<Cover> GetCoverAsync(string id)
        {
            return _coversMapper.FromDbModel(await _coversRepository.GetCoverAsync(id));
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            return (await _coversRepository.GetCoversAsync()).Select(_coversMapper.FromDbModel);
        }
    }
}
