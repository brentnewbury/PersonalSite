using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Removes the <c>Server</c> header from the response.
    /// </summary>
    public class RemoveServerHeaderMiddleware
    {
        private const string ServerHeaderName = "Server";

        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialises a new <see cref="RemoveServerHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        public RemoveServerHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Processes the request and removes the <c>Server</c> header.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Response.Headers.ContainsKey(ServerHeaderName))
                context.Response.Headers.Remove(ServerHeaderName);

            await _next(context);
        }
    }
}
