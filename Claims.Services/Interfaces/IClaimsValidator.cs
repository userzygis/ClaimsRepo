using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;

namespace Claims.Services.Interfaces
{
    public interface IClaimsValidator
    {
        void ValidateDamageCost(Claim claim);
        void ValidateCreatedDate(Claim claim, CoverModel cover);
    }
}
