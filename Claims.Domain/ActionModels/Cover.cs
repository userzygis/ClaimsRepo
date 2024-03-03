using Newtonsoft.Json;

namespace Claims.Domain.ActionModels;

public class Cover : IComputePremiumData
{
    /// <summary>
    /// unique identifier
    /// </summary>
    [JsonProperty(PropertyName = "id", Required = Required.AllowNull)]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "startDate")]
    public DateOnly StartDate { get; set; }

    [JsonProperty(PropertyName = "endDate")]
    public DateOnly EndDate { get; set; }

    [JsonProperty(PropertyName = "coverType")]
    public CoverType CoverType { get; set; }

    [JsonProperty(PropertyName = "premium")]
    public decimal Premium { get; set; }
}

public enum CoverType
{
    Yacht = 0,
    PassengerShip = 1,
    ContainerShip = 2,
    BulkCarrier = 3,
    Tanker = 4
}