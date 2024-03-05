using Claims.Domain.ActionModels;
using Claims.Services.Impl;
using Claims.Services.Interfaces;
using Xunit;

namespace Claims.Tests
{
    public class PremiumComputationTests
    {
        private readonly IPremiumCalcService premiumCalcService;
        public PremiumComputationTests()
        {
            premiumCalcService = new PremiumCalcService();
        }

        [Fact]
        public async Task PremiumDependsOnObjType()
        {
            ComputePremiumTestData data = new ComputePremiumTestData()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate= DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };

            data.CoverType = CoverType.Yacht;
            var totalPremiumYacht = await premiumCalcService.ComputePremiumAsync(data);

            data.CoverType = CoverType.PassengerShip;
            var totalPremiumPassengerShip = await premiumCalcService.ComputePremiumAsync(data);

            data.CoverType = CoverType.Tanker;
            var totalPremiumTanker = await premiumCalcService.ComputePremiumAsync(data);

            data.CoverType = CoverType.BulkCarrier;
            var totalPremiumBulkCarrier = await premiumCalcService.ComputePremiumAsync(data);

            data.CoverType = CoverType.ContainerShip;
            var totalPremiumContainerShip = await premiumCalcService.ComputePremiumAsync(data);

            Assert.True(totalPremiumTanker > totalPremiumPassengerShip && totalPremiumPassengerShip > totalPremiumYacht);
            Assert.Equal(totalPremiumBulkCarrier, totalPremiumContainerShip);
        }

        /// <summary>
        /// Premium depends on the length of the insurance period
        /// </summary>
        /// <returns></returns>
        [Fact]        
        public async Task PremiumDependsOnInsurancePeriodLength()
        {
            ComputePremiumTestData dataLengthDay = new ComputePremiumTestData()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };

            dataLengthDay.CoverType = CoverType.Yacht;
            var totalDayPremiumYacht = await premiumCalcService.ComputePremiumAsync(dataLengthDay);

            dataLengthDay.CoverType = CoverType.PassengerShip;
            var totalDayPremiumPassengerShip = await premiumCalcService.ComputePremiumAsync(dataLengthDay);

            dataLengthDay.CoverType = CoverType.Tanker;
            var totalDayPremiumTanker = await premiumCalcService.ComputePremiumAsync(dataLengthDay);

            dataLengthDay.CoverType = CoverType.BulkCarrier;
            var totalDayPremiumBulkCarrier = await premiumCalcService.ComputePremiumAsync(dataLengthDay);

            dataLengthDay.CoverType = CoverType.ContainerShip;
            var totalDayPremiumContainerShip = await premiumCalcService.ComputePremiumAsync(dataLengthDay);

