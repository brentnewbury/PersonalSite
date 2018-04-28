using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System;

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

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

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

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

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

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("ALLOW-FROM http://localhost/", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }
    }
}
