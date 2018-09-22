using System.Linq;
using System.Threading.Tasks;
using PersonalSite.Middleware;
using Xunit;

namespace PersonalSite.Tests
{
    public class XContentTypeOptionsHeaderMiddlewareTests
    {
        [Fact]
        public async Task XContentTypeOptionsHeaderMiddleware_EmitsXFrameOptionsHeaderWithNosniffValue()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXContentTypeOptionsHeader();
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

            // Assert
            Assert.True(response.Headers.Contains(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName));
            Assert.Equal("nosniff", response.Headers.GetValues(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName).First());
        }
    }
}
