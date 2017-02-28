using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using PersonalSite.Middleware;
using System;

namespace PersonalSite
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(typeof(ApplicationEnvironment), PlatformServices.Default.Application);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
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

            app.UseRemoveServerHeader();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(14),
                        Public = true
                    };
                }
            });

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseCanonicalDomain(Configuration["AppSettings:Domain"], requireHttps: true);

                ConfigureSecurityHeaders(app);
            }

            app.UseStatusCodePagesWithReExecute("/404", statusCode: 404);

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
                    "https://cdn.brentnewbury.com/"
                },
                ScriptSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    ContentSecurityPolicyHeaderOptions.UnsafeInline,
                    ContentSecurityPolicyHeaderOptions.UnsafeEval,
                    "https://cdn.brentnewbury.com/",
                    "https://az416426.vo.msecnd.net/",
                    "https://fonts.googleapis.com/"
                },
                StyleSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    "https://cdn.brentnewbury.com/",
                    "https://fonts.googleapis.com/"
                },
                FontSources =
                {
                    "https://fonts.gstatic.com/"
                },
                ConnectSources =
                {
                    "https://dc.services.visualstudio.com/"
                },
                FrameAncestors =
                {
                    ContentSecurityPolicyHeaderOptions.None
                }
            });
        }
    }
}
