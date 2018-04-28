using PersonalSite.Middleware;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class CanonicalDomainMiddlewareTests
    {
        [Fact]
        public async Task CanonicalDomainMiddleware_RedirectToHttps()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(requireHttps: true);
            });

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal("https", response.Headers.Location.Scheme);
        }

        [Fact]
        public async Task CanonicalDomainMiddleware_RedirectsToCanonicalDomain()
        {
            // Arrange
            var canonicalDomain = "other.com";
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(domain: canonicalDomain);
            });

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal(response.Headers.Location.Host, canonicalDomain);
        }

        [Fact]
        public async Task CanonicalDomainMiddleware_RedirectKeepsPathAndQueryIntact()
        {
            // Arrange
            var canonicalDomain = "other.com";
            var pathAndQuery = "path?query=value";
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(domain: canonicalDomain);
            });

            // Act
            var response = await server.CreateRequest($"http://wrong.com/{pathAndQuery}").GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal(canonicalDomain, response.Headers.Location.Host);
            Assert.Equal(pathAndQuery, response.Headers.Location.PathAndQuery.TrimStart('/'));
        }
    }
}
