using Newtonsoft.Json;

namespace Claims.Persistence.DbModels
{
    public class ClaimModel: CosmoModel
    {
        [JsonProperty(PropertyName = "coverId")]
        public string CoverId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public ClaimModelType Type { get; set; }

        [JsonProperty(PropertyName = "damageCost")]
        public decimal DamageCost { get; set; }
    }

    public enum ClaimModelType
    {
        Collision = 0,
        Grounding = 1,
        BadWeather = 2,
        Fire = 3
    }
}
