using System.Net;
using System.Threading.Tasks;
using PersonalSite.Middleware;
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
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

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
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://server/");

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
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync($"http://wrong.com/{pathAndQuery}");

            // Assert
            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal(canonicalDomain, response.Headers.Location.Host);
            Assert.Equal(pathAndQuery, response.Headers.Location.PathAndQuery.TrimStart('/'));
        }
    }
}
