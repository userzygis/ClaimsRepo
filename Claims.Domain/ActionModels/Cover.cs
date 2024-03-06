namespace Claims.Domain.ActionModels;

public class Cover : IComputePremiumData
{
    /// <summary>
    /// unique identifier
    /// </summary>
    public string? Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public CoverType CoverType { get; set; }

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