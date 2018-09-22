using System;
using System.Linq;
using System.Threading.Tasks;
using PersonalSite.Middleware;
using Xunit;

namespace PersonalSite.Tests
{
    public class StrictTransportSecurityHeaderMiddlewareTests
    {
        [Fact]
        public async Task StrictTransportSecurityHeaderMiddleware_DoesntEmitStrictTransportSecurityHeaderForHttpRequests()
        {
            // Arrange
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

            // Assert
            Assert.False(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
        }

        [Fact]
        public async Task StrictTransportSecurityHeaderMiddleware_EmitsStrictTransportSecurityHeader()
        {
            // Arrange
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("https://server/");

            // Assert
            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.StartsWith($"max-age={maxAge.TotalSeconds}", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }

        [Fact]
        public async Task StrictTransportSecurityHeaderMiddleware_EmitsStrictTransportSecurityHeaderWithIncludeSubdomainsValue()
        {
            // Arrange
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge, includeSubDomains: true);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("https://server/");

            // Assert
            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.EndsWith("includeSubDomains", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }

        [Fact]
        public async Task StrictTransportSecurityHeaderMiddleware_EmitsStrictTransportSecurityHeaderWithPreloadValue()
        {
            // Arrange
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge, preload: true);
            });
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("https://server/");

            // Assert
            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.EndsWith("preload", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }
    }
}
