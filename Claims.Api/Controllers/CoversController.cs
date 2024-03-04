using Claims.ActionModels.Requests.CoverRequests;
using Claims.ActionModels.Responses.CoverResponses;
using Claims.Domain.ActionModels.Requests.CoverRequests;
using Claims.Domain.ActionModels.Responses;
using Claims.Domain.ActionModels.Responses.CoverResponses;
using Claims.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("api/v1/covers")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerErrorResponse))]
public class CoversController : ApiControllerBase
{
    private readonly ICoversService _coversService;
    private readonly IPremiumCalcService _premiumCalcService;

    public CoversController(ICoversService coversService, IPremiumCalcService premiumCalcService)
    {
        _coversService = coversService;
        _premiumCalcService = premiumCalcService;
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
        var response = new ComputePremiumResponse() { TotalPremium = await _premiumCalcService.ComputePremiumAsync(request) };
        return Ok(response);
    }

    /// <summary>
    /// Gets all covers
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCoversResponse))]
    public async Task<ActionResult> GetAsync([FromQuery]GetCoversRequest request)
    {
        //todo now it is not used, but in future request may contain filter conditions 
        var covers = await _coversService.GetCoversAsync();
        return Ok(new GetCoversResponse() { Covers = covers });
    }

    /// <summary>
    /// Gets cover by id
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("cover")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCoverResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CoverNotFoundResponse))]
    public async Task<ActionResult> GetAsync([FromQuery]GetCoverRequest request)
    {
        var cover = await _coversService.GetCoverAsync(request.Id);
        return cover != null ? Ok(new GetCoverResponse() { Cover = cover }) : CoverNotFound(request.Id);
    }

    /// <summary>
    /// Creates cover
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("cover")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCoverResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest request)
    {
        var createdCoverId = await _coversService.AddCoverAsync(request.Cover);
        return Ok(new CreateCoverResponse() { Id = createdCoverId });
    }

    /// <summary>
    /// Deletes cover
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete("cover")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCoverResponse))]
    public async Task<ActionResult> DeleteAsync(DeleteCoverRequest request)
    {
        await _coversService.DeleteCoverAsync(request.Id);
        return Ok(new DeleteCoverResponse());
    }
}