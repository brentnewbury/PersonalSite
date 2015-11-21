namespace PersonalSite.Middleware
{
    public class XFrameOptions
    {
        public XFrameOption Option { get; set; } = XFrameOption.Deny;

        public XFrameOptions()
        {
        }

        public XFrameOptions(XFrameOption option)
        {
            Option = option;
        }
    }

    public enum XFrameOption
    {
        Deny,
        SameOrigin
    }
}
