using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace PersonalSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Global : System.Web.HttpApplication
    {
        public const string SiteTitle = "Brent Newbury";
        private const string NoIndexSettingName = "NO_INDEX";
        private const string NoIndex = "noindex";
        private const string Index = "index,follow";

        public static string Robots
        {
            get;
            private set;
        }

        protected void Application_Start()
        {
            Robots = (String.IsNullOrEmpty(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting(NoIndexSettingName))) ? Index : NoIndex;

            // Ensure only a single view engine is loads
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            // Removes the X-AspNetWebPages-Version from response headers
            WebPageHttpHandler.DisableWebPagesResponseHeader = true;
        }
    }
}