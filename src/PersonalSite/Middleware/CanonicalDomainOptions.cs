namespace PersonalSite.Middleware
{
    public class CanonicalDomainOptions
    {
        public string Domain { get; set; }

        public bool RequireHttps { get; set; }

        public CanonicalDomainOptions()
        {
        }
    }
}
