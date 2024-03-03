using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;

namespace Claims.Services.Interfaces
{
    public interface ICoversMapper
    {
        Cover FromDbModel(CoverModel claimModel);
        CoverModel FromRequest(Cover claim);
    }
}
