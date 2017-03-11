using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>X-Content-Type-Options</c> header as part of the response with a value of <c>nosniff</c>.
    /// </summary>
    public class XContentTypeOptionsHeaderMiddleware
    {
        /// <summary>
        /// The <c>X-Content-Type-Options</c> header.
        /// </summary>
        public const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialises a new <see cref="XContentTypeOptionsHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        public XContentTypeOptionsHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Processes the request and emits the <c>X-Content-Type-Options</c> header with a value of <c>nosniff</c>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            AddXContentTypeOptionsHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddXContentTypeOptionsHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(XContentTypeOptionsHeaderName))
                return;

            headers[XContentTypeOptionsHeaderName] = "nosniff";
        }
    }
}
