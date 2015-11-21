using System;

namespace PersonalSite.Middleware
{
    public class StrictTransportSecurityHeaderOptions
    {
        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(30);

        public bool IncludeSubDomains { get; set; }

        public bool Preload { get; set; }

        public StrictTransportSecurityHeaderOptions()
        {
        }
    }
}
