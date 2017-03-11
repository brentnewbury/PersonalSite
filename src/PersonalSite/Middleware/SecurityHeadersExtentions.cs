using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Extension methods to add various security related headers to the response.
    /// </summary>
    public static class SecurityHeadersExtentions
    {
        /// <summary>
        /// Adds a <c>Strict-Transport-Security</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options configuring the <c>Strict-Transport-Security</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStrictTransportSecurityHeader(this IApplicationBuilder builder, StrictTransportSecurityHeaderOptions options)
        {
            return builder.UseMiddleware<StrictTransportSecurityHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>Strict-Transport-Security</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="maxAge">Sets the duration the browser should internally redirect requests to HTTPS.</param>
        /// <param name="includeSubDomains"><see langword="true"/> to instruct the browser to also redirect requests to subdomains to HTTPS.</param>
        /// <param name="preload"><see langword="true"/> to tell vendors to include the domain on a pre-loaded list of domains to automatically redirect requests to HTTPS.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStrictTransportSecurityHeader(this IApplicationBuilder builder, TimeSpan maxAge, bool includeSubDomains = false, bool preload = false)
        {
            var options = new StrictTransportSecurityHeaderOptions
            {
                MaxAge = maxAge,
                IncludeSubDomains = includeSubDomains,
                Preload = preload
            };

            return builder.UseMiddleware<StrictTransportSecurityHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>X-Content-Type-Options</c> header with a value of <c>nosniff</c> to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseXContentTypeOptionsHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XContentTypeOptionsHeaderMiddleware>();
        }

        /// <summary>
        /// Adds a <c>X-Frame-Options</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="option">Determines whether the header value is <c>DENY</c> or <c>SAMEORIGIN</c>.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseXFrameOptionsHeader(this IApplicationBuilder builder, XFrameOption option)
        {
            var options = new XFrameOptions
            {
                Option = option
            };

            return builder.UseMiddleware<XFrameOptionsHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>X-XSS-Protection</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options configuring the <c>X-XSS-Protection</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseXXSSProtectionHeader(this IApplicationBuilder builder, XXSSProtectionHeaderOptions options)
        {
            return builder.UseMiddleware<XXSSProtectionHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>X-XSS-Protection</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="enabled">Determines whether the header value is <c>1</c> or <c>0</c>.</param>
        /// <param name="mode">Appends the block mode to the header value, e.g. <c>; mode=deny</c>.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseXXSSProtectionHeader(this IApplicationBuilder builder, bool enabled = true, XXSSProtectionMode mode = XXSSProtectionMode.Block)
        {
            var options = new XXSSProtectionHeaderOptions
            {
                Enabled = enabled,
                Mode = mode
            };

            return builder.UseMiddleware<XXSSProtectionHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>Content-Security-Policy</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options configuring the <c>Content-Security-Policy</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseContentSecurityPolicyHeader(this IApplicationBuilder builder, ContentSecurityPolicyHeaderOptions options)
        {
            return builder.UseMiddleware<ContentSecurityPolicyHeaderMiddleware>(Options.Create(options));
        }


        /// <summary>
        /// Adds a <c>Referrer-Policy</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="policy">The <c>Refer-Policy</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseReferrerPolicyHeader(this IApplicationBuilder builder, ReferrerPolicy policy)
        {
            var options = new ReferrerPolicyHeaderOptions
            {
                Policy = policy
            };

            return builder.UseMiddleware<ReferrerPolicyHeaderMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds a <c>Referrer-Policy</c> header to the response. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options configuring the <c>Referrer-Policy</c> header value.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseReferrerPolicyHeader(this IApplicationBuilder builder, ReferrerPolicyHeaderOptions options)
        {
            return builder.UseMiddleware<ReferrerPolicyHeaderMiddleware>(Options.Create(options));
        }
    }
}
