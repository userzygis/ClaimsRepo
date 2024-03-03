using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;
using Claims.Services.Interfaces;

namespace Claims.Services.Mappers
{
    public class ClaimsMapper : IClaimsMapper
    {
        public Claim FromDbModel(ClaimModel claimModel)
        {
            //todo automapper could be used
            return new Claim()
            {
                CoverId = claimModel.CoverId,
                Created = claimModel.Created,
                DamageCost = claimModel.DamageCost,
                Id = claimModel.Id,
                Name = claimModel.Name,
                Type = (ClaimType)claimModel.Type,
            };
        }

        public ClaimModel FromRequest(Claim claim)
        {
            //todo automapper could be used
            return new ClaimModel()
            {
                CoverId = claim.CoverId,
                Created = claim.Created,
                DamageCost = claim.DamageCost,
                Id= claim.Id,   
                Name= claim.Name,   
                Type = (Persistence.DbModels.ClaimModelType)claim.Type
            };
        }
    }
}
