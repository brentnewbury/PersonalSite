namespace PersonalSite.Middleware
{
    public class ReferrerPolicyHeaderOptions
    {
        public ReferrerPolicy Policy { get; set; } = ReferrerPolicy.NoReferrerWhenDowngrade;

        public ReferrerPolicyHeaderOptions()
        {
        }

        public ReferrerPolicyHeaderOptions(ReferrerPolicy policy)
        {
            Policy = policy;
        }
    }

    public class ReferrerPolicy
    {
        public static ReferrerPolicy NoReferrer = new ReferrerPolicy("no-referrer");
        public static ReferrerPolicy NoReferrerWhenDowngrade = new ReferrerPolicy("no-referrer-when-downgrade");
        public static ReferrerPolicy SameOrigin = new ReferrerPolicy("same-origin");
        public static ReferrerPolicy Origin = new ReferrerPolicy("origin");
        public static ReferrerPolicy StrictOrigin = new ReferrerPolicy("strict-origin");
        public static ReferrerPolicy OriginWhenCrossOrigin = new ReferrerPolicy("origin-when-cross-origin");
        public static ReferrerPolicy StrictOriginWhenCrossOrigin = new ReferrerPolicy("strict-origin-when-cross-origin");
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