using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSite.Tests
{
    public static class PersonalSiteTestServer
    {
        public static TestServer Create(Action<IApplicationBuilder> configureApp)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("webroot", ".")
            });
            return TestServer.Create(configurationBuilder.Build(), configureApp, configureServices: null);
        }
    }
}
