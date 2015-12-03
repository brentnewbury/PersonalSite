using Microsoft.AspNet.Builder;
using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class XXSSProtectionHeaderMiddlewareTests
    {
        [Fact]
        public async Task EmitXXSSProtectionHeaderWithBlockMode()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXXSSProtectionHeader(enabled: true, mode: XXSSProtectionMode.Block);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName));
            Assert.StartsWith("1", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
            Assert.EndsWith("mode=block", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
        }

        [Fact]
        public async Task EmitXXSSProtectionHeaderDisabled()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXXSSProtectionHeader(enabled: false);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName));
            Assert.StartsWith("0", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
            Assert.DoesNotContain("mode", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
        }
    }
}
