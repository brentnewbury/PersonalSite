using PersonalSite.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonalSite.Tests
{
    public class ReferrerPolicyHeaderMiddlewareTests
    {
        public static IEnumerable<object[]> Policies
        {
            get
            {
                yield return new object[] { ReferrerPolicy.NoReferrer, "no-referrer" };
                yield return new object[] { ReferrerPolicy.NoReferrerWhenDowngrade, "no-referrer-when-downgrade" };
                yield return new object[] { ReferrerPolicy.SameOrigin, "same-origin" };
                yield return new object[] { ReferrerPolicy.Origin, "origin" };
                yield return new object[] { ReferrerPolicy.StrictOrigin, "strict-origin" };
                yield return new object[] { ReferrerPolicy.OriginWhenCrossOrigin, "origin-when-cross-origin" };
                yield return new object[] { ReferrerPolicy.StrictOriginWhenCrossOrigin, "strict-origin-when-cross-origin" };
                yield return new object[] { ReferrerPolicy.UnsafeUrl, "unsafe-url" };
            }
        }

        [Theory]
        [MemberData(nameof(Policies))]
        public async Task EmitReferrerPolicyOptionValue(ReferrerPolicy policy, string value)
        {
            var server = PersonalSiteTestServer.Create(app =>
            {
                app.UseReferrerPolicyHeader(policy);
            });
            var response = await server.CreateRequest("http://server/").GetAsync();

            Assert.True(response.Headers.Contains(ReferrerPolicyHeaderMiddleware.ReferrerPolicyHeaderName));
            Assert.Equal(value, response.Headers.GetValues(ReferrerPolicyHeaderMiddleware.ReferrerPolicyHeaderName).First());
        }
    }
}
