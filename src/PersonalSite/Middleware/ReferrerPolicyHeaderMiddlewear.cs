using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class ReferrerPolicyHeaderMiddleware
    {
        public const string ReferrerPolicyHeaderName = "Referrer-Policy";

        private readonly RequestDelegate _next;
        private readonly string _value;

        public ReferrerPolicyHeaderMiddleware(RequestDelegate next, ReferrerPolicyHeaderOptions options)
        {
            _next = next;
            _value = options?.Policy?.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            AddReferrerPolicyHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddReferrerPolicyHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(ReferrerPolicyHeaderName))
                return;

            headers[ReferrerPolicyHeaderName] = _value;
        }
    }
}
