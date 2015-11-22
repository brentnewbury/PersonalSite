using PersonalSite.Middleware;

namespace Microsoft.AspNet.Builder
{
    public static class RemoveServerHeaderExtensions
    {
        /// <summary>
        /// Removes the <c>Server</c> header from the response.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRemoveServerHeader(this IApplicationBuilder builder)
        {
            return builder.Use(next => new RemoveServerHeaderMiddleware(next).Invoke);
        }
    }
}
