namespace PersonalSite.Middleware
{
    /// <summary>
    /// Options for the <c>X-Frame-Options</c> header.
    /// </summary>
    public class XFrameOptions
    {
        /// <summary>
        /// Gets or sets the <c>X-Frame-Options</c> directive.
        /// </summary>
        public XFrameOption Option { get; set; } = XFrameOption.Deny;

        /// <summary>
        /// Initialises a new <see cref="XFrameOptions"/> instance.
        /// </summary>
        public XFrameOptions()
        {
        }

        /// <summary>
        /// Initialises a new <see cref="XFrameOptions"/> instance.
        /// </summary>
        /// <param name="option">The <see cref="XFrameOption"/> to use.</param>
        public XFrameOptions(XFrameOption option)
        {
            Option = option;
        }
    }

    /// <summary>
    /// Directives for the <c>X-Frame-Option</c> header.
    /// </summary>
    public class XFrameOption
    {
        /// <summary>
        /// The page cannot be displayed in a frame.
        /// </summary>
        public static XFrameOption Deny = new XFrameOption("DENY");

        /// <summary>
        /// The page can only be displayed in a frame from the same origin as the page itself.
        /// </summary>
        public static XFrameOption SameOrigin = new XFrameOption("SAMEORIGIN");

        internal string Value { get; }

        private XFrameOption()
        {
        }

        private XFrameOption(string value)
        {
            Value = value;
        }
    }
}
