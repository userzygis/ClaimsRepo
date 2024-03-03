using Newtonsoft.Json;

namespace Claims.Persistence.DbModels
{
    public abstract class CosmoModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
