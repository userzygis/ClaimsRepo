using Microsoft.AspNetCore.Mvc;
using Claims.Services.Interfaces;
using Claims.Domain.ActionModels.Requests.ClaimsRequests;
using Claims.Domain.ActionModels.Responses.ClaimsResponses;
using Claims.Domain.ActionModels.Responses;

namespace Claims.Controllers
{
    [ApiController]
    [Route("api/v1/claims")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerErrorResponse))]
    public class ClaimsController : ApiControllerBase
    {        
        private readonly ILogger<ClaimsController> _logger; //todso logergerger
        private readonly IClaimsService _claimsService;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService)
        {
            _logger = logger;
            _claimsService = claimsService;
        }

        /// <summary>
        /// Gets all claims
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetClaimsResponse))]
        public async Task<ActionResult> GetAsync([FromQuery]GetClaimsRequest request)
        {            
            //todo now it is not used, but in future request may contain filter conditions 
            var claims = await _claimsService.GetClaimsAsync();
            return Ok(new GetClaimsResponse() { Claims = claims });
        }

        /// <summary>
        /// Creates claims
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateClaimResponse))]
        public async Task<ActionResult> CreateAsync(CreateClaimRequest request)
        {
            var createdClaimId = await _claimsService.AddClaimAsync(request.Claim);
            return Ok(new CreateClaimResponse() { Id = createdClaimId });
        }

        /// <summary>
        /// Deletes claim
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteClaimResponse))]
        public async Task<ActionResult> DeleteAsync(DeleteClaimRequest request)
        {
            await _claimsService.DeleteClaimAsync(request.Id);
            return Ok(new DeleteClaimResponse());
        }

        /// <summary>
        /// Gets claims by id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetClaimResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ClaimNotFoundResponse))]
        public async Task<ActionResult> GetAsync([FromQuery]GetClaimRequest request)
        {
            var claim = await _claimsService.GetClaimAsync(request.Id);
            return claim != null ? Ok(new GetClaimResponse() { Claim = claim }) : ClaimNotFound(request.Id);
        }
    }
}