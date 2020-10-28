using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryAPIIntegrationTest
{
    public class GettingServerStatusTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GettingServerStatusTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]//Do we get a 200?
        public async Task HasOkStatus()
        {
            var response = await _client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]//Is the response 
        public async Task ResponseIsJson()
        {
            var response = await _client.GetAsync("/status");
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.Equal("application/json", contentType);
        }

        [Fact]//Is the response 
        public async Task HasProperEntity()
        {
            var response = await _client.GetAsync("/status");
            var content = await response.Content.ReadAsAsync<GetStatusResponse>();

            Assert.Equal("All is good", content.message);
            Assert.Equal(DateTime.Now, content.createdAt);
        }
    }

    public class GetStatusResponse
    {
        public string message { get; set; }
        public DateTime createdAt { get; set; }
    }
}
