using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Middleware component that redirects requests to specified domain, optionally requiring HTTPS.
    /// </summary>
    /// <remarks>
    /// We use this middleware rather than using <c>[RequireHtps]</c> as we don't want to potentially
    /// have two successive 301 redirects should the incoming request be on a non-canonical domain and
    /// not over HTTPS when required.
    /// </remarks>
    public class CanonicalDomainMiddleware
    {
        private const string HttpScheme = "https";
        private const string SchemeSeparator = "://";
        private const string LocationHeaderName = "Location";

        private RequestDelegate _next;
        private CanonicalDomainOptions _options;

        public CanonicalDomainMiddleware(RequestDelegate next, CanonicalDomainOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            bool redirectToCanonicalDomain = !String.Equals(request.Host.Value, _options.Domain, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(_options.Domain);
            bool redirectToHttps = !request.IsHttps && _options.RequireHttps;

            if (redirectToCanonicalDomain || redirectToHttps)
            {
                var location = CreateRedirectUri(context.Request);

                context.Response.Redirect(location, permanent: true);

                return;
            }

            await _next(context);
        }

        private string CreateRedirectUri(HttpRequest request)
        {
            var scheme = (_options.RequireHttps) ? HttpScheme: request.Scheme;
            var domain = (!String.IsNullOrEmpty(_options.Domain)) ? _options.Domain : request.Host.Value;
            var path = request.Path.ToUriComponent();
            var query = request.QueryString.ToUriComponent();

            return scheme + SchemeSeparator + domain + path + query;
        }
    }
}
