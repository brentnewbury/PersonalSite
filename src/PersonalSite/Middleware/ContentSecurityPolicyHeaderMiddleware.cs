using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Emits the <c>Content-Security-Policy</c> header as part of the response, if the request is made over HTTPS.
    /// </summary>
    public class ContentSecurityPolicyHeaderMiddleware
    {
        private const string ContentSecurityPolicyHeaderName = "Content-Security-Policy";

        private readonly RequestDelegate _next;
        private readonly string _value;

        /// <summary>
        /// Initialises a new <see cref="ContentSecurityPolicyHeaderMiddleware"/> instance.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="options">The configuration for this middleware.</param>
        public ContentSecurityPolicyHeaderMiddleware(RequestDelegate next, IOptions<ContentSecurityPolicyHeaderOptions> options)
        {
            _next = next;
            _value = BuildHeaderValue(options.Value);
        }

        /// <summary>
        /// Processes the request and emits the <c>Content-Security-Polcy</c> header, if the request is made over HTTPS.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
            var policies = new List<string>();

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

            if (options.FrameAncestors.Count > 0)
                policies.Add($"frame-ancestors {String.Join(" ", options.FrameAncestors)};");

            if (options.FrameSources.Count > 0)
                policies.Add($"frame-src {String.Join(" ", options.FrameSources)}");

            if (options.UpgradeInsecureRequests)
                policies.Add("upgrade-insecure-requests;");

            return String.Join(" ", policies);
        }
    }
}
