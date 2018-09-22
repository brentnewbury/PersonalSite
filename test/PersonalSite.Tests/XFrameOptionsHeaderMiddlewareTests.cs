using System;
using System.Linq;
using System.Threading.Tasks;
using PersonalSite.Middleware;
using Xunit;

namespace PersonalSite.Tests
{
    public class XFrameOptionsHeaderMiddlewareTests
    {
        [Fact]
        public async Task XFrameOptionsHeaderMiddleware_EmitsXFrameOptionsHeaderWithDenyValue()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXFrameOptionsHeader(XFrameOption.Deny);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

            // Assert
            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("DENY", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }

        [Fact]
        public async Task XFrameOptionsHeaderMiddleware_EmitsXFrameOptionsHeaderWithSameOriginValue()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXFrameOptionsHeader(XFrameOption.SameOrigin);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

            // Assert
            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("SAMEORIGIN", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }

        [Fact]
        public async Task XFrameOptionsHeaderMiddleware_EmitsXFrameOptionsHeaderWithAllowFromValue()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXFrameOptionsHeader(XFrameOption.CreateAllowFrom(new Uri("http://localhost/")));
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

            // Assert
            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("ALLOW-FROM http://localhost/", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }
    }
}
