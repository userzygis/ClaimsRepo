using Newtonsoft.Json;

namespace Claims.Persistence.DbModels
{
    public class CoverModel: CosmoModel
    {
        [JsonProperty(PropertyName = "startDate")]
        public DateOnly StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateOnly EndDate { get; set; }

        [JsonProperty(PropertyName = "coverType")]
        public CoverModelType Type { get; set; }

        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }
    }

    public enum CoverModelType
    {
        Yacht = 0,
        PassengerShip = 1,
        ContainerShip = 2,
        BulkCarrier = 3,
        Tanker = 4
    }
}
