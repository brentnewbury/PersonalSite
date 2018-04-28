using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
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

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.True(response.Headers.Contains(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName));
            Assert.Equal("nosniff", response.Headers.GetValues(XContentTypeOptionsHeaderMiddleware.XContentTypeOptionsHeaderName).First());
        }
    }
}
