using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class ContentSecurityPolicyHeaderMiddleware
    {
        private const string ContentSecurityPolicyHeaderName = "Content-Security-Policy";

        private RequestDelegate _next;
        private string _value;

        public ContentSecurityPolicyHeaderMiddleware(RequestDelegate next, ContentSecurityPolicyHeaderOptions options)
        {
            _next = next;
            _value = BuildHeaderValue(options);
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
                AddContentSecurityPolicyHeader(context.Response.Headers);

            await _next.Invoke(context);
        }

        private void AddContentSecurityPolicyHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey(ContentSecurityPolicyHeaderName))
                return;

            headers[ContentSecurityPolicyHeaderName] = _value;
        }

        private static string BuildHeaderValue(ContentSecurityPolicyHeaderOptions options)
        {
            List<string> policies = new List<string>();

            if (options.DefaultSources.Count > 0)
                policies.Add($"default-src {String.Join(" ", options.DefaultSources)};");

            if (options.ImageSources.Count > 0)
                policies.Add($"img-src {String.Join(" ", options.ImageSources)};");

            if (options.ScriptSources.Count > 0)
                policies.Add($"script-src {String.Join(" ", options.ScriptSources)};");

            if (options.StyleSources.Count > 0)
                policies.Add($"style-src {String.Join(" ", options.StyleSources)};");

            if (options.FontSources.Count > 0)
                policies.Add($"font-src {String.Join(" ", options.FontSources)};");

            if (options.ConnectSources.Count > 0)
                policies.Add($"connect-src {String.Join(" ", options.ConnectSources)};");

            if (options.FrameSources.Count > 0)
                policies.Add($"frame-src {String.Join(" ", options.FrameSources)}");

            return String.Join(" ", policies);
        }
    }
}
