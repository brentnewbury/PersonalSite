using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class XXSSProtectionHeaderMiddleware
    {
        public const string XXSSProtectionHeaderName = "X-XSS-Protection";

        private RequestDelegate _next;
        private string _value;

        public XXSSProtectionHeaderMiddleware(RequestDelegate next, XXSSProtectionHeaderOptions options)
        {
            _next = next;
            _value = BuildHeaderValue(options);
        }

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
