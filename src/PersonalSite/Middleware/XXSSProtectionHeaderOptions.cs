namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the <c>X-XSS-Protection</c> header.
    /// </summary>
    public class XXSSProtectionHeaderOptions
    {
        /// <summary>
        /// Gets or sets a value whether to enable or disable XSS filtering.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value whether to block rendering of the page if an attack is detected.
        /// </summary>
        public XXSSProtectionMode Mode { get; set; }
    }

    /// <summary>
    /// Values of the <c>block</c> directive.
    /// </summary>
    public enum XXSSProtectionMode
    {
        /// <summary>
        /// Instructs the browser to block rendering of the page if an attack is detected.
        /// </summary>
        Block
    }
}
