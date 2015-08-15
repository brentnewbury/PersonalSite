using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class XContentTypeOptionsHeaderMiddleware
    {
        private const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        
        private RequestDelegate _next;

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
