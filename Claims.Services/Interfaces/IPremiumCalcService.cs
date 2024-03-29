﻿using Claims.Domain.ActionModels;

namespace Claims.Services.Interfaces
{
    public interface IPremiumCalcService
    {
        Task<decimal> ComputePremiumAsync(IComputePremiumData data);
        int GetBaseDayRate(CoverType? coverType);
        decimal GetMultiplier(CoverType coverType);
    }
}
