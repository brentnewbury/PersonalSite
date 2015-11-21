using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalSite.Middleware;
using System;

namespace PersonalSite
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsProduction())
            {
                app.UseApplicationInsightsRequestTelemetry();
            }

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Error");
            }

            if (env.IsProduction())
            {
                app.UseApplicationInsightsExceptionTelemetry();
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            if (env.IsProduction() || env.IsEnvironment("Staging"))
            {
                app.UseCanonicalDomain(Configuration["AppSettings:Domain"], requireHttps: true);

                ConfigureSecurityHeaders(app);

                app.UseStatusCodePagesWithReExecute("/404", statusCode: 404);
            }

            app.UseStaticFiles();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{action=Index}",
                    defaults: new { controller = "Home" }
                    );
            });
        }

        public static void ConfigureSecurityHeaders(IApplicationBuilder app)
        {
            app.UseStrictTransportSecurityHeader(maxAge: TimeSpan.FromDays(365), includeSubDomains: true);

            app.UseXContentTypeOptionsHeader();

            app.UseXFrameOptionsHeader(XFrameOption.Deny);

            app.UseXXSSProtectionHeader(enabled: true, mode: XXSSProtectionMode.Block);

            app.UseContentSecurityPolicyHeader(new ContentSecurityPolicyHeaderOptions
            {
                DefaultSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self
                },
                ImageSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    ContentSecurityPolicyHeaderOptions.Data,
                    "https://az797012.vo.msecnd.net/"
                },
                ScriptSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    ContentSecurityPolicyHeaderOptions.UnsafeInline,
                    ContentSecurityPolicyHeaderOptions.UnsafeEval,
                    "https://az797012.vo.msecnd.net/",
                    "https://fonts.googleapis.com/"
                },
                StyleSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    "https://az797012.vo.msecnd.net/",
                    "https://fonts.googleapis.com/"
                },
                FontSources =
                {
                    "https://fonts.gstatic.com/"
                },
                ConnectSources =
                {
                    "https://dc.services.visualstudio.com/"
                }
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
