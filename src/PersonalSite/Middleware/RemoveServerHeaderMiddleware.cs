using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class RemoveServerHeaderMiddleware
    {
        private const string ServerHeaderName = "Server";

        private RequestDelegate _next;

        public RemoveServerHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Response.Headers.ContainsKey(ServerHeaderName))
                context.Response.Headers.Remove(ServerHeaderName);

            await _next(context);
        }
    }
}
