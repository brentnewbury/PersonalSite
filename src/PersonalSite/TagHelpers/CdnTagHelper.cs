using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;

namespace PersonalSite.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;link&gt;, &lt;meta&gt;, &lt;script&gt;, &lt;img&gt; elements that supports replacing paths to instead load from a CDN.
    /// </summary>
    [HtmlTargetElement(LinkTag, Attributes = Attributes)]
    [HtmlTargetElement(MetaTag, Attributes = Attributes)]
    [HtmlTargetElement(ScriptTag, Attributes = Attributes)]
    [HtmlTargetElement(ImgTag, Attributes = Attributes)]
    public class CdnTagHelper : UrlResolutionTagHelper
    {
        private const string LinkTag = "link";
        private const string MetaTag = "meta";
        private const string ScriptTag = "script";
        private const string ImgTag = "img";
        private const string HrefAttribute = "href";
        private const string ContentAttribute = "content";
        private const string SrcAttribute = "src";
        private const string Attributes = CdnUriAttribute + "," + AppendVersionAttribute;
        private const string CdnUriAttribute = "cdn-uri";
        private const string AppendVersionAttribute = "cdn-append-version";

        private readonly Dictionary<string, string> pathAttribues = new Dictionary<string, string>
        {
            [LinkTag] = HrefAttribute,
            [MetaTag] = ContentAttribute,
            [ScriptTag] = SrcAttribute,
            [ImgTag] = SrcAttribute
        };

        private readonly IHostingEnvironment _env;
        private readonly IMemoryCache _cache;
        private FileVersionProvider _fileVersionProvider;

        /// <summary>
        /// The URI to a CDN location that will deliver the resource.
        /// </summary>
        [HtmlAttributeName(CdnUriAttribute)]
        public string CdnUri { get; set; }

        /// <summary>
        /// Value indicating if file version should be appended to the href urls.
        /// </summary>
        /// <remarks>
        /// If <see langword="true">true</see> then a query string "v" with the encoded content of the file is added.
        /// </remarks>
        [HtmlAttributeName(AppendVersionAttribute)]
        public bool AppendVersion { get; set; }

        /// <summary>
        /// Creates a new <see cref="CdnTagHelper"/>.
        /// </summary>
        /// <param name="env">The <see cref="IHostingEnvironment"/>.</param>
        /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
        /// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory"/>.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/>.</param>
        public CdnTagHelper(IHostingEnvironment env, IMemoryCache cache, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder)
            : base(urlHelperFactory, htmlEncoder)
        {
            _env = env;
            _cache = cache;
        }

        /// <summary>
        /// Processes the tag helper.
        /// </summary>
        /// <param name="context">The <see cref="TagHelperContext"/> instance.</param>
        /// <param name="output">The <see cref="TagHelperOutput"/> instance.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Don't load from CDN in development
            if (_env.IsDevelopment())
                return;

            var cdnUri = (CdnUri.EndsWith("/")) ? CdnUri.TrimEnd('/') : CdnUri;

            if (String.IsNullOrWhiteSpace(cdnUri))
                return;

            if (!pathAttribues.ContainsKey(output.TagName))
                return;

            var pathAttributeName = pathAttribues[output.TagName];

            if (!output.Attributes.ContainsName(pathAttributeName))
                return;

            ProcessUrlAttribute(pathAttributeName, output);

            var path = output.Attributes[pathAttributeName].Value?.ToString();
            if (String.IsNullOrWhiteSpace(path))
                return;

            var index = path.IndexOf('?');
            if (index > 0)
                path = path.Substring(index);

            // Don't change path if it's absolute
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
                return;

            if (AppendVersion)
            {
                EnsureFileVersionProvider();

                path = _fileVersionProvider.AddFileVersionToPath(path);
            }

            if (path.StartsWith("~/"))
                path = path.Substring(1);

            if (!path.StartsWith("/"))
                path = "/" + path;

            var cdnPath = cdnUri + path;

            output.Attributes.SetAttribute(pathAttributeName, cdnPath);
        }

        private void EnsureFileVersionProvider()
        {
            if (_fileVersionProvider != null)
                return;

            _fileVersionProvider = new FileVersionProvider(
                _env.WebRootFileProvider,
                _cache,
                ViewContext.HttpContext.Request.PathBase
                );
        }
    }
}
