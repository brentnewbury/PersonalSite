using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class XContentTypeOptionsHeaderMiddleware
    {
        public const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        
        private readonly RequestDelegate _next;

        public XContentTypeOptionsHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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
