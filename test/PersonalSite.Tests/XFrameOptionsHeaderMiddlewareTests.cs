using Microsoft.AspNet.Builder;
using PersonalSite.Middleware;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class XFrameOptionsHeaderMiddlewareTests
    {
        [Fact]
        public async Task EmitXFrameOptionsHeaderWithDenyValue()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXFrameOptionsHeader(XFrameOption.Deny);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("DENY", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }

        [Fact]
        public async Task EmitXFrameOptionsHeaderWithSameOriginValue()
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseXFrameOptionsHeader(XFrameOption.SameOrigin);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName));
            Assert.Equal("SAMEORIGIN", response.Headers.GetValues(XFrameOptionsHeaderMiddleware.XFrameOptionsHeaderName).First());
        }
    }
}
