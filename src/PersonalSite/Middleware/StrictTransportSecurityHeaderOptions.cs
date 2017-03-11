using System;

namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the <c>Strict-Transport-Security</c> header.
    /// </summary>
    public class StrictTransportSecurityHeaderOptions
    {
        /// <summary>
        /// Gets or sets the <c>max-age</c> directive, instructing the browser how long it should remember to always issue requests via HTTPS.
        /// </summary>
        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(30);

        /// <summary>
        /// Gets or sets whether to emit the <c>includeDubDomains</c> directive, to apply the rule to the subdomains as well.
        /// </summary>
        public bool IncludeSubDomains { get; set; }

        /// <summary>
        /// Gets or sets whether to emit the <c>preload</c> directive, allowing browsers to preload the domain.
        /// </summary>
        public bool Preload { get; set; }

        /// <summary>
        /// Initialises a new <see cref="StrictTransportSecurityHeaderOptions"/> instance.
        /// </summary>
        public StrictTransportSecurityHeaderOptions()
        {
        }
    }
}
