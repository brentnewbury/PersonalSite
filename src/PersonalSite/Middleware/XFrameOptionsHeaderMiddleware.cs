using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>X-Frame-Options</c> header as part of the response..
    /// </summary>
    public class XFrameOptionsHeaderMiddleware
    {
        /// <summary>
        /// The name of the <c>X-Frame-Options</c> header.
        /// </summary>
        public const string XFrameOptionsHeaderName = "X-Frame-Options";

        private readonly XFrameOptions _options;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialises a new <see cref="XFrameOptionsHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="options">The options to configure this middleware.</param>
        public XFrameOptionsHeaderMiddleware(RequestDelegate next, XFrameOptions options)
        {
            _next = next;
            _options = options;
        }

        /// <summary>
        /// Processes the request and emits the <c>X-Frame-Options</c> header.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            AddXFrameOptionsHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddXFrameOptionsHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(XFrameOptionsHeaderName))
                return;

            headers[XFrameOptionsHeaderName] = (_options.Option == XFrameOption.Deny) ? "DENY" : "SAMEORIGIN";
        }
    }
}
