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

        private RequestDelegate _next;
        private string _value;

        public StsHeaderMiddleware(RequestDelegate next, StsHeaderOptions options)
        {
            _next = next;
            _value = BuildHeaderValue(options);
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

            headers[StsHeaderName] = _value;
        }

        private static string BuildHeaderValue(StsHeaderOptions options)
        {
            var value = $"max-age: {options.MaxAge.TotalSeconds}";

            if (options.IncludeSubDomains)
                value += "; includeSubDomains";

            if (options.Preload)
                value += "; preload";

            return value;
        }
    }
}
