using PersonalSite.Middleware;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class CanonicalDomainMiddlewareTests
    {
        [Fact]
        public async Task RedirectToHttps()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(requireHttps: true);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal("https", response.Headers.Location.Scheme);
        }

        [Fact]
        public async Task RedirectToCanonicalDomain()
        {
            var canonicalDomain = "other.com";
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(domain: canonicalDomain);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal(response.Headers.Location.Host, canonicalDomain);
        }

        [Fact]
        public async Task RedirectKeepsPathAndQueryIntact()
        {
            var canonicalDomain = "other.com";
            var pathAndQuery = "path?query=value";
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseCanonicalDomain(domain: canonicalDomain);
            });
            var response = await server.CreateRequest($"http://wrong.com/{pathAndQuery}").GetAsync();

            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal(canonicalDomain, response.Headers.Location.Host);
            Assert.Equal(pathAndQuery, response.Headers.Location.PathAndQuery.TrimStart('/'));
        }
    }
}
