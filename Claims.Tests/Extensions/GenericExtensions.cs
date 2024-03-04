using Newtonsoft.Json;
using System.Text;

namespace Claims.Tests.Extensions
{
    public static class GenericExtensions
    {
        public static async Task<HttpResponseMessage> DeleteWithBodyAsync(this HttpClient client, string url, StringContent content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url, UriKind.Relative),
                Content = content
            };
            return await client.SendAsync(request);
        }
    }
}
