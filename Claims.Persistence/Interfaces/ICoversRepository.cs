using Claims.Persistence.DbModels;

namespace Claims.Persistence.Interfaces
{
    public interface ICoversRepository
    {
        Task<IEnumerable<CoverModel>> GetCoversAsync();

        Task<CoverModel> GetCoverAsync(string id);

        Task AddCoverAsync(CoverModel item);

        Task DeleteCoverAsync(string id);
    }
}
