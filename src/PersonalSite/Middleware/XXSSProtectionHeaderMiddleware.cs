using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>X-XSS-Protection</c> header as part of the response.
    /// </summary>
    public class XXSSProtectionHeaderMiddleware
    {
        /// <summary>
        /// The name of the <c>X-XSS-Protection</c> header.
        /// </summary>
        public const string XXSSProtectionHeaderName = "X-XSS-Protection";

        private readonly RequestDelegate _next;
        private readonly string _value;

        /// <summary>
        /// Initialises a new <see cref="XXSSProtectionHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="options">The options to configure this middleware.</param>
        public XXSSProtectionHeaderMiddleware(RequestDelegate next, IOptions<XXSSProtectionHeaderOptions> options)
        {
            _next = next;
            _value = BuildHeaderValue(options.Value);
        }

        /// <summary>
        /// Processes the request and emits the <c>X-XSS-Protection</c> header.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            AddXXSSProtectionHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddXXSSProtectionHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(XXSSProtectionHeaderName))
                return;

            headers[XXSSProtectionHeaderName] = _value;
        }

        private static string BuildHeaderValue(XXSSProtectionHeaderOptions options)
        {
            var value = $"{Convert.ToInt32(options.Enabled)}";

            if (options.Enabled && options.Mode == XXSSProtectionMode.Block)
                value += "; mode=block";

            return value;
        }
    }
}
