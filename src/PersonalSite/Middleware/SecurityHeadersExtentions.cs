using Microsoft.AspNet.Builder;
using PersonalSite.Middleware;
using System;

namespace Microsoft.AspNet.Builder
{
    public static class SecurityHeadersExtentions
    {
        /// <summary>
        /// Adds a <c>Strict-Transport-Security</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options configuring the <c>Strict-Transport-Security</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStsHeader(this IApplicationBuilder builder, StsHeaderOptions options)
        {
            return builder.Use(next => new StsHeaderMiddleware(next, options).Invoke);
        }

        /// <summary>
        /// Adds a <c>Strict-Transport-Security</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="maxAge">Sets the duration the browser should internally redirect requests to HTTPS.</param>
        /// <param name="includeSubDomains"><see langword="true"/> to instruct the browser to also redirect requests to subdomains to HTTPS.</param>
        /// <param name="preload"><see langword="true"/> to tell vendors to include the domain on a pre-loaded list of domains to automatically redirect requests to HTTPS.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStsHeader(this IApplicationBuilder builder, TimeSpan maxAge, bool includeSubDomains = false, bool preload = false)
        {
            var options = new StsHeaderOptions
            {
                MaxAge = maxAge,
                IncludeSubDomains = includeSubDomains,
                Preload = preload
            };

            return builder.Use(next => new StsHeaderMiddleware(next, options).Invoke);
        }

        /// <summary>
        /// Adds a <c>X-Content-Type-Options</c> header with a value of <c>nosniff</c> to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseXContentTypeOptionsHeader(this IApplicationBuilder builder)
        {
            return builder.Use(next => new XContentTypeOptionsHeaderMiddleware(next).Invoke);
        }
    }
}
