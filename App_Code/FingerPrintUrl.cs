using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

/* 
 * Technique developed by Mads Kristensen
 * http://madskristensen.net/post/cache-busting-in-aspnet
 */

public class FingerPrintUrl
{
    public static string Create(string url)
    {
        if (String.IsNullOrWhiteSpace(url))
            return String.Empty;

        if (HttpRuntime.Cache[url] == null)
        {
            string relative = VirtualPathUtility.ToAbsolute("~" + url);
            string absolute = HostingEnvironment.MapPath(relative);

            if (!File.Exists(absolute))
                throw new FileNotFoundException("File not found.", absolute);

            DateTime date = File.GetLastWriteTime(absolute);
            int index = relative.LastIndexOf('/');

            string result = relative.Insert(index, "/f-" + date.Ticks);

            HttpRuntime.Cache.Insert(url, result, new CacheDependency(absolute));
        }

        return HttpRuntime.Cache[url] as string;
    }
}