using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace PersonalSite.Tests
{
    public static class PersonalSiteTestServer
    {
        public static TestServer Create(Action<IApplicationBuilder> configureApp)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("webroot", ".")
                })
                .Build();

            var builder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .Configure(configureApp);

            return new TestServer(builder);
        }
    }
}
