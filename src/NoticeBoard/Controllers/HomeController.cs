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
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ContentContext _contentCtx;

        public HomeController(IMemoryCache memoryCache, ContentContext contentContext)
        {
            _memoryCache = memoryCache;
            _contentCtx = contentContext;
        }

        public IActionResult Index(string name)
        {
            if (name == null)
            {
                ViewData["Title"] = "Home Page";
                return View();
            }

            var pages = _memoryCache.Get<IEnumerable<Page>>("PageList");
            if (pages == null)
            {
                pages = _memoryCache.Set<IEnumerable<Page>>("PageList", _contentCtx.Pages.ToList(),
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15)));
            }

            var page = pages.FirstOrDefault(p => p.Pagename == name);
            if (page == null)
                return NotFound();
            else
            {
                ViewData["Title"] = page.Title;
                return View();
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
