using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using NoticeBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoticeBoard.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "asp-markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-markdown")]
        public bool IsMarkdown { get; set; }

        [HtmlAttributeName("asp-minify")]
        public bool IsMinify { get; set; }

        private readonly IUrlHelper _urlHelper;
        private readonly ContentContext _contentCtx;
        private readonly IMemoryCache _memoryCache;

        public MarkdownTagHelper(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory, ContentContext ContentContext, IMemoryCache memoryCache)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _contentCtx = ContentContext;
            _memoryCache = memoryCache;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string path = _urlHelper.Action();

            string html = _memoryCache.Get<string>(path);
            if (html == null)
            {
                html = _memoryCache.Set<string>(path, await GetContentAsync(output, path),
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15)));
            }
            output.Content.SetHtmlContent(html);
        }

        private async Task<string> GetContentAsync(TagHelperOutput output, string path)
        {
            string markdown;

            Content content = _contentCtx.Contents.Where(c => c.Path == path).OrderByDescending(c => c.Id).FirstOrDefault();
            if (content == null)
            {
                var inner = await output.GetChildContentAsync();
                markdown = inner.GetContent();
            }
            else
                markdown = content.Markdown;

            string html = Markdown.ToHtml(markdown, new MarkdownPipelineBuilder().UsePipeTables().Build());

            if (IsMinify)
                html = html.Replace("\t", String.Empty).Replace("\r", String.Empty).Replace("\n", String.Empty).Trim(' ');

            return html;
        }
    }
}
