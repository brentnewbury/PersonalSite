using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class XFrameOptionsHeaderMiddleware
    {
        public const string XFrameOptionsHeaderName = "X-Frame-Options";

        private XFrameOptions _options;
        private RequestDelegate _next;

        public XFrameOptionsHeaderMiddleware(RequestDelegate next, XFrameOptions options)
        {
            _next = next;
            _options = options;
        }

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
