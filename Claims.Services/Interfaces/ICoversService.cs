using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface ICoversService
    {
        Task<string> AddCoverAsync(Cover cover);

        Task DeleteCoverAsync(string id);

        Task<Cover> GetCoverAsync(string id);

        Task<IEnumerable<Cover>> GetCoversAsync();
    }
}
