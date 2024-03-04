using Claims.Domain.ActionModels;
using Claims.Domain.Exceptions;
using Claims.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Claims.Services.Validators
{
    public class CoversValidator: ICoversValidator
    {
        public void ValidateStartDate(Cover cover)
        {
            if(cover.StartDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ClaimsValidationException($"StartDate cannot be in the past ({cover.StartDate})");
            }
        }

        public void ValidateTotalInsurancePeriod(Cover cover)
        {
            var startDatePlusYear = cover.StartDate.AddYears(1);
            var endDatePlusYear = cover.EndDate.AddYears(1);

            if ((cover.StartDate >= cover.EndDate || cover.EndDate >= startDatePlusYear) && (cover.StartDate < cover.EndDate || cover.StartDate >= endDatePlusYear))
            {
                throw new ClaimsValidationException($"Total insurance period cannot exceed 1 year ({cover.StartDate}-{cover.EndDate})");
            }
        }
    }
}
