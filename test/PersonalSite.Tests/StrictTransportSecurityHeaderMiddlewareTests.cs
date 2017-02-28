using PersonalSite.Middleware;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class StrictTransportSecurityHeaderMiddlewareTests
    {
        [Fact]
        public async Task DoesntEmitStrictTransportSecurityHeaderForNoneHttpsRequests()
        {
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.False(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
        }

        [Fact]
        public async Task EmitStrictTransportSecurityHeader()
        {
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge);
            });
            var response = await server.CreateRequest("https://server/").GetAsync();

            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.StartsWith($"max-age={maxAge.TotalSeconds}", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }

        [Fact]
        public async Task EmitStrictTransportSecurityHeaderWithIncludeSubdomainsValue()
        {
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
            app.UseStrictTransportSecurityHeader(maxAge, includeSubDomains: true);
            });
            var response = await server.CreateRequest("https://server/").GetAsync();

            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.EndsWith("includeSubDomains", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }

        [Fact]
        public async Task EmitStrictTransportSecurityHeaderWithPreloadValue()
        {
            var maxAge = TimeSpan.FromDays(1);
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseStrictTransportSecurityHeader(maxAge, preload: true);
            });
            var response = await server.CreateRequest("https://server/").GetAsync();

            Assert.True(response.Headers.Contains(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName));
            Assert.EndsWith("preload", response.Headers.GetValues(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeaderName).First());
        }
    }
}
