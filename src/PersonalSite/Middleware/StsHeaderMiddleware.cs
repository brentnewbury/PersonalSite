using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class StsHeaderMiddleware
    {
        private const string StsHeaderName = "Strict-Transport-Security";

        private StsHeaderOptions _options;
        private RequestDelegate _next;

        public StsHeaderMiddleware(RequestDelegate next, StsHeaderOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
                AddStsHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddStsHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(StsHeaderName))
                return;

            var value = $"max-age: {_options.MaxAge.TotalSeconds}";

            if (_options.IncludeSubDomains)
                value += "; includeSubDomains";

            if (_options.Preload)
                value += "; preload";

            headers[StsHeaderName] = value;
        }
    }
}
