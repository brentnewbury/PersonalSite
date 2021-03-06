﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using PersonalSite.Middleware;

namespace PersonalSite
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseCanonicalDomain(Configuration["AppSettings:Domain"], requireHttps: true);

                ConfigureSecurityHeaders(app);
            }

            app.UseStatusCodePagesWithReExecute("/404", statusCode: 404);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        public static void ConfigureSecurityHeaders(IApplicationBuilder app)
        {
            app.UseStrictTransportSecurityHeader(maxAge: TimeSpan.FromDays(365), includeSubDomains: true, preload: true);

            app.UseXContentTypeOptionsHeader();

            app.UseXFrameOptionsHeader(XFrameOption.Deny);

            app.UseXXSSProtectionHeader(enabled: true, mode: XXSSProtectionMode.Block);

            app.UseReferrerPolicyHeader(ReferrerPolicy.StrictOriginWhenCrossOrigin);

            app.UseContentSecurityPolicyHeader(new ContentSecurityPolicyHeaderOptions
            {
                DefaultSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self
                },
                ImageSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    ContentSecurityPolicyHeaderOptions.Data
                },
                ScriptSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
                    ContentSecurityPolicyHeaderOptions.UnsafeInline,
                    ContentSecurityPolicyHeaderOptions.UnsafeEval,
                    "https://az416426.vo.msecnd.net/",
                    "https://fonts.googleapis.com/"
                },
                StyleSources =
                {
                    ContentSecurityPolicyHeaderOptions.Self,
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
                },
                UpgradeInsecureRequests = true
            });
        }
    }
}
