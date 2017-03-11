namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the canonical domain.
    /// </summary>
    public class CanonicalDomainOptions
    {
        /// <summary>
        /// Gets or sets the canonical domain name.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets whether to redirect to HTTPS.
        /// </summary>
        public bool RequireHttps { get; set; }
    }
}
