using System.Collections.Generic;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the <c>Content-Security-Policy</c> header.
    /// </summary>
    public class ContentSecurityPolicyHeaderOptions
    {
        /// <summary>
        /// Prevents loading resources from any source.
        /// </summary>
        public const string None = "'none'";

        /// <summary>
        /// Allows loading resources from the same origin (same scheme, host and port).
        /// </summary>
        public const string Self = "'self'";

        /// <summary>
        /// Allows use of inline source elements such as style attribute, onclick, or script tag bodies.
        /// </summary>
        public const string UnsafeInline = "'unsafe-inline'";

        /// <summary>
        /// Allows unsafe dynamic code evaluation.
        /// </summary>
        public const string UnsafeEval = "'unsafe-eval'";

        /// <summary>
        /// Allows loading resources via the data scheme (eg Base64 encoded images).
        /// </summary>
        public const string Data = "data:";

        /// <summary>
        /// Fallback sources in <c>default-src</c> directive will be used if other directives are not specified.
        /// </summary>
        public List<string> DefaultSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>img-src</c> directive.
        /// </summary>
        public List<string> ImageSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>script-src</c> directive.
        /// </summary>
        public List<string> ScriptSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>style-src</c> directive.
        /// </summary>
        public List<string> StyleSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>font-src</c> directive.
        /// </summary>
        public List<string> FontSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>connect-src</c> directive.
        /// </summary>
        public List<string> ConnectSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>frame-src</c> directive.
        /// </summary>
        public List<string> FrameSources { get; } = new List<string>();

        /// <summary>
        /// Sources for the <c>frame-ancestors</c> directive.
        /// </summary>
        public List<string> FrameAncestors { get; } = new List<string>();

        /// <summary>
        /// If <see langword="true" />, adds the <c>upgrade-insecure-request</c> directive 
        /// into the <c>Content-Security-Policy</c> to instruct the browser to treat regular
        /// HTTP URLs as HTTPS URLs.
        /// </summary>
        public bool UpgradeInsecureRequests { get; set; }

        /// <summary>
        /// Initialises a new <see cref="ContentSecurityPolicyHeaderOptions"/> instance.
        /// </summary>
        public ContentSecurityPolicyHeaderOptions()
        {
        }
    }
}
