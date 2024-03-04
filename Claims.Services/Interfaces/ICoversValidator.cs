using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface ICoversValidator
    {
        void ValidateStartDate(Cover cover);
        void ValidateTotalInsurancePeriod(Cover cover);
    }
}
