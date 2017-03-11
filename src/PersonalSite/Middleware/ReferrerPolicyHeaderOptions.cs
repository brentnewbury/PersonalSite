namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the <c>Referrer-Policy</c> header.
    /// </summary>
    public class ReferrerPolicyHeaderOptions
    {
        /// <summary>
        /// Gets or sets the <c>Referrer-Policy</c> directive.
        /// </summary>
        public ReferrerPolicy Policy { get; set; } = ReferrerPolicy.NoReferrerWhenDowngrade;

        /// <summary>
        /// Initialises a new <see cref="ReferrerPolicyHeaderOptions"/> instance.
        /// </summary>
        public ReferrerPolicyHeaderOptions()
        {
        }

        /// <summary>
        /// Initialises a new <see cref="ReferrerPolicyHeaderOptions"/> instance with a specified policy.
        /// </summary>
        /// <param name="policy">The referrer policy to use.</param>
        public ReferrerPolicyHeaderOptions(ReferrerPolicy policy)
        {
            Policy = policy;
        }
    }

    /// <summary>
    /// The <c>Referrer-Policy</c> directive.
    /// </summary>
    public class ReferrerPolicy
    {
        /// <summary>
        /// The <c>no-referrer</c> policy directive. The <c>Referer</c> header is omitted entirely.
        /// </summary>
        public static ReferrerPolicy NoReferrer = new ReferrerPolicy("no-referrer");

        /// <summary>
        /// The <c>no-referrer-when-downgrade</c> policy directive. The <c>Referer</c> heaper is omitted when navigating to less secure sites.
        /// </summary>
        public static ReferrerPolicy NoReferrerWhenDowngrade = new ReferrerPolicy("no-referrer-when-downgrade");

        /// <summary>
        /// The <c>same-origin</c> policy directive. The <c>Referer</c> header will be omitted when navigating to a different domain/subdomain, port, or protocol.
        /// </summary>
        public static ReferrerPolicy SameOrigin = new ReferrerPolicy("same-origin");

        /// <summary>
        /// The <c>origin</c> policy directive. The <c>Referer</c> header will not contain the path when navigating.
        /// </summary>
        public static ReferrerPolicy Origin = new ReferrerPolicy("origin");

        /// <summary>
        /// The <c>strict-origin</c> policy directive. The <c>Referer</c> header will not contain the path when navigating, and omit the <c>Referer</c> header entirely when navigting to less secure sites.
        /// </summary>
        public static ReferrerPolicy StrictOrigin = new ReferrerPolicy("strict-origin");

        /// <summary>
        /// The <c>origin-when-cross-origin</c> policy directive. The <c>Referer</c> header will not contain the full path when navigiting to a different domain/subdomain, port, or protocol.
        /// </summary>
        public static ReferrerPolicy OriginWhenCrossOrigin = new ReferrerPolicy("origin-when-cross-origin");

        /// <summary>
        /// The <c>strict-origin-when-cross-origin</c> policy directive. The <c>Referer</c> header will not contain the full path when navigatign to a different domaon/subdomain, port, or protocol, and omit the <c>Referer</c> header entirely when navigating to a less secure site.
        /// </summary>
        public static ReferrerPolicy StrictOriginWhenCrossOrigin = new ReferrerPolicy("strict-origin-when-cross-origin");

        /// <summary>
        /// The <c>unsafe-url</c> policy directive. The <c>Referer</c> header will contain the full URL when navigating.
        /// </summary>
        public static ReferrerPolicy UnsafeUrl = new ReferrerPolicy("unsafe-url");

        internal string Value { get; }

        private ReferrerPolicy()
        {
        }

        private ReferrerPolicy(string value)
        {
            Value = value;
        }
    }
}