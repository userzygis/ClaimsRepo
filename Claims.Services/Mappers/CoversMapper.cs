using Claims.Domain.ActionModels;
using Claims.Persistence.DbModels;
using Claims.Services.Interfaces;

namespace Claims.Services.Mappers
{
    public class CoversMapper : ICoversMapper
    {
        public Cover FromDbModel(CoverModel coverModel)
        {
            //todo automapper could be used
            return new Cover()
            {
                EndDate = coverModel.EndDate,
                Id = coverModel.Id,
                Premium = coverModel.Premium,
                StartDate = coverModel.StartDate,
                CoverType = (CoverType)coverModel.Type  
            };
        }

        public CoverModel FromRequest(Cover cover)
        {
            //todo automapper could be used
            return new CoverModel()
            {
                EndDate = cover.EndDate,
                Id = cover.Id,
                Premium = cover.Premium,
                StartDate = cover.StartDate,
                Type = (Persistence.DbModels.CoverModelType)cover.CoverType   
            };
        }
    }
}
