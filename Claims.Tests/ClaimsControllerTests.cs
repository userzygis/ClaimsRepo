using Claims.Domain.ActionModels.Responses.ClaimsResponses;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using Xunit;
using Claims.Domain.ActionModels.Requests.ClaimsRequests;
using Microsoft.AspNetCore.Http;
using Claims.Domain.ActionModels.Responses;
using Claims.Domain.ActionModels.Responses.CoverResponses;
using Claims.Domain.ActionModels.Requests.CoverRequests;
using Claims.Tests.Extensions;
using Claims.Domain.ActionModels;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        public ClaimsControllerTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                { }); ;
        }

        [Theory]
        [InlineData("/api/v1/claims/claim")]
        public async Task Create_ClaimReturnFailAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            CreateClaimRequest req = new CreateClaimRequest()
            {
                Claim = new Domain.ActionModels.Claim()
                {
                    CoverId = "we dont have cover id", //we dont have CoverId, validation should fail
                    Created = DateTime.UtcNow,
                    DamageCost = 100,
                    Name = "from test",
                    Type = Domain.ActionModels.ClaimType.Collision
                }
            };

            var content = new StringContent(GetJson(req), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<ValidationErrorResponse>(jsonString);

            Assert.NotNull(obj);
            Assert.NotNull(obj.Error);            
        }

        [Theory]
        [InlineData("/api/v1/claims/claim")]
        public async Task Create_ClaimReturnSuccessCorrectContentTypeAndJson(string url)
        {
            var client = _factory.CreateClient();

            string coverId = await this.CreateTestCover(client, "/api/v1/covers/cover");

            CreateClaimRequest req = new CreateClaimRequest()
            {
                Claim = new Domain.ActionModels.Claim()
                {
                    CoverId = coverId,
                    Created = DateTime.UtcNow,
                    DamageCost = 100,
                    Name = "from test",
                    Type = Domain.ActionModels.ClaimType.Collision
                }
            };

            var content = new StringContent(GetJson(req), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<CreateClaimResponse>(jsonString);

            Assert.NotNull(obj);
            Assert.NotNull(obj.Id);
        }

        [Theory]
        [InlineData("/api/v1/claims")]
        public async Task Get_ClaimsReturnSuccessCorrectContentTypeAndJson(string url)
        {
            var client = _factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<GetClaimsResponse>(jsonString);

            Assert.NotNull(obj);
            Assert.NotNull(obj.Claims);
        }

        [Fact]
        public async Task Delete_ClaimReturnFailCorrectContentTypeAndJson()
        {
            var client = _factory.CreateClient();

            DeleteClaimRequest req = new DeleteClaimRequest()
            {
                Id = "bad_id"
            };

            StringContent content = new StringContent(GetJson(req), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.DeleteWithBodyAsync("/api/v1/claims/claim/", content);

            Assert.Equal(StatusCodes.Status500InternalServerError, (int)response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<InternalServerErrorResponse>(jsonString);

            Assert.NotNull(obj);
            Assert.NotNull(obj.Error);
        }

        [Fact]
        public async Task Delete_ClaimReturnSuccessCorrectContentTypeAndJson()
        {
            var client = _factory.CreateClient();
            var claimToDel = await GetClaim(client, "/api/v1/claims");

            Assert.NotNull(claimToDel);

            DeleteClaimRequest req = new DeleteClaimRequest()
            {
                Id = claimToDel.Id.ToString()
            };

            StringContent content = new StringContent(GetJson(req), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.DeleteWithBodyAsync("/api/v1/claims/claim/", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<DeleteClaimResponse>(jsonString);

            Assert.NotNull(obj);
        }

        private T GetObj<T>(string jsonString) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        private string GetJson<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        private async Task<string> CreateTestCover(HttpClient client, string url)
        {
            var req = new CreateCoverRequest()
            {
                Cover = new Domain.ActionModels.Cover()
                {
                    CoverType = Domain.ActionModels.CoverType.Yacht,
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                }
            };
            var content = new StringContent(GetJson(req), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<CreateCoverResponse>(jsonString);
            return obj.Id;
        }

        private async Task<Claim> GetClaim(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = GetObj<GetClaimsResponse>(jsonString);
            return obj.Claims.FirstOrDefault();
        }
    }
}
