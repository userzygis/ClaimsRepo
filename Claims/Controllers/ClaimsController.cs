using Claims.ActionModels.Responses;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Claims.ActionModels;
using Claims.ActionModels.Responses.ClaimsResponses;
using Claims.ActionModels.Requests.ClaimsRequests;

namespace Claims.Controllers
{
    [ApiController]
    [Route("api/v1/claims")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerErrorResponse))]
    public class ClaimsController : ApiControllerBase
    {
        
        private readonly ILogger<ClaimsController> _logger;
        private readonly CosmosDbService _cosmosDbService;
        private readonly Auditer _auditer;

        public ClaimsController(ILogger<ClaimsController> logger, CosmosDbService cosmosDbService, AuditContext auditContext)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetClaimsResponse))]
        public async Task<ActionResult> GetAsync(GetClaimsRequest request)
        {
            var claims = await _cosmosDbService.GetClaimsAsync();
            return Ok(new GetClaimsResponse() { Claims = claims });
        }

        [HttpPut("claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateClaimResponse))]
        public async Task<ActionResult> CreateAsync(CreateClaimRequest request)
        {
            var claim = request.Claim;
            claim.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddItemAsync(claim);
            _auditer.AuditClaim(claim.Id, "POST");
            return Ok(new CreateClaimResponse() { Id = claim.Id });
        }

        [HttpDelete("claim/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteClaimResponse))]
        public async Task<ActionResult> DeleteAsync(DeleteClaimRequest request)
        {
            _auditer.AuditClaim(request.Id, "DELETE");
            await _cosmosDbService.DeleteItemAsync(request.Id);
            return Ok(new DeleteClaimResponse());
        }

        [HttpGet("claim/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetClaimResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ClaimNotFoundResponse))]
        public async Task<ActionResult> GetAsync(GetClaimRequest request)
        {
            var claim = await _cosmosDbService.GetClaimAsync(request.Id);
            return claim != null ? Ok(new GetClaimResponse() { Claim = claim }) : ClaimNotFound(request.Id);
        }
    }
    public class CosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            var query = _container.GetItemQueryIterator<Claim>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Claim>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Claim> GetClaimAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Claim>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Task AddItemAsync(Claim item)
        {
            return _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public Task DeleteItemAsync(string id)
        {
            return _container.DeleteItemAsync<Claim>(id, new PartitionKey(id));
        }
    }
}