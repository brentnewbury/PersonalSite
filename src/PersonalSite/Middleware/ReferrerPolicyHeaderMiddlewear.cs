using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>Referrer-Policy</c> header as part of the response.
    /// </summary>
    public class ReferrerPolicyHeaderMiddleware
    {
        /// <summary>
        /// The <c>Referrer-Policy</c> header name.
        /// </summary>
        public const string ReferrerPolicyHeaderName = "Referrer-Policy";

        private readonly RequestDelegate _next;
        private readonly string _value;

        /// <summary>
        /// Initialises a new <see cref="ReferrerPolicyHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="options">The configuration for this middleware.</param>
        public ReferrerPolicyHeaderMiddleware(RequestDelegate next, ReferrerPolicyHeaderOptions options)
        {
            _next = next;
            _value = options?.Policy?.Value;
        }

        /// <summary>
        /// Processes the request and emits the <c>Content-Security-Polcy</c> header, if the request is made over HTTPS.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
