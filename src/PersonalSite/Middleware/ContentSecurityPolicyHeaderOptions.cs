using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSite.Middleware
{
    public class ContentSecurityPolicyHeaderOptions
    {
        public const string Self = "'self'";
        public const string Inline = "'inline'";
        public const string UnsafeInline = "'unsafe-inline'";
        public const string UnsafeEval = "'unsafe-eval'";
        public const string Data = "data:";

        public List<string> DefaultSources { get; } = new List<string>();
        public List<string> ImageSources { get; } = new List<string>();
        public List<string> ScriptSources { get; } = new List<string>();
        public List<string> StyleSources { get; } = new List<string>();
        public List<string> FontSources { get; } = new List<string>();
        public List<string> ConnectSources { get; } = new List<string>();

        public ContentSecurityPolicyHeaderOptions()
        {
        }
    }
}
