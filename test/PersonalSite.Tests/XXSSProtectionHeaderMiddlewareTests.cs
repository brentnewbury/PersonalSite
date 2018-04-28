using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class XXSSProtectionHeaderMiddlewareTests
    {
        [Fact]
        public async Task XXSSProtectionHeaderMiddleware_EmitsXXSSProtectionHeaderWithBlockMode()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXXSSProtectionHeader(enabled: true, mode: XXSSProtectionMode.Block);
            });

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.True(response.Headers.Contains(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName));
            Assert.StartsWith("1", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
            Assert.EndsWith("mode=block", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
        }

        [Fact]
        public async Task XXSSProtectionHeaderMiddleware_EmitsXXSSProtectionHeaderDisabled()
        {
            // Arrange
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXXSSProtectionHeader(enabled: false);
            });

            // Act
            var response = await server.CreateRequest("http://server/").GetAsync();

            // Assert
            Assert.True(response.Headers.Contains(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName));
            Assert.StartsWith("0", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
            Assert.DoesNotContain("mode", response.Headers.GetValues(XXSSProtectionHeaderMiddleware.XXSSProtectionHeaderName).First());
        }
    }
}
