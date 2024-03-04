using Claims.Domain.ActionModels;
using Claims.Domain.Exceptions;
using Claims.Persistence.DbModels;
using Claims.Services.Interfaces;

namespace Claims.Services.Validators
{
    public class ClaimsValidator: IClaimsValidator
    {
        private const int MaxDamageCost = 100000;
        public void ValidateDamageCost(Claim claim)
        {
            if(claim.DamageCost > MaxDamageCost)
            {
                throw new ClaimsValidationException($"Damage cost cannot exceed {MaxDamageCost}");
            }
        }
        public void ValidateCreatedDate(Claim claim, CoverModel cover)
        {
            if (cover == null)
            {
                throw new ClaimsValidationException($"Cover '{claim.CoverId}' does not exist");
            }
            var createdDateOnly = DateOnly.FromDateTime(claim.Created);
            if (createdDateOnly < cover.StartDate || createdDateOnly > cover.EndDate)
            {
                throw new ClaimsValidationException($"Created date must be within the period of the related Cover ({cover.StartDate} - {cover.EndDate})");
            }
        }
    }
}
