
namespace PersonalSite.Middleware
{
    public class XXSSProtectionHeaderOptions
    {
        public bool Enabled { get; set; }

        public XXSSProtectionMode Mode { get; set; }
    }

    public enum XXSSProtectionMode
    {
        Block
    }
}
