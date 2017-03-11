using Microsoft.AspNetCore.Builder;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Extension methods for removing the <c>Server</c> header from the response.
    /// </summary>
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
