using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Extension methods for adding the <see cref="CanonicalDomainMiddleware"/> into the request pipeline.
    /// </summary>
    public static class CanonicalDomainExtensions
    {
        /// <summary>
        /// Redirects all requests not matching the specified <paramref name="domain"/> to the domain, optionally redirecting HTTP to HTTPS.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="domain">The domain to redirect to if the request is not for the domain specified.</param>
        /// <param name="requireHttps">Redirect any request made over HTTP to HTTPS.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCanonicalDomain(this IApplicationBuilder builder, string domain = null, bool requireHttps = false)
        {
            var options = new CanonicalDomainOptions
            {
                Domain = domain,
                RequireHttps = requireHttps
            };

            return builder.UseMiddleware<CanonicalDomainMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Redirects all requests when not matching the domain in the specified <paramref name="options"/>, optionally redirecting HTTP to HTTPS.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">The options for the canonical domain.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCanonicalDomain(this IApplicationBuilder builder, CanonicalDomainOptions options)
        {
            return builder.UseMiddleware<CanonicalDomainMiddleware>(Options.Create(options));
        }
    }
}
