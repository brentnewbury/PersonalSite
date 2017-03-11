using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>Strict-Transport-Security</c> header as part of the response, if the request is made over HTTPS.
    /// </summary>
    public class StrictTransportSecurityHeaderMiddleware
    {
        /// <summary>
        /// The <c>Strict-Transport-Security</c> header name.
        /// </summary>
        public const string StrictTransportSecurityHeaderName = "Strict-Transport-Security";

        private readonly RequestDelegate _next;
        private readonly string _value;

        /// <summary>
        /// Initialises a new <see cref="StrictTransportSecurityHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="options">The configuration for this middleware.</param>
        public StrictTransportSecurityHeaderMiddleware(RequestDelegate next, IOptions<StrictTransportSecurityHeaderOptions> options)
        {
            _next = next;
            _value = BuildHeaderValue(options.Value);
        }
        
        /// <summary>
        /// Processes the request and emits the <c>Strict-Transport-Security</c> header, if the request is made over HTTPS.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
                AddStrictTransportSecurityHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddStrictTransportSecurityHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(StrictTransportSecurityHeaderName))
                return;

            headers[StrictTransportSecurityHeaderName] = _value;
        }

        private static string BuildHeaderValue(StrictTransportSecurityHeaderOptions options)
        {
            var value = $"max-age={options.MaxAge.TotalSeconds}";

            if (options.IncludeSubDomains)
                value += "; includeSubDomains";

            if (options.Preload)
                value += "; preload";

            return value;
        }
    }
}
