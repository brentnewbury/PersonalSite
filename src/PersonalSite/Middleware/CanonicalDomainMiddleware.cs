using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Middleware component that redirects requests to specified domain, optionally requiring HTTPS.
    /// </summary>
    /// <remarks>
    /// We use this middleware rather than using <c>[RequireHttps]</c> as we don't want to potentially
    /// have two successive 301 redirects should the incoming request be on a non-canonical domain and
    /// not over HTTPS when required.
    /// </remarks>
    public class CanonicalDomainMiddleware
    {
        private const string HttpScheme = "https";
        private const string SchemeSeparator = "://";
        private const string LocationHeaderName = "Location";

        private readonly RequestDelegate _next;
        private readonly CanonicalDomainOptions _options;

        /// <summary>
        /// Initialises a new <see cref="CanonicalDomainMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">The configuration for this middleware.</param>
        public CanonicalDomainMiddleware(RequestDelegate next, IOptions<CanonicalDomainOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        /// <summary>
        /// Processes the request and redirects the request if required..
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            var redirectToCanonicalDomain = !String.Equals(request.Host.Value, _options.Domain, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(_options.Domain);
            var redirectToHttps = !request.IsHttps && _options.RequireHttps;

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