            ComputePremiumTestData dataLengthMonth = new ComputePremiumTestData()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            };

            dataLengthDay.CoverType = CoverType.Yacht;
            var totalMonthPremiumYacht = await premiumCalcService.ComputePremiumAsync(dataLengthMonth);

            dataLengthDay.CoverType = CoverType.PassengerShip;
            var totalMonthPremiumPassengerShip = await premiumCalcService.ComputePremiumAsync(dataLengthMonth);

            dataLengthDay.CoverType = CoverType.Tanker;
            var totalMonthPremiumTanker = await premiumCalcService.ComputePremiumAsync(dataLengthMonth);

            dataLengthDay.CoverType = CoverType.BulkCarrier;
            var totalMonthPremiumBulkCarrier = await premiumCalcService.ComputePremiumAsync(dataLengthMonth);

            dataLengthDay.CoverType = CoverType.ContainerShip;
            var totalMonthPremiumContainerShip = await premiumCalcService.ComputePremiumAsync(dataLengthMonth);

            Assert.True(totalMonthPremiumYacht > totalDayPremiumYacht);
            Assert.True(totalMonthPremiumPassengerShip > totalDayPremiumPassengerShip);
            Assert.True(totalMonthPremiumTanker > totalDayPremiumTanker);
            Assert.True(totalMonthPremiumBulkCarrier > totalDayPremiumBulkCarrier);
            Assert.True(totalMonthPremiumContainerShip > totalDayPremiumContainerShip);
        }

        /// <summary>
        /// Base day rate was set to be 1250.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BaseDayRate1250()
        {
            Assert.Equal(1250, premiumCalcService.BaseDayRate);
        }

        /// <summary>
        ///Yacht should be 10% more expensive,
        ///Passenger ship 20%, 
        ///Tanker 50%,
        ///and other types 30%
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void TestMultiplierByCoverType()
        {            
            Assert.Equal(1.1m, premiumCalcService.GetMultiplier(CoverType.Yacht));
            Assert.Equal(1.2m, premiumCalcService.GetMultiplier(CoverType.PassengerShip));
            Assert.Equal(1.5m, premiumCalcService.GetMultiplier(CoverType.Tanker));
            Assert.Equal(1.3m, premiumCalcService.GetMultiplier(CoverType.BulkCarrier));
            Assert.Equal(1.3m, premiumCalcService.GetMultiplier(CoverType.ContainerShip));
        }

        /// <summary>
        /// First 30 days are computed based on the logic above
        /// </summary>
        [Fact]
        public async Task First30DaysNoDiscount()
        {
            for (int i = 1; i <= 30; i ++)
            {
                ComputePremiumTestData data = new ComputePremiumTestData()
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i))
                };

                data.CoverType = CoverType.Yacht;
                var totalDayPremiumYacht = await premiumCalcService.ComputePremiumAsync(data);

                data.CoverType = CoverType.PassengerShip;
                var totalDayPremiumPassengerShip = await premiumCalcService.ComputePremiumAsync(data);

                data.CoverType = CoverType.Tanker;
                var totalDayPremiumTanker = await premiumCalcService.ComputePremiumAsync(data);

                data.CoverType = CoverType.BulkCarrier;
                var totalDayPremiumBulkCarrier = await premiumCalcService.ComputePremiumAsync(data);

                data.CoverType = CoverType.ContainerShip;
                var totalDayPremiumContainerShip = await premiumCalcService.ComputePremiumAsync(data);

                var div = i * premiumCalcService.BaseDayRate;
                Assert.Equal(1.1m, totalDayPremiumYacht/div);
                Assert.Equal(1.2m, totalDayPremiumPassengerShip/div);
                Assert.Equal(1.5m, totalDayPremiumTanker/div);
                Assert.Equal(1.3m, totalDayPremiumBulkCarrier/div);
                Assert.Equal(1.3m, totalDayPremiumContainerShip/ div);
            }
        }

        /// <summary>
        /// Following 150 days are discounted by 5% for Yacht and by 2% for other types
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Following150Days5Yacht2othersDiscount()
        {
            await Following150DaysDiscount(CoverType.Yacht, 1.05m);
            await Following150DaysDiscount(CoverType.Tanker, 1.02m);
            await Following150DaysDiscount(CoverType.BulkCarrier, 1.02m);
            await Following150DaysDiscount(CoverType.PassengerShip, 1.02m);
            await Following150DaysDiscount(CoverType.ContainerShip, 1.02m);
        }


        /// <summary>
        /// The remaining days are discounted by additional 3% for Yacht and by 1% for other types
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task From151Day3Yacht1othersDiscount()
        {
            await From151DaysDiscount(CoverType.Yacht, 1.03m);
            await From151DaysDiscount(CoverType.Tanker, 1.01m);
            await From151DaysDiscount(CoverType.BulkCarrier, 1.01m);
            await From151DaysDiscount(CoverType.PassengerShip, 1.01m);
            await From151DaysDiscount(CoverType.ContainerShip, 1.01m);
        }

        #region private helpers
        private async Task From151DaysDiscount(CoverType coverType, decimal discountShouldBe)
        {
            for (int i = 151; i <= 365; i++)
            {
                ComputePremiumTestData data = new ComputePremiumTestData()
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    CoverType = coverType
                };

                var totalDayPremiumForCoverType = await premiumCalcService.ComputePremiumAsync(data);
                var premium150Days = await Get150DaysPremiumWithDiscount(data.CoverType);
                var premium151days = totalDayPremiumForCoverType - premium150Days;
                var premiumPerDay = premiumCalcService.BaseDayRate * premiumCalcService.GetMultiplier(data.CoverType);
                var discount = premiumPerDay / (premium151days / (i - 150));
                Assert.Equal(discountShouldBe, decimal.Round(discount, 2));
            }
        }

        private async Task Following150DaysDiscount(CoverType coverType, decimal discountShouldBe)
        {
            for (int i = 31; i <= 150; i++)
            {
                ComputePremiumTestData data = new ComputePremiumTestData()
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    CoverType = coverType
                };

                var totalDayPremiumForCoverType = await premiumCalcService.ComputePremiumAsync(data);
                var premium30Days = await Get30DaysPremiumWithNoDiscount(data.CoverType);
                var premium31_150days = totalDayPremiumForCoverType - premium30Days;
                var premiumPerDay = premiumCalcService.BaseDayRate * premiumCalcService.GetMultiplier(data.CoverType);
                var discount = premiumPerDay / (premium31_150days / (i - 30));
                Assert.Equal(discountShouldBe, decimal.Round(discount, 2));
            }
        }

        private async Task<decimal> Get30DaysPremiumWithNoDiscount(CoverType coverType)
        {
            ComputePremiumTestData data = new ComputePremiumTestData()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                CoverType = coverType,
            };
            return await premiumCalcService.ComputePremiumAsync(data);
        }

        private async Task<decimal> Get150DaysPremiumWithDiscount(CoverType coverType)
        {
            ComputePremiumTestData data = new ComputePremiumTestData()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(150)),
                CoverType = coverType,
            };
            return await premiumCalcService.ComputePremiumAsync(data);
        }

        #endregion
    }

    public class ComputePremiumTestData : IComputePremiumData
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CoverType CoverType { get; set; }
    }
}
