using Azure.Core;
using Claims.ActionModels;
using Claims.ActionModels.Requests.CoverRequests;
using Claims.ActionModels.Responses;
using Claims.ActionModels.Responses.CoverResponses;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Claims.Controllers;

[ApiController]
[Route("api/v1/covers")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerErrorResponse))]
public class CoversController : ApiControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly Auditer _auditer;
    private readonly Container _container;

    public CoversController(CosmosClient cosmosClient, AuditContext auditContext, ILogger<CoversController> logger)
    {
        _logger = logger;
        _auditer = new Auditer(auditContext);
        _container = cosmosClient?.GetContainer("ClaimDb", "Cover")
                     ?? throw new ArgumentNullException(nameof(cosmosClient));
    }

    /// <summary>
    /// Computes premium
    /// </summary>
    /// <param name="request">params for computation</param>
    /// <returns></returns>
    [HttpPost("computePremium")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ComputePremiumResponse))]
    public async Task<ActionResult> ComputePremiumAsync(ComputePremiumRequest request)
    {
        //todo mv ComputePremium to svc
        //todo make computepremium async?: with when all complete
        var response = new ComputePremiumResponse() { TotalPremium = ComputePremium(request.StartDate, request.EndDate, request.CoverType) };

        return Ok(response);
    }

    /// <summary>
    /// Gets all covers
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCoversResponse))]
    public async Task<ActionResult> GetAsync(GetCoversRequest request)
    {
        //todo mv to svc:
        var query = _container.GetItemQueryIterator<Cover>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<Cover>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return Ok(new GetCoversResponse() { Covers = results });
    }

    /// <summary>
    /// Gets cover by id
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("cover{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCoverResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CoverNotFoundResponse))]
    public async Task<ActionResult> GetAsync(GetCoverRequest request)
    {
        try
        {
            var response = await _container.ReadItemAsync<Cover>(request.Id, new (request.Id));
            return response != null ? Ok(new GetCoverResponse() { Cover = response.Resource }) : CoverNotFound(request.Id);
        }
        //todo move this catch to svc return null instead, handle in middleware
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates cover
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("cover")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCoverResponse))]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest request)
    {
        var cover = request.Cover;
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _container.CreateItemAsync(cover, new PartitionKey(cover.Id));
        _auditer.AuditCover(cover.Id, "POST");
        return Ok(new CreateCoverResponse()
        {
            Id = cover.Id
        });
    }

    /// <summary>
    /// Deletes cover
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete("cover/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCoverResponse))]
    public async Task<ActionResult> DeleteAsync(DeleteCoverRequest request)
    {
        _auditer.AuditCover(request.Id, "DELETE");
        var cover = await _container.DeleteItemAsync<Cover>(request.Id, new (request.Id));
        return Ok(new DeleteCoverResponse() { ItemResponseCover = cover });
    }

    private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}