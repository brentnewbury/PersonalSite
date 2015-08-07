using PersonalSite.Middleware;

namespace Microsoft.AspNet.Builder
{
    public static class CanonicalDomainExtensions
    {
        /// <summary>
        /// Redirects all requests to the specified <paramref name="domain"/>, optionally redirecting HTTP to HTTPS.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="domain">The domain to redirect to if the request is not for the domain specified.</param>
        /// <param name="requireHttps">Redirect any request made over HTTP to HTTPS.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCanonicalDomain(this IApplicationBuilder builder, string domain = null, bool requireHttps = false)
        {
            return builder.Use(next => new CanonicalDomainMiddleware(next, domain, requireHttps).Invoke);
        }
    }
}
