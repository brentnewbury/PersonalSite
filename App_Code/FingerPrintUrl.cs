using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

/* 
 * Adapted from technique developed by Mads Kristensen
 * http://madskristensen.net/post/cache-busting-in-aspnet
 */

public class FingerPrintUrl
{
    private const string CdnUrl = "//az680800.vo.msecnd.net/cdn";

    public static string Create(string url)
    {
        if (String.IsNullOrWhiteSpace(url))
            return String.Empty;

        if (HttpRuntime.Cache[url] == null)
        {
            string abolutePath = VirtualPathUtility.ToAbsolute("~" + url);
            string physicalPath = HostingEnvironment.MapPath(abolutePath);

            if (!File.Exists(physicalPath))
                return url;

            DateTime date = File.GetLastWriteTime(physicalPath);

            string result = url + "?v=" + date.Ticks;

            if (!HttpContext.Current.Request.Url.IsLoopback)
                result = CdnUrl + result;

            HttpRuntime.Cache.Insert(url, result, new CacheDependency(physicalPath));
        }

        return HttpRuntime.Cache[url] as string;
    }
}