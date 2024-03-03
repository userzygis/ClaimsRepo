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

        public CoversService(IPremiumCalcService premiumCalcService, IAuditerService auditerService, ICoversRepository coversRepository, ICoversMapper coversMapper)
        {
            _premiumCalcService = premiumCalcService;
            _auditerService = auditerService;
            _coversRepository = coversRepository;
            _coversMapper = coversMapper;   
        }

        public async Task<string> AddCoverAsync(Cover cover)
        {
            cover.Premium = await _premiumCalcService.ComputePremiumAsync(cover);
            var coverModel = _coversMapper.FromRequest(cover);
            await _coversRepository.AddCoverAsync(coverModel);
            _auditerService.AuditCoverCreate(coverModel.Id);
            return coverModel.Id;
        }

        public async Task DeleteCoverAsync(string id)
        {
            await _coversRepository.DeleteCoverAsync(id);
            _auditerService.AuditCoverDelete(id);
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
