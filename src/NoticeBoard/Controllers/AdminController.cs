using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Markdig;
using NoticeBoard.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;

namespace NoticeBoard.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ContentContext _contentCtx;
        private const string PagesKey = "PageList";

        public AdminController(IMemoryCache memoryCache, ContentContext contentContext)
        {
            _memoryCache = memoryCache;
            _contentCtx = contentContext;
        }

        public IActionResult Index()
        {
            return View(_contentCtx.Pages.ToList());
        }

        public IActionResult Editor()
        {
            ViewData["Message"] = "Edit your page content.";

            string markdown;

            string path = Request.Query["path"];

            Content content = _contentCtx.Contents.Where(c => c.Path == path).OrderByDescending(c => c.Id).FirstOrDefault();
            if (content == null)
                markdown = String.Empty;
            else
                markdown = content.Markdown;

            return View((object)markdown);
        }

        [HttpPost]
        public IActionResult GetPreview(string markdown)
        {
            if (markdown == null)
                return NoContent();
            else
            {
                var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
                var html = Markdown.ToHtml(markdown, pipeline);
                return Content(html);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveContent(string path, string content)
        {
            Content item = new Content()
            {
                Path = path,
                Markdown = content,
                CreatedDate = DateTime.Now,
                Username = User.Identity.Name,
            };
            _contentCtx.Contents.Add(item);

            if (await _contentCtx.SaveChangesAsync() > 0)
                _memoryCache.Remove(path);

            return Redirect(path);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPage(Page page)
        {
            if (ModelState.IsValid)
            {
                _contentCtx.Pages.Add(page);
                if (await _contentCtx.SaveChangesAsync() > 0)
                    _memoryCache.Remove(PagesKey);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid data");

                return View(page);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePage(Page page)
        {
            if (ModelState.IsValid)
            {
                _contentCtx.Pages.Update(page);
                if (await _contentCtx.SaveChangesAsync() > 0)
                    _memoryCache.Remove(PagesKey);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid data");

                return View(page);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePage(string pageName)
        {
            Page page = _contentCtx.Pages.Single(m => m.Pagename == pageName);

            _contentCtx.Pages.Remove(page);
            if (await _contentCtx.SaveChangesAsync() > 0)
                _memoryCache.Remove(PagesKey);

            return RedirectToAction("Index");
        }
    }
}
