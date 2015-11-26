using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using PersonalSite.Middleware;
using Xunit;
using System.Net;

namespace PersonalSite.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
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
