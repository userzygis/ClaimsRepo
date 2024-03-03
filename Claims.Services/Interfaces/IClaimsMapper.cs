using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;

namespace Claims.Services.Interfaces
{
    public interface IClaimsMapper
    {
        Claim FromDbModel(ClaimModel claimModel);
        ClaimModel FromRequest(Claim claim);
    }
}
