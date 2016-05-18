﻿using Microsoft.AspNetCore.Http;
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
        private string _domain;
        private bool _requireHttps;

        public CanonicalDomainMiddleware(RequestDelegate next, string domain = null, bool requireHttps = false)
        {
            _next = next;
            _domain = domain;
            _requireHttps = requireHttps;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            bool redirectToCanonicalDomain = !String.Equals(request.Host.Value, _domain, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(_domain);
            bool redirectToHttps = !request.IsHttps && _requireHttps;

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
            var scheme = (_requireHttps) ? HttpScheme: request.Scheme;
            var domain = (!String.IsNullOrEmpty(_domain)) ? _domain : request.Host.Value;
            var path = request.Path.ToUriComponent();
            var query = request.QueryString.ToUriComponent();

            return scheme + SchemeSeparator + domain + path + query;
        }
    }
}
