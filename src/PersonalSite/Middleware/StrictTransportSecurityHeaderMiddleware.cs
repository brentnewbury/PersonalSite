using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class StrictTransportSecurityHeaderMiddleware
    {
        public const string StrictTransportSecurityHeaderName = "Strict-Transport-Security";

        private readonly RequestDelegate _next;
        private readonly string _value;

        public StrictTransportSecurityHeaderMiddleware(RequestDelegate next, StrictTransportSecurityHeaderOptions options)
        {
            _next = next;
            _value = BuildHeaderValue(options);
        }

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
